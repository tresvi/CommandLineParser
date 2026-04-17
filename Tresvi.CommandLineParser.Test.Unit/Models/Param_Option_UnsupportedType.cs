using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// Opción con tipo no soportado por OptionAttribute (p. ej. Guid).
    /// </summary>
    internal class Param_Option_UnsupportedType
    {
        [Option("id", 'i', true, "Identificador")]
        public System.Guid Id { get; set; }
    }
}
