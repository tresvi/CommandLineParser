using System;

namespace Tresvi.CommandParser.Exceptions
{
    public class NotDefaultVerbException : CommandParserBaseException
    {
        public NotDefaultVerbException() { }
        public NotDefaultVerbException(string message) : base(message) { }
        public NotDefaultVerbException(string message, Exception inner) : base(message, inner) { }
    }
}
