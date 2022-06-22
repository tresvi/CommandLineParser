using System;

namespace CommandParser.Exceptions
{
    public class RequiredParameterNotFoundException : CommandParserException
    {
        public RequiredParameterNotFoundException() { }
        public RequiredParameterNotFoundException(string message) : base(message) { }
        public RequiredParameterNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
