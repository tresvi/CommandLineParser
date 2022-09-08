using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class MultiDefinitionParameterException : CommandParserBaseException
    {
        public MultiDefinitionParameterException() { }
        public MultiDefinitionParameterException(string message) : base(message) { }
        public MultiDefinitionParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
