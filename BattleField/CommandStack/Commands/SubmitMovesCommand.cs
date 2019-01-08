using System;
using System.Collections.Generic;
using Models.Enums;

namespace CommandStack.Commands
{
    public class SubmitMovesCommand
    {
        public string Player { get; set; }
        public Guid Match { get; set; }
        public IList<Attack> Attacks { get; set; }
        public IList<Deploy> Deployments { get; set; }
    }

    public class Deploy
    {
        public UnitType Type { get; set; }
    }

    public class Attack
    {
        public UnitType Source { get; set; }
        public UnitType Target { get; set; }
    }
}
