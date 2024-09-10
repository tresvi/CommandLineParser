using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public abstract class CommandParserBaseException : SystemException
    {
        protected CommandParserBaseException() { }
        protected CommandParserBaseException(string message) : base(message) { }
        protected CommandParserBaseException(string message, Exception inner) : base(message, inner) { }
    }
}
