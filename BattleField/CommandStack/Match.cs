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
        public int CurrentTurn { get; private set; }
        public bool ActiveMatch { get; private set; }
        public string Winner { get; private set; }

        public List<Move> ActiveTurnMoves { get; }
        public Dictionary<Guid, IUnit> Units { get; }

        public Match(Guid aggregateRootId) : base(aggregateRootId)
        {
            RegisterDomainEventAppliers();

            ActiveTurnMoves = new List<Move>();
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
                    ApplyDomainEvent(new MoveSubmittedEvent(Id, CurrentTurn, command.Player, move.Source, move.Target));
                }

                ApplyDomainEvent(new AllPlayerMovesSubmittedForTurnEvent(Id, command.Player, CurrentTurn));
            }
        }

        internal void Apply(ProcessTurnCommand command)
        {
            foreach(var move in ActiveTurnMoves)
            {
                if (move.Deploy) //deployed
                {
                    ApplyDomainEvent(new DeployedEvent(Id, Guid.NewGuid(), move.Player, move.Source));
                }
                else //attack
                {
                    var attackResults = AttackCalculator.Attack(move.Source, move.Target);
                    if (attackResults.success)
                    {
                        ApplyDomainEvent(new DamagedEvent(Id, move.AttackedId, move.Target, attackResults.dmg));

                        //check for destruction
                        if(Units[move.AttackedId].Health <  attackResults.dmg)
                            ApplyDomainEvent(new DestroyedEvent(Id, move.AttackedId, Units[move.AttackedId].OwnedByPlayer, Units[move.AttackedId].Type));
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
        }

        internal void Apply(EndGameCommand command)
        {
            ApplyDomainEvent(new MatchEndedEvent(Id, command.Winner));
        }
        #endregion

        #region Event Handling
        public void RegisterDomainEventAppliers()
        {
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
            ActiveTurnMoves.Add(new Move{ Player = devent.Player, Source = devent.Source, Target = devent.Target});
        }

        private void OnMatchTurnEndedEvent(MatchTurnEndedEvent devent)
        {
            ActiveTurnMoves.Clear();
            CurrentTurn++;
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

        public class Move
        {
            /// <summary>
            /// Whether or not this is a deployment move
            /// </summary>
            public bool Deploy { get; set; }
            public string Player { get; set; }
            public UnitType Source { get; set; }
            public UnitType Target { get; set; }
            /// <summary>
            /// The id of the thing being attacked if this is an attack move
            /// </summary>
            public Guid AttackedId { get; set; }
        }
    }
}
