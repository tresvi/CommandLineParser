using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class InvalidStringListValueException : CommandParserBaseException
    {
        public InvalidStringListValueException() { }
        public InvalidStringListValueException(string message) : base(message) { }
        public InvalidStringListValueException(string message, Exception inner) : base(message, inner) { }
    }
}

