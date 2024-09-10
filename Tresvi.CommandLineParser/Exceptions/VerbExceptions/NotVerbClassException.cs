using System;

namespace Tresvi.CommandParser.Exceptions
{
    public class NotVerbClassException : CommandParserBaseException
    {
        public NotVerbClassException() { }
        public NotVerbClassException(string message) : base(message) { }
        public NotVerbClassException(string message, Exception inner) : base(message, inner) { }
    }
}
