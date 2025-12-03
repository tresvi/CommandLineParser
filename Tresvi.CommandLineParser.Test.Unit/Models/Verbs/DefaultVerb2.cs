using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("default2", "Segundo verbo por defecto", isDefault: true)]
    internal class DefaultVerb2
    {
        [Option("param", 'p', false, "Parámetro de prueba")]
        public string? Param { get; set; }
    }
}

