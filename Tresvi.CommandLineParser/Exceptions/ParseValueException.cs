using System;

namespace Tresvi.CommandParser.Exceptions
{
    public class ParseValueException : CommandParserBaseException
    {
        public ParseValueException() { }
        public ParseValueException(string message) : base(message) { }
        public ParseValueException(string message, Exception inner) : base(message, inner) { }
    }
}
