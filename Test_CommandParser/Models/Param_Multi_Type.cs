using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes.DecoratorFormatterAttributes;
using System;

namespace Test_CommandParser.Models
{
    internal class Param_Multi_Type
    {
        [Option("string", "s", true)]
        public string? PropString { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("datetime", "d", true)]
        public DateTime PropDateTime { get; set; }

        [Option("byte", "b", true)]
        public byte PropByte { get; set; }

        [Option("sbyte", "sb", true)]
        public sbyte PropSByte { get; set; }

        [Option("short", "sh", true)]
        public short PropShort { get; set; }

        [Option("ushort", "us", true)]
        public ushort PropUShort { get; set; }

        [Option("int", "i", true)]
        public int PropInt { get; set; }

        [Option("uint", "ui", true)]
        public uint PropUInt { get; set; }

        [Option("long", "l", true)]
        public long PropLong { get; set; }

        [Option("ulong", "ul", true)]
        public ulong PropULong { get; set; }

        [Option("float", "f", true)]
        public float PropFloat { get; set; }

        [Option("double", "do", true)]
        public double PropDouble { get; set; }

        [Option("decimal", "de", true)]
        public decimal PropDecimal { get; set; }

        [Option("bool", "bo", true)]
        public bool PropBool { get; set; }

    }
}
