using CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Parameter_With_Flag
    {
        [Option("inputfile", "i", true, HelpText = "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }

        [Option("outputfile", "o", true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }

        [Flag("sendmail", "s", HelpText = "Indica si debe notificar por mail")]
        public bool SendMail { get; set; }
    }
}
