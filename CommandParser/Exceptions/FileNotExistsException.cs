using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class FileNotExistsException: CommandParserException
    {
        public FileNotExistsException() { }
        public FileNotExistsException(string message) : base(message) { }
        public FileNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
