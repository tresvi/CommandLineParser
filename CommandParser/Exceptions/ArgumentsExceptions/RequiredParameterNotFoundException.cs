using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class RequiredParameterNotFoundException : CommandParserBaseException
    {
        public RequiredParameterNotFoundException() { }
        public RequiredParameterNotFoundException(string message) : base(message) { }
        public RequiredParameterNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
