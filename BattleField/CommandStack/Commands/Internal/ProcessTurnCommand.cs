using System;

namespace CommandStack.Commands.Internal
{
    internal class ProcessTurnCommand
    {
        public Guid MatchId { get; }
        public ProcessTurnCommand(Guid matchId)
        {
            MatchId = matchId;
        }
    }
}
