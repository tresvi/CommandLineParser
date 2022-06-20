using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class UnknownVerbException: CommandParserException
    {
        public UnknownVerbException() { }
        public UnknownVerbException(string message) : base(message) { }
        public UnknownVerbException(string message, Exception inner) : base(message, inner) { }

    }
}
