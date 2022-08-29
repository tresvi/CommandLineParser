using System;

namespace CommandParser.Exceptions
{
    public abstract class CommandParserBaseException : SystemException
    {
        public CommandParserBaseException() { }
        public CommandParserBaseException(string message) : base(message) { }
        public CommandParserBaseException(string message, Exception inner) : base(message, inner) { }
    }
}
