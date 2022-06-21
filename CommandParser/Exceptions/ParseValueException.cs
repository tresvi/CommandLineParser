using System;

namespace CommandParser.Exceptions
{
    internal class ParseValueException : CommandParserException
    {
        public ParseValueException() { }
        public ParseValueException(string message) : base(message) { }
        public ParseValueException(string message, Exception inner) : base(message, inner) { }
    }
}
