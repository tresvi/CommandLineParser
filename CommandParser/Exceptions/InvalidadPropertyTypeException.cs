using System;

namespace CommandParser.Exceptions
{
    internal class InvalidadPropertyTypeException: CommandParserException
    {
        public InvalidadPropertyTypeException() { }
        public InvalidadPropertyTypeException(string message) : base(message) { }
        public InvalidadPropertyTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
