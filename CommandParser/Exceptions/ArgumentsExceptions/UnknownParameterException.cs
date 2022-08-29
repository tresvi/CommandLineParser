using System;

namespace CommandParser.Exceptions
{
    public class UnknownParameterException : CommandParserBaseException
    {
        public UnknownParameterException() { }
        public UnknownParameterException(string message) : base(message) { }
        public UnknownParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
