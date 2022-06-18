using System;

namespace CommandParser.Exceptions
{
    internal class WrongPropertyTypeException: Exception
    {
        public WrongPropertyTypeException() { }
        public WrongPropertyTypeException(string message) : base(message) { }
        public WrongPropertyTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
