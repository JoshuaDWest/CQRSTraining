using System;
using System.Collections.Generic;
using System.Linq;
using CommandStack.Commands;
using CommandStack.Commands.Internal;
using CommandStack.Utilities;
using Models.Enums;
using Models.Events;
using Models.Events.Errors;
using Xer.DomainDriven;

namespace CommandStack
{
    public class Match : AggregateRoot
    {
        private readonly IRandomizer _rand;
        public int CurrentTurn { get; private set; }
        public bool ActiveMatch { get; private set; }
        public string Winner { get; private set; }

        public List<MoveSubmittedEvent> ActiveTurnMoves { get; }
        public Dictionary<Guid, IUnit> Units { get; }

        public Match(Guid aggregateRootId) : this(aggregateRootId, new BattleRandomizer())
        {
        }

        internal Match(Guid aggregateRootId, IRandomizer rand) : base(aggregateRootId)
        {
            _rand = rand;

            RegisterDomainEventAppliers();

            ActiveTurnMoves = new List<MoveSubmittedEvent>();
            Units = new Dictionary<Guid, IUnit>();
        }

        #region Command Handling
        public void Apply(StartNewGameCommand command)
        {
            if(ActiveMatch)
                ApplyDomainEvent(new MatchEndedEvent(Id, String.Empty));
            ApplyDomainEvent(new MatchStartedEvent(Id));

            //init player unit counts
            InitPlayerUnits("player1");
            InitPlayerUnits("player2");
        }

        public void Apply(SubmitMovesCommand command)
        {
            if (!ActiveMatch)
            {
                ApplyDomainEvent(new MoveSubmittedToInactiveMatchEvent(Id));
            }
            else if(ActiveTurnMoves.Any(m => m.Player == command.Player))
            {
                ApplyDomainEvent(new PlayerDoubleSubmittedMoveEvent(Id, command.Player));
            }
            else
            {
                foreach(var move in command.Deployments)
                {
                    ApplyDomainEvent(new MoveSubmittedEvent(Id, CurrentTurn, command.Player, move.Type));
                }

                foreach(var move in command.Attacks)
                {
                    MakeAllPlayerUnitsOfTypeAttackTargetType(command, move);
                }

                ApplyDomainEvent(new AllPlayerMovesSubmittedForTurnEvent(Id, command.Player, CurrentTurn));
            }
        }

        private void MakeAllPlayerUnitsOfTypeAttackTargetType(SubmitMovesCommand command, Attack move)
        {
            var numberOfAttacksAllowed = Units.Count(m => m.Value.OwnedByPlayer == command.Player && m.Value.Type == move.Source);
            var numberOfOpposingPlayerUnits = Units.Count(m => m.Value.OwnedByPlayer != command.Player && m.Value.Type == move.Target);

            if(numberOfOpposingPlayerUnits > 0)
                for (var i = 0; i < numberOfAttacksAllowed; i++)
                    ApplyDomainEvent(new MoveSubmittedEvent(Id, CurrentTurn, command.Player, move.Source, move.Target));
        }

