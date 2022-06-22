using CommandParser.Attributes.Validation;
using CommandParser.Attributtes.Keywords;


namespace Test_CommandParser.Models
{
    internal class Params_With_File_Dir_NotExists
    {
        [FileNotExists]
        [Option("inputfile", "i", true, HelpText = "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }

        [DirectoryNotExists]
        [Option("outputdir", "o", true, HelpText = "Directorio de Salida de procesamiento.")]
        public string? OutputFile { get; set; }
    }
}
