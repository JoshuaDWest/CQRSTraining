using System.Threading;
using System.Threading.Tasks;
using ReadStack.Queries;
using ReadStack.ReadModel;
using ReadStack.Repositories;
using Xer.Cqrs.QueryStack;

namespace ReadStack.QueryHandlers
{
    internal class QueryActiveMatchHandler : IQueryAsyncHandler<QueryActiveMatch, MatchReadModel>
    {
        private readonly IMatchReadStackRepository _repo;

        public QueryActiveMatchHandler(IMatchReadStackRepository repo)
        {
            _repo = repo;
        }

        public Task<MatchReadModel> HandleAsync(QueryActiveMatch query, CancellationToken cancellationToken = new CancellationToken())
        {
            return _repo.GetActiveMatch(cancellationToken);
        }
    }
}
