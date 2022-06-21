using System;

namespace CommandParser.Exceptions
{
    internal class NotDefaultVerbException : CommandParserException
    {
        public NotDefaultVerbException() { }
        public NotDefaultVerbException(string message) : base(message) { }
        public NotDefaultVerbException(string message, Exception inner) : base(message, inner) { }
    }
}
