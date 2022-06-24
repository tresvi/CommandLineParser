using System;

namespace CommandParser.Exceptions
{
    public class MultiDefinitionParameterException : CommandParserException
    {
        public MultiDefinitionParameterException() { }
        public MultiDefinitionParameterException(string message) : base(message) { }
        public MultiDefinitionParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
