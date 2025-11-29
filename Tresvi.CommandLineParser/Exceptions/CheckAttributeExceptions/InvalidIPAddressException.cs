using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class InvalidIPAddressException : CommandParserBaseException
    {
        public InvalidIPAddressException() { }
        public InvalidIPAddressException(string message) : base(message) { }
        public InvalidIPAddressException(string message, Exception inner) : base(message, inner) { }
    }
}

