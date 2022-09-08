using System;

namespace CommandParser.Exceptions
{
    [Serializable]
    public class FileNotExistsException : CommandParserBaseException
    {
        public FileNotExistsException() { }
        public FileNotExistsException(string message) : base(message) { }
        public FileNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
