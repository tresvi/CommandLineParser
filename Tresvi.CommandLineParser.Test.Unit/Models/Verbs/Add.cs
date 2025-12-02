using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("add", "Agrega una instancia")]
    internal class Add
    {
        [Option("directory", 'd', true, "Carpeta donde se creará el archivo")]
        public string? Directory { get; set; }


        [Option("name", 'n', true, "Nombre del archivo a crear con su extensión")]
        public string? Nombre { get; set; }
    }
}
