using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class InvalidEmailAddressException : CommandParserBaseException
    {
        public InvalidEmailAddressException() { }
        public InvalidEmailAddressException(string message) : base(message) { }
        public InvalidEmailAddressException(string message, Exception inner) : base(message, inner) { }
    }
}

