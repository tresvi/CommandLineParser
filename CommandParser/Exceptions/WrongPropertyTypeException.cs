using System;

namespace CommandParser.Exceptions
{
    internal class WrongPropertyTypeException: CommandParserException
    {
        public WrongPropertyTypeException() { }
        public WrongPropertyTypeException(string message) : base(message) { }
        public WrongPropertyTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
