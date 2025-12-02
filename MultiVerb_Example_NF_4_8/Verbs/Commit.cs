using Tresvi.CommandParser.Attributtes.Keywords;

namespace MultiVerb_Example_NF_4_8.Verbs
{
    [Verb("commit", "Comando para confirmar cambios")]
    public class Commit
    {
        [Option("file", 'f', true, "Ruta del archivo a confirmar")]
        public string File { get; set; }

        [Option("message", 'm', true, "Breve descripcion de las modificaciones")]
        public string Message { get; set; }
    }
}

