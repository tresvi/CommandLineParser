using System;
using System.Runtime.Serialization;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class MultiDefinitionParameterException : CommandParserBaseException, ISerializable
    {
        public MultiDefinitionParameterException() { }
        public MultiDefinitionParameterException(string message) : base(message) { }
        public MultiDefinitionParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
