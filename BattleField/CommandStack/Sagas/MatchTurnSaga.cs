using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommandStack.Commands.Internal;
using Models.Constants;
using Models.Events;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.EventStack;

namespace CommandStack.Sagas
{
    // responsible for listening to match moves that are submitted. Once both players have submitted all moves then
    // a turn can be evaluated.
    public class MatchTurnSaga : IEventAsyncHandler<AllPlayerMovesSubmittedForTurnEvent>
    {
        private readonly CommandDelegator _commandDelegator;
        private List<string> _playersReadForBattle;

        public MatchTurnSaga(CommandDelegator commandDelegator)
        {
            _commandDelegator = commandDelegator;
            _playersReadForBattle = new List<string>();
        }

        public Task HandleAsync(AllPlayerMovesSubmittedForTurnEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            _playersReadForBattle.Add(@event.Player);
            if (_playersReadForBattle.Count == PlayerInfo.PLAYER_COUNT)
            {
                _commandDelegator.SendAsync(new ProcessTurnCommand(@event.AggregateRootId));
            }
            return Task.CompletedTask;
        }
    }
}
