﻿using System;

namespace CommandParser.Exceptions
{
    public class ValueNotFoundException : CommandParserBaseException
    {
        public ValueNotFoundException() { }
        public ValueNotFoundException(string message) : base(message) { }
        public ValueNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
