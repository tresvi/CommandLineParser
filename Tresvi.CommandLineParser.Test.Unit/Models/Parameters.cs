using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Parameters
    {
        [Option("inputfile", 'i', true, helpText : "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }


        [Option("outputfile", 'o', true, helpText : "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }

    }
}
