using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandStack.Commands.Internal;
using Models.Enums;
using Models.Events;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.EventStack;

namespace CommandStack.Sagas
{
    public class GameSaga : IEventAsyncHandler<MatchStartedEvent>,
                            IEventAsyncHandler<DeployedEvent>,
                            IEventAsyncHandler<DestroyedEvent>
    {
        private Guid _currentMatchId;
        private readonly CommandDelegator _commandDelegator;
        private Dictionary<string, PlayerPieceCount> _counts;

        public GameSaga(CommandDelegator commandDelegator)
        {
            _commandDelegator = commandDelegator;
            _counts = new Dictionary<string, PlayerPieceCount>();
        }
        public Task HandleAsync(MatchStartedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            _counts.Clear();
            _currentMatchId = @event.AggregateRootId;
            return Task.CompletedTask;
        }

        public Task HandleAsync(DeployedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            EnsurePlayerExists(@event.Player);
            switch (@event.Type)
            {
                case UnitType.TankHeavy:
                    _counts[@event.Player].HeavyTankCount++;
                    break;
                case UnitType.TankLight:
                    _counts[@event.Player].LightTankCount++;
                    break;
                case UnitType.PlaneBomber:
                    _counts[@event.Player].BomberCount++;
                    break;
                case UnitType.PlaneFighter:
                    _counts[@event.Player].FighterPlaneCount++;
                    break;
                case UnitType.Infantry:
                    _counts[@event.Player].SoldierCount++;
                    break;
            }
            return Task.CompletedTask;
        }

        public Task HandleAsync(DestroyedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            switch (@event.Type)
            {
                case UnitType.TankHeavy:
                    _counts[@event.Player].HeavyTankCount--;
                    break;
                case UnitType.TankLight:
                    _counts[@event.Player].LightTankCount--;
                    break;
                case UnitType.PlaneBomber:
                    _counts[@event.Player].BomberCount--;
                    break;
                case UnitType.PlaneFighter:
                    _counts[@event.Player].FighterPlaneCount--;
                    break;
                case UnitType.Infantry:
                    _counts[@event.Player].SoldierCount--;
                    break;
            }

            if (GameIsOver())
            {
                var winner = _counts.Single(m => !m.Value.NoPieces).Key;
                _commandDelegator.SendAsync(new EndGameCommand(_currentMatchId, winner));
            }
            return Task.CompletedTask;
        }

        private bool GameIsOver()
        {
            foreach (var player in _counts)
            {
                if (player.Value.NoPieces)
                {
                    return true;
                }
            }

            return false;
        }

        private void EnsurePlayerExists(string player)
        {
            if (!_counts.ContainsKey(player))
            {
                _counts.Add(player, new PlayerPieceCount());
            }
        }

        public class PlayerPieceCount
        {
            public int HeavyTankCount { get; set; }
            public int LightTankCount { get; set; }
            public int FighterPlaneCount { get; set; }
            public int BomberCount { get; set; }
            public int SoldierCount { get; set; }

            public bool NoPieces => (HeavyTankCount | LightTankCount | FighterPlaneCount | BomberCount | SoldierCount) == 0;
        }
    }
}
