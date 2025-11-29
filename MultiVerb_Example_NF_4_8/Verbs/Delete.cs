using Tresvi.CommandParser.Attributtes.Keywords;

namespace MultiVerb_Example_NF_4_8.Verbs
{
    [Verb("delete", "Comando para eliminar archivo")]
    public class Delete
    {
        [Option("file", 'f', true, "", "Ruta del archivo a eliminar")]
        public string File { get; set; }
    }
}

