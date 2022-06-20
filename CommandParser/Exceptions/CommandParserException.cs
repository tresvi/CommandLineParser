using System;

namespace CommandParser.Exceptions
{
    public abstract class CommandParserException: SystemException
    {
        public CommandParserException() { }
        public CommandParserException(string message) : base(message) { }
        public CommandParserException(string message, Exception inner) : base(message, inner) { }
    }
}
