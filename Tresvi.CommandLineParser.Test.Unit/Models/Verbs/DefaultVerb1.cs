using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("default1", "Primer verbo por defecto", isDefault: true)]
    internal class DefaultVerb1
    {
        [Option("param", 'p', false, "Parámetro de prueba")]
        public string? Param { get; set; }
    }
}

