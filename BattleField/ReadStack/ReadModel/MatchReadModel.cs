using System;

namespace ReadStack.ReadModel
{
    public class MatchReadModel
    {
        public void Update(MatchReadModel other)
        {
            Active = other.Active;
            Id = other.Id;
            Player1?.Update(other.Player1);
            Player2?.Update(other.Player2);
        }

        public Guid Id { get; set; }
        public bool Active { get; set; }

        public PlayerInfoReadModel Player1 { get; set; }
        public PlayerInfoReadModel Player2 { get; set; }
    }
}
