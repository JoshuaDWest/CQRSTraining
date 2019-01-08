using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            existingMatch.Active = match.Active;
            return Task.CompletedTask;
        }
    }
}
