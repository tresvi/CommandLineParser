using System;

namespace Tresvi.CommandParser.Exceptions
{
    public class WrongPropertyTypeException : CommandParserBaseException
    {
        public WrongPropertyTypeException() { }
        public WrongPropertyTypeException(string message) : base(message) { }
        public WrongPropertyTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
