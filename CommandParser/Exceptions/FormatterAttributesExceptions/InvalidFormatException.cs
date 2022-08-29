using System;

namespace CommandParser.Exceptions
{
    public class InvalidFormatException : CommandParserBaseException
    {
        public InvalidFormatException() { }
        public InvalidFormatException(string message) : base(message) { }
        public InvalidFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
