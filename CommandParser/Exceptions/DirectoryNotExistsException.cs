using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class DirectoryNotExistsException: CommandParserException
    {
        public DirectoryNotExistsException() { }
        public DirectoryNotExistsException(string message) : base(message) { }
        public DirectoryNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
