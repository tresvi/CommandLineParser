﻿using System;

namespace CommandParser.Exceptions
{
    public class MultiInvocationParameterException: CommandParserException
    {
        public MultiInvocationParameterException() { }
        public MultiInvocationParameterException(string message) : base(message) { }
        public MultiInvocationParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
