using Tresvi.CommandParser.Attributtes.Keywords;

namespace MultiVerb_Example_NF_4_8.Verbs
{
    [Verb("add", "Agrega una instancia")]
    public class Add
    {
        [Option("directory", 'd', true, "Carpeta donde se creará el archivo")]
        public string Directory { get; set; }

        [Option("name", 'n', true, "Nombre del archivo a crear con su extensión")]
        public string Nombre { get; set; }
    }
}

