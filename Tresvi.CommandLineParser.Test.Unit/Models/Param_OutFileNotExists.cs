using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Param_OutFileNotExists
    {
        [DirectoryNotExists]
        [Option("outputfile", 'o', true, helpText : "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }
    }
}
