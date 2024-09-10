using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    internal class Param_Multi_Type
    {
        [Option("string", 'x', true)]
        public string? PropString { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("datetime", 'd', true)]
        public DateTime PropDateTime { get; set; }

        [Option("byte", 'b', true)]
        public byte PropByte { get; set; }

        [Option("sbyte", 'B', true)]
        public sbyte PropSByte { get; set; }

        [Option("short", 's', true)]
        public short PropShort { get; set; }

        [Option("ushort", 'S', true)]
        public ushort PropUShort { get; set; }

        [Option("int", 'i', true)]
        public int PropInt { get; set; }

        [Option("uint", 'I', true)]
        public uint PropUInt { get; set; }

        [Option("long", 'l', true)]
        public long PropLong { get; set; }

        [Option("ulong", 'L', true)]
        public ulong PropULong { get; set; }

        [Option("float", 'f', true)]
        public float PropFloat { get; set; }

        [Option("double", 'D', true)]
        public double PropDouble { get; set; }

        [Option("decimal", 'e', true)]
        public decimal PropDecimal { get; set; }

        [Option("bool", 'c', true)]
        public bool PropBool { get; set; }

    }
}
