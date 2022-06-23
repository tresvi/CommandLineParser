using System;

namespace CommandParser.Exceptions
{
    internal class RepeatedParameterDefinitionException : CommandParserException
    {
        public RepeatedParameterDefinitionException() { }
        public RepeatedParameterDefinitionException(string message) : base(message) { }
        public RepeatedParameterDefinitionException(string message, Exception inner) : base(message, inner) { }
    }
}
