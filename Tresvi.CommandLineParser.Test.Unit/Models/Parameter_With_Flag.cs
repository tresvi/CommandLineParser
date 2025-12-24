using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Parameter_With_Flag
    {
        [Option("inputfile", 'i', true, "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }

        [Option("outputfile", 'o', true, "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }

        [Flag("sendmail", 's', "Indica si debe notificar por mail")]
        public bool SendMail { get; set; }
    }
}
