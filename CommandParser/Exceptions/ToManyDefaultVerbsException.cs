using System;

namespace CommandParser.Exceptions
{
    internal class ToManyDefaultVerbsException : CommandParserException
    {
        public ToManyDefaultVerbsException() { }
        public ToManyDefaultVerbsException(string message) : base(message) { }
        public ToManyDefaultVerbsException(string message, Exception inner) : base(message, inner) { }
    }
}
