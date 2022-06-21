using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;

namespace Test_CommandParser.Models
{
    public class ParametroConFlag
    {
        [FileExists]
        [Option("inputfile", "i", true, HelpText = "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }

        // [DateTimeFormatter("ee")]
        [DirectoryExists]
        [Option("outputfile", "o", true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }
    }
}
