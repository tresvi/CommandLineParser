using System;

namespace Tresvi.CommandParser.Exceptions
{
    [Serializable]
    public class DirectoryAlreadyExistsException : CommandParserBaseException
    {
        public DirectoryAlreadyExistsException() { }
        public DirectoryAlreadyExistsException(string message) : base(message) { }
        public DirectoryAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
