using System;

namespace CommandParser.Exceptions
{
    public class InvalidFormatException : CommandParserException
    {
        public InvalidFormatException() { }
        public InvalidFormatException(string message) : base(message) { }
        public InvalidFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
