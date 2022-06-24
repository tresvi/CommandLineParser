using CommandParser.Attributes.Formatter;
using CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    public class Param_Repited_ShortKeyword
    {
        [Option("string", 's', true)]
        public string? PropString { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("datetime", 'd', true)]
        public DateTime PropDateTime { get; set; }

        [Option("byte", 'b', true)]
        public byte PropByte { get; set; }

        [Option("sbyte", 's', true)]
        public sbyte PropSByte { get; set; }
    }
}
