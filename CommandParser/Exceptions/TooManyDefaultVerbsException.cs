using System;

namespace CommandParser.Exceptions
{
    internal class TooManyDefaultVerbsException : CommandParserException
    {
        public TooManyDefaultVerbsException() { }
        public TooManyDefaultVerbsException(string message) : base(message) { }
        public TooManyDefaultVerbsException(string message, Exception inner) : base(message, inner) { }
    }
}
