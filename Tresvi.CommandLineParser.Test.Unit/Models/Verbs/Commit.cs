using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("commit", "Comando para confirmar cambios")]
    internal class Commit
    {
        [Option("file", 'f', true, "", "Ruta del archivo a confirmar")]
        public string? File { get; set; }


        [Option("message", 'm', true, "", "Breve descripcion de las modificaciones")]
        public string? Message { get; set; }
    }
}
