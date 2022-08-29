using System;

namespace CommandParser.Exceptions
{
    public class FileAlreadyExistsException : CommandParserBaseException
    {
        public FileAlreadyExistsException() { }
        public FileAlreadyExistsException(string message) : base(message) { }
        public FileAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
