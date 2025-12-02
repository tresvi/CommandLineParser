using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{

    [Verb("push", "Envia la rama especificada al repositorio remoto")]
    internal class PushVerb
    {
        [Option("remote", 'r', true, "---")]
        public string? Remote { get; set; }


        [Option("branch", 'b', false, "Branch a donde se quieren enviar los cambios")]
        public string? branch { get; set; }
    }
}
