using System;

namespace CommandStack.Commands.Internal
{
    internal class EndGameCommand
    {
        public Guid MatchId { get; }
        public string Winner { get; }
        public EndGameCommand(Guid id, string winner)
        {
            MatchId = id;
            Winner = winner;
        }
    }
}
