using System;

namespace CommandParser.Exceptions
{
    internal class TooManyDefaultVerbsException : CommandParserBaseException
    {
        public TooManyDefaultVerbsException() { }
        public TooManyDefaultVerbsException(string message) : base(message) { }
        public TooManyDefaultVerbsException(string message, Exception inner) : base(message, inner) { }
    }
}
