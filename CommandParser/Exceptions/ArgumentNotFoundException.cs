using System;

namespace CommandParser.Exceptions
{
    public class ArgumentNotFoundException: Exception
    {
        public ArgumentNotFoundException() { }
        public ArgumentNotFoundException(string message) : base(message) { }
        public ArgumentNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
