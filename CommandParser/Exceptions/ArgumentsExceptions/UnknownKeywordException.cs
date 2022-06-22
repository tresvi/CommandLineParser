using System;

namespace CommandParser.Exceptions
{
    internal class UnknownKeywordException : CommandParserException
    {
        public UnknownKeywordException() { }
        public UnknownKeywordException(string message) : base(message) { }
        public UnknownKeywordException(string message, Exception inner) : base(message, inner) { }
    }
}
