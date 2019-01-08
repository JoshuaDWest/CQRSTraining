using System;

namespace CommandStack.Commands
{
    public class StartNewGameCommand
    {
        public Guid Id { get; }

        public StartNewGameCommand(Guid matchId)
        {
            Id = matchId;
        }

        public StartNewGameCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}
