using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class DirectoryNotExistsException : CommandParserBaseException
    {
        public DirectoryNotExistsException() { }
        public DirectoryNotExistsException(string message) : base(message) { }
        public DirectoryNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
