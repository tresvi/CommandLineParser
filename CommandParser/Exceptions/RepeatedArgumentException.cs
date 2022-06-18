using System;

namespace CommandParser.Exceptions
{
    internal class RepeatedArgumentException: Exception
    {
        public RepeatedArgumentException() { }
        public RepeatedArgumentException(string message) : base(message) { }
        public RepeatedArgumentException(string message, Exception inner) : base(message, inner) { }
    }
}
