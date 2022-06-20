using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Exceptions
{
    public class ValueNotSpecifiedException: CommandParserException
    {
        public ValueNotSpecifiedException() { }
        public ValueNotSpecifiedException(string message) : base(message) { }
        public ValueNotSpecifiedException(string message, Exception inner) : base(message, inner) { }
    }
}
