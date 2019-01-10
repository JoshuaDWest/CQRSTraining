using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.Constants;
using Models.Enums;
using Models.Events;
using ReadStack.ReadModel;
using ReadStack.Repositories;
using Xer.Cqrs.EventStack;

namespace ReadStack.EventHandlers
{
    internal class MatchEventHandlers : IEventAsyncHandler<MatchStartedEvent>,
                                        IEventAsyncHandler<MatchEndedEvent>,
                                        IEventAsyncHandler<DeployedEvent>,
                                        IEventAsyncHandler<DestroyedEvent>
    {
        private readonly IMatchReadStackRepository _repo;

        public MatchEventHandlers(IMatchReadStackRepository repo)
        {
            _repo = repo;
        }

        public async Task HandleAsync(MatchStartedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.AddMatch(new MatchReadModel()
            {
                Id = @event.AggregateRootId,
                Active = true,
                Player1 = PlayerInfoReadModel.New,
                Player2 = PlayerInfoReadModel.New
            }, cancellationToken);
        }

        public async Task HandleAsync(MatchEndedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.UpdateMatch(new MatchReadModel() {Id = @event.AggregateRootId, Active = false}, cancellationToken);
        }

        public async Task HandleAsync(DeployedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.AddUnit(@event.AggregateRootId, @event.Type, @event.Player, cancellationToken);
        }

        public async Task HandleAsync(DestroyedEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            await _repo.RemoveUnit(@event.AggregateRootId, @event.Type, @event.Player, cancellationToken);
        }
    }
}
