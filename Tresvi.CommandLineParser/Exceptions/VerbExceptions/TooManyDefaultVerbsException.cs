using System;

namespace Tresvi.CommandParser.Exceptions
{
    public class TooManyDefaultVerbsException : CommandParserBaseException
    {
        public TooManyDefaultVerbsException() { }
        public TooManyDefaultVerbsException(string message) : base(message) { }
        public TooManyDefaultVerbsException(string message, Exception inner) : base(message, inner) { }
    }
}
