using System;

namespace CommandParser.Exceptions
{
    public class DirectoryAlreadyExistsException : CommandParserException
    {
        public DirectoryAlreadyExistsException() { }
        public DirectoryAlreadyExistsException(string message) : base(message) { }
        public DirectoryAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
