using System;

namespace CommandParser.Exceptions
{
    internal class UnknownArgumentException : CommandParserException
    {
        public UnknownArgumentException() { }
        public UnknownArgumentException(string message) : base(message) { }
        public UnknownArgumentException(string message, Exception inner) : base(message, inner) { }
    }
}
