using System;
using System.Threading;
using System.Threading.Tasks;
using CommandStack.Commands.Internal;
using Xer.Cqrs.CommandStack;
using Xer.DomainDriven.Repositories;

namespace CommandStack.CommandHandler.Internal
{
    internal class EndGameCommandHandler : ICommandAsyncHandler<EndGameCommand>
    {
        private readonly IAggregateRootRepository<Match> _matchRepository;

        public EndGameCommandHandler(IAggregateRootRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task HandleAsync(EndGameCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            var currentMatch = await _matchRepository.GetByIdAsync(command.MatchId);
            if (currentMatch == null)
            {
                throw new ArgumentException("The match could not be found");
            }
            currentMatch.Apply(command);
            await _matchRepository.SaveAsync(currentMatch, cancellationToken);
        }
    }
}
