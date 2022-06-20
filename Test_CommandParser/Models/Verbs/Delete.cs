using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;
using CommandParser.DecoratorAttributes.DecoratorFormatterAttributes;

namespace Test_CommandParser.Models.Verbs
{

    [Verb("delete", "Comando para eliminar archivo")]
    internal class Delete
    {
        [Option("file", "f", true, "", "Ruta del archivo a eliminar")]
        public string? File { get; set; }

    }
}
