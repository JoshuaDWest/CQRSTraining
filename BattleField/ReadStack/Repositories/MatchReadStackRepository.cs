using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Constants;
using Models.Enums;
using ReadStack.ReadModel;

namespace ReadStack.Repositories
{
    public class MatchReadStackRepository : IMatchReadStackRepository
    {
        private readonly List<MatchReadModel> _matches = new List<MatchReadModel>();

        public Task<MatchReadModel> GetActiveMatch(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(_matches.Single(m => m.Active));
        }

        public Task<IReadOnlyCollection<MatchReadModel>> GetAllMatches(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult((IReadOnlyCollection<MatchReadModel>)_matches);
        }

        public Task AddMatch(MatchReadModel match, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            _matches.Add(match);

            return Task.CompletedTask;
        }

        public Task UpdateMatch(MatchReadModel match, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(match == null) throw new ArgumentNullException(nameof(match));

            var existingMatch = _matches.SingleOrDefault(m => m.Id == match.Id);
            if(existingMatch == null) throw new ArgumentException("The given match did not exist");
            existingMatch.Update(match);
            
            return Task.CompletedTask;
        }

        public Task AddUnit(Guid match, UnitType type, string player,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var existingMatch = _matches.SingleOrDefault(m => m.Id == match);
            ModifyCount(existingMatch, type, player, 1);

            return Task.CompletedTask;
        }

        public Task RemoveUnit(Guid match, UnitType type, string player,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var existingMatch = _matches.SingleOrDefault(m => m.Id == match);
            ModifyCount(existingMatch, type, player, -1);
            return Task.CompletedTask;
        }

        private void ModifyCount(MatchReadModel match, UnitType type, string player, int change)
        {
            switch (type)
            {
                case UnitType.TankHeavy:
                    if (player == PlayerInfo.PLAYER_ONE)
                    {
                        match.Player1.HeavyTankCount.Count += change;
                    }
                    else if (player == PlayerInfo.PLAYER_TWO)
                    {
                        match.Player2.HeavyTankCount.Count += change;
                    }
                    break;
                case UnitType.TankLight:
                    if (player == PlayerInfo.PLAYER_ONE)
                    {
                        match.Player1.LightTankCount.Count += change;
                    }
                    else if (player == PlayerInfo.PLAYER_TWO)
                    {
                        match.Player2.LightTankCount.Count += change;
                    }
                    break;
                case UnitType.PlaneBomber:
                    if (player == PlayerInfo.PLAYER_ONE)
                    {
                        match.Player1.BomberCount.Count += change;
                    }
                    else if (player == PlayerInfo.PLAYER_TWO)
                    {
                        match.Player2.BomberCount.Count += change;
                    }
                    break;
                case UnitType.PlaneFighter:
                    if (player == PlayerInfo.PLAYER_ONE)
                    {
                        match.Player1.FighterPlaneCount.Count += change;
                    }
                    else if (player == PlayerInfo.PLAYER_TWO)
                    {
                        match.Player2.FighterPlaneCount.Count += change;
                    }
                    break;
                case UnitType.Infantry:
                    if (player == PlayerInfo.PLAYER_ONE)
                    {
                        match.Player1.InfantryCount.Count += change;
                    }
                    else if (player == PlayerInfo.PLAYER_TWO)
                    {
                        match.Player2.InfantryCount.Count += change;
                    }
                    break;
            }
        }
    }
}
