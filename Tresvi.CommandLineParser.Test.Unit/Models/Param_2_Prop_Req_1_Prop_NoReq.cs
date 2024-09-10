using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Param_2_Prop_Req_1_Prop_NoReq
    {
        [Option("inputfile", 'i', true, HelpText = "Archivo de entrada a ser procesado.")]
        public string? InputFile { get; set; }


        [Option("outputfile", 'o', true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }

        [Option("name", 'n', false, HelpText = "Nombre del proceso.")]
        public string? Name { get; set; }
    }
}
