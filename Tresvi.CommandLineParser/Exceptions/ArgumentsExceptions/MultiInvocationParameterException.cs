using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class MultiInvocationParameterException: CommandParserBaseException
    {
        public MultiInvocationParameterException() { }
        public MultiInvocationParameterException(string message) : base(message) { }
        public MultiInvocationParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
