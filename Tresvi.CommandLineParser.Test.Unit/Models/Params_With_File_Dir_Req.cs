using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Params_With_File_Dir_Exists
    {
        [FileExists]
        [Option("inputfile", 'i', true, helpText : "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }

        [DirectoryExists]
        [Option("outputdir", 'o', true, helpText : "Directorio de Salida de procesamiento.")]
        public string? OutputFile { get; set; }
    }
}
