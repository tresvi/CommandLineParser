using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("status", "Muestra el estado del repositorio", isDefault: true)]
    internal class Status
    {
        [Flag("verbose", 'v', "Mostrar detalles adicionales")]
        public bool Verbose { get; set; }
    }
}

