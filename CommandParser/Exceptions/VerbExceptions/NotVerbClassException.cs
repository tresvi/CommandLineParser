using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class NotVerbClassException : CommandParserBaseException
    {
        public NotVerbClassException() { }
        public NotVerbClassException(string message) : base(message) { }
        public NotVerbClassException(string message, Exception inner) : base(message, inner) { }
    }
}
