using System;

namespace CommandParser.Exceptions
{
    public class NotVerbClassException : CommandParserException
    {
        public NotVerbClassException() { }
        public NotVerbClassException(string message) : base(message) { }
        public NotVerbClassException(string message, Exception inner) : base(message, inner) { }
    }
}
