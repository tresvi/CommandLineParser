using CommandParser.Attributes.Formatter;
using CommandParser.Attributes.Validation;
using CommandParser.Attributtes.Keywords;
using System;
using System.Reflection;
using System.Text;

namespace Ejemplo_NF_4_8
{
    [Verb("Commit", "Es un commit")]
    public class Parameters
    {
        //The Art of UNIX Programming
        //https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html
        //https://tldp.org/LDP/abs/html/standard-options.html
        //https://www.gnu.org/prep/standards/html_node/Command_002dLine-Interfaces.html
        //https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run

        [FileExists]
        [Option("inputfile", "i", true, HelpText = "Archivo de entrada a ser procesado.")]
        public string InputFile { get; set; }


        [DirectoryExists]
        [Option("outputfile", "o", true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string OutputFile { get; set; }


        [DateTimeFormatter("yyyyMMdd")]
        [Option("fechaproceso", "f", true, HelpText = "Fecha en la cual se realiza el procesamiento.")]
        public DateTime FechaProceso { get; set; }

        [Obsolete]
        [Flag("notificarpormail", "n", HelpText = "Indica si se debe notificar por mail el resultado del proceso.")]
        public bool NotificarPorMail { get; set; }

        [Option("reintentos", "r", true, HelpText = "Fecha en la cual se realiza el procesamiento.")]
        public int Reintentos { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                sb.AppendLine($"{property.Name} : {property.GetValue(this)}");
            }
            return sb.ToString();
        }
    }

}
