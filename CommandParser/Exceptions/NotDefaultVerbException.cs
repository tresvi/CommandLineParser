using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class NotDefaultVerbException: CommandParserException
    {
        public NotDefaultVerbException() { }
        public NotDefaultVerbException(string message) : base(message) { }
        public NotDefaultVerbException(string message, Exception inner) : base(message, inner) { }
    }
}
