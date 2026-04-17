using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// DateTimeFormatter en propiedad no DateTime para probar InvalidadPropertyTypeException.
    /// </summary>
    internal class Param_DateTimeFormatter_WrongPropertyType
    {
        [DateTimeFormatter("yyyyMMdd")]
        [Option("when", 'w', true)]
        public string When { get; set; } = string.Empty;
    }
}
