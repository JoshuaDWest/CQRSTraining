using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Exceptions
{
    public class CannotCalculateAttackException : Exception
    {
        public CannotCalculateAttackException(string message) : base(message)
        {
        }
        public CannotCalculateAttackException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
