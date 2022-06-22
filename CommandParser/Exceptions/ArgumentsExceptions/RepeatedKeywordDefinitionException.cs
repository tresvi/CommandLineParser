using System;

namespace CommandParser.Exceptions
{
    internal class RepeatedKeywordDefinitionException : CommandParserException
    {
        public RepeatedKeywordDefinitionException() { }
        public RepeatedKeywordDefinitionException(string message) : base(message) { }
        public RepeatedKeywordDefinitionException(string message, Exception inner) : base(message, inner) { }
    }
}
