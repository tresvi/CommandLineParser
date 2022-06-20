using System;

namespace CommandParser.Exceptions
{
    public class FileAlreadyExistsException: CommandParserException
    {
        public FileAlreadyExistsException() { }
        public FileAlreadyExistsException(string message) : base(message) { }
        public FileAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
