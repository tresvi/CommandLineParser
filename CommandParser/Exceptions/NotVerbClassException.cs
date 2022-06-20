using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class NotVerbClassException: CommandParserException
    {
        public NotVerbClassException() { }
        public NotVerbClassException(string message) : base(message) { }
        public NotVerbClassException(string message, Exception inner) : base(message, inner) { }
    }
}
