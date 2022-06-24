using CommandParser.Attributes.Formatter;
using CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    internal class Param_Repited_Keyword
    {
        [Option("string", 's', true)]
        public string? PropString { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("datetime", 'd', true)]
        public DateTime PropDateTime { get; set; }

        [Option("byte", 'b', true)]
        public byte PropByte { get; set; }

        [Option("byte", 'B', true)]
        public sbyte PropSByte { get; set; }
    }
}
