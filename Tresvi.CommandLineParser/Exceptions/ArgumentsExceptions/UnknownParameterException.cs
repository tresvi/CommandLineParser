using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class UnknownParameterException : CommandParserBaseException
    {
        public UnknownParameterException() { }
        public UnknownParameterException(string message) : base(message) { }
        public UnknownParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
