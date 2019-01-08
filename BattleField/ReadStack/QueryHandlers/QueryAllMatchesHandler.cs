using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReadStack.Queries;
using ReadStack.ReadModel;
using ReadStack.Repositories;
using Xer.Cqrs.QueryStack;

namespace ReadStack.QueryHandlers
{
    public class QueryAllMatchesHandler : IQueryAsyncHandler<QueryAllMatches, IReadOnlyCollection<MatchReadModel>>
    {
        private readonly IMatchReadStackRepository _repo;

        public QueryAllMatchesHandler(IMatchReadStackRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyCollection<MatchReadModel>> HandleAsync(QueryAllMatches query, CancellationToken cancellationToken = new CancellationToken())
        {
            return _repo.GetAllMatches(cancellationToken);
        }
    }
}
