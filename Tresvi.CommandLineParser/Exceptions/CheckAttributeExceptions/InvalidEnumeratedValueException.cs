using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class InvalidEnumeratedValueException : CommandParserBaseException
    {
        public InvalidEnumeratedValueException() { }
        public InvalidEnumeratedValueException(string message) : base(message) { }
        public InvalidEnumeratedValueException(string message, Exception inner) : base(message, inner) { }
    }
}