        internal void Apply(ProcessTurnCommand command)
        {
            foreach(var move in ActiveTurnMoves)
            {
                if (move.Deployment) //deployed
                {
                    ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), move.Player, move.Source));
                }
                else //attack
                {
                    //pick a random unit from the opposing players units in this category to hit
                    var possiblyAttacked = Units.Where(m => m.Value.OwnedByPlayer != move.Player && m.Value.Type == move.Target).ToArray();
                    if (!possiblyAttacked.Any()) continue; // they all died! :(

                    var attacked = possiblyAttacked[_rand.Generate(possiblyAttacked.Length)].Key;
                    var attackResults = AttackCalculator.Attack(_rand, move.Source, move.Target);

                    if (attackResults.success)
                    {
                        ApplyDomainEvent(new DamagedEvent(Id, attacked, move.Target, attackResults.dmg));

                        //check for destruction
                        if(Units[attacked].Health <=  attackResults.dmg)
                            ApplyDomainEvent(new DestroyedEvent(Id, attacked, Units[attacked].OwnedByPlayer, Units[attacked].Type));
                    }
                    else if (attackResults.exc == null)
                    {
                        // haha, failed, do nothing
                    }
                    else
                    {
                        //push an error event...
                    }
                }
            }

            ApplyDomainEvent(new MatchTurnEndedEvent(Id, CurrentTurn));
        }

        internal void Apply(EndGameCommand command)
        {
            ApplyDomainEvent(new MatchEndedEvent(Id, command.Winner));
        }
        #endregion

        #region Event Handling
        public void RegisterDomainEventAppliers()
        {
            RegisterDomainEventApplier<AllPlayerMovesSubmittedForTurnEvent>(OnAllPlayerMovesSubmittedForTurnEvent);
            RegisterDomainEventApplier<MatchStartedEvent>(OnMatchStartedEvent);
            RegisterDomainEventApplier<MoveSubmittedEvent>(OnMoveSubmittedEvent);
            RegisterDomainEventApplier<MatchTurnEndedEvent>(OnMatchTurnEndedEvent);
            RegisterDomainEventApplier<MatchEndedEvent>(OnMatchEndedEvent);

            RegisterDomainEventApplier<DeployedEvent>(OnDeployedEvent);
            RegisterDomainEventApplier<DestroyedEvent>(OnDestroyedEvent);
            RegisterDomainEventApplier<HealthAddedEvent>(OnHealthAddedEvent);
            RegisterDomainEventApplier<DamagedEvent>(OnDamagedEvent);
        }

        private void OnMatchStartedEvent(MatchStartedEvent devent)
        {
            ActiveMatch = true;
            Winner = string.Empty;
            CurrentTurn = 0;
            ActiveTurnMoves.Clear();
            Units.Clear();
        }

        private void OnMoveSubmittedEvent(MoveSubmittedEvent devent)
        {
            ActiveTurnMoves.Add(devent);
        }

        private void OnMatchTurnEndedEvent(MatchTurnEndedEvent devent)
        {
            ActiveTurnMoves.Clear();
            CurrentTurn++;
        }

        private void OnAllPlayerMovesSubmittedForTurnEvent(AllPlayerMovesSubmittedForTurnEvent devent)
        {
        }

        private void OnMatchEndedEvent(MatchEndedEvent devent)
        {
            ActiveMatch = false;
            Winner = devent.Winner;
        }

        private void OnDeployedEvent(DeployedEvent devent)
        {
            switch (devent.Type)
            {
                case UnitType.Infantry:
                    Units.Add(devent.DeployedId, new Soldier(devent.DeployedId, "Billy Bob", devent.Player));
                    break;
                case UnitType.PlaneBomber:
                case UnitType.PlaneFighter:
                    Units.Add(devent.DeployedId, new Plane(devent.DeployedId, devent.Type, devent.Player));
                    break;
                case UnitType.TankHeavy:
                case UnitType.TankLight:
                    Units.Add(devent.DeployedId, new Tank(devent.DeployedId, devent.Type, devent.Player));
                    break;
            }
        }

        private void OnDestroyedEvent(DestroyedEvent devent)
        {
            Units.Remove(devent.UnitId);
        }

        private void OnHealthAddedEvent(HealthAddedEvent devent)
        {
            Units[devent.UnitId].Heal(devent.Amount);
        }

        private void OnDamagedEvent(DamagedEvent devent)
        {
            Units[devent.UnitId].Damage(devent.Amount);
        }
        #endregion

        private void InitPlayerUnits(string player)
        {
            for(var i = 0; i < 20; i++)
                ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.Infantry));

            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.PlaneFighter));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.PlaneFighter));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.PlaneFighter));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.PlaneBomber));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.PlaneBomber));

            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.TankHeavy));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.TankHeavy));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.TankLight));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.TankLight));
            ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), player, UnitType.TankLight));
        }
    }
}
