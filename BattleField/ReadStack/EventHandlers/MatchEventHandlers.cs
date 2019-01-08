using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.Events;
using ReadStack.ReadModel;
using ReadStack.Repositories;
using Xer.Cqrs.EventStack;

namespace ReadStack.EventHandlers
{
    internal class MatchEventHandlers : IEventAsyncHandler<MatchStartedEvent>,
                                        IEventAsyncHandler<MatchEndedEvent>
    {
        private readonly IMatchReadStackRepository _repo;

        public MatchEventHandlers(IMatchReadStackRepository repo)
        {
            _repo = repo;
        }

        public async Task HandleAsync(MatchStartedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.AddMatch(new MatchReadModel() {Id = @event.AggregateRootId, Active = true}, cancellationToken);
        }

        public async Task HandleAsync(MatchEndedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.UpdateMatch(new MatchReadModel() {Id = @event.AggregateRootId, Active = false}, cancellationToken);
        }
    }
}
