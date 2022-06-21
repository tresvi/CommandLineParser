using System;

namespace CommandParser.Exceptions
{
    public class DirectoryNotExistsException : CommandParserException
    {
        public DirectoryNotExistsException() { }
        public DirectoryNotExistsException(string message) : base(message) { }
        public DirectoryNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
