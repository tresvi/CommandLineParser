using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;
using CommandParser.DecoratorAttributes.DecoratorFormatterAttributes;
using System;

namespace Test_CommandParser.Models
{
    internal class Parameters
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
