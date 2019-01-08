using System.Threading;
using System.Threading.Tasks;
using CommandStack.Commands;
using Xer.Cqrs.CommandStack;
using Xer.DomainDriven.Repositories;

namespace CommandStack.CommandHandler
{
    public class StartNewGameCommandHandler : ICommandAsyncHandler<StartNewGameCommand>
    {
        private readonly IAggregateRootRepository<Match> _matchRepository;

        public StartNewGameCommandHandler(IAggregateRootRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public Task HandleAsync(StartNewGameCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            var match = new Match(command.Id);
            match.Apply(command);
            return _matchRepository.SaveAsync(match);
        }
    }
}
