using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    public class Param_With_Format
    {
        [DateTimeFormatter("yyyyMMdd")]
        [Option("fecha-inicial", 'f', true)]
        public DateTime FechaInicio { get; set; }

        [DateTimeFormatter("yyyyMM")]
        [Option("fecha-final", 'F', false)]
        public DateTime FechaFinal { get; set; }
    }
}
