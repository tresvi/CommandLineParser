using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("log", "Muestra el historial de commits", isDefault: false)]
    internal class Log
    {
        [Option("count", 'c', false, "Número de commits a mostrar")]
        public int? Count { get; set; }
    }
}

