using System;
using System.Threading;
using System.Threading.Tasks;
using CommandStack.Commands;
using CommandStack.Utilities;
using Xer.Cqrs.CommandStack;
using Xer.DomainDriven.Repositories;

namespace CommandStack.CommandHandler
{
    public class SubmitMovesCommandHandler : ICommandAsyncHandler<SubmitMovesCommand>
    {
        private IAggregateRootRepository<Match> _matchRepository;

        public SubmitMovesCommandHandler(IAggregateRootRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }
        public async Task HandleAsync(SubmitMovesCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!DeploymentCreditValidator.ValidDeployment(command.Deployments))
            {
                throw new ArgumentException("Exceeded deployment credit allotment for move");
            }

            var currentMatch = await _matchRepository.GetByIdAsync(command.Match);
            if (currentMatch == null)
            {
                throw new ArgumentException("The match could not be found");
            }
            currentMatch.Apply(command);
            await _matchRepository.SaveAsync(currentMatch, cancellationToken);
        }
    }
}
