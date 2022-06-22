using System;

namespace CommandParser.Exceptions
{
    public class UnknownVerbException : CommandParserException
    {
        public UnknownVerbException() { }
        public UnknownVerbException(string message) : base(message) { }
        public UnknownVerbException(string message, Exception inner) : base(message, inner) { }

    }
}
