using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;

namespace Test_CommandParser.Models
{
    internal class Param_OutFileNotExists
    {
        [DirectoryNotExists]
        [Option("outputfile", "o", true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }


    }
}
