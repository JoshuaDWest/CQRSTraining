using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.Enums;
using ReadStack.ReadModel;

namespace ReadStack.Repositories
{
    public interface IMatchReadStackRepository
    {
        Task<MatchReadModel> GetActiveMatch(CancellationToken cancellationToken = default(CancellationToken));
        Task<IReadOnlyCollection<MatchReadModel>> GetAllMatches(CancellationToken cancellationToken = default(CancellationToken));
        Task AddMatch(MatchReadModel match, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateMatch(MatchReadModel match, CancellationToken cancellationToken = default(CancellationToken));
        Task AddUnit(Guid match, UnitType type, string player, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveUnit(Guid match, UnitType type, string player, CancellationToken cancellationToken = default(CancellationToken));
    }
}
