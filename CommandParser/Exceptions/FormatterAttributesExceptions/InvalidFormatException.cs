using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class InvalidFormatException : CommandParserBaseException
    {
        public InvalidFormatException() { }
        public InvalidFormatException(string message) : base(message) { }
        public InvalidFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
