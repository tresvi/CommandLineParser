using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace Basic_Example_2_NF_4_8
{
    public class Parameters
    {
        [FileExists]
        [Option("inputfile", 'i', true, HelpText = "Archivo de entrada a ser procesado.")]
        public string InputFile { get; set; }

        [DirectoryExists]  
        [Option("outputfile", 'o', true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string OutputFile { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("fechaproceso", 'f', true, HelpText = "Fecha en la cual se realiza el procesamiento.")]
        public DateTime FechaProceso { get; set; }

        [Flag("notificarpormail", 'n', HelpText = "Indica si se debe notificar por mail el resultado del proceso.")]
        public bool NotificarPorMail { get; set; }

        [Option("reintentos", 'r', true, HelpText = "Fecha en la cual se realiza el procesamiento.")]
        public int Reintentos { get; set; }
    }

}
