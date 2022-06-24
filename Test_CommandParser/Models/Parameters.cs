using CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Parameters
    {
        [Option("inputfile", 'i', true, HelpText = "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }


        [Option("outputfile", 'o', true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }

    }
}
