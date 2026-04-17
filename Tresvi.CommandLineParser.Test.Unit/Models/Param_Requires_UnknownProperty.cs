using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// Requires apunta a una propiedad inexistente para ejercitar CheckRequiredParameters (propiedad requerida ausente en la clase).
    /// </summary>
    internal class Param_Requires_UnknownProperty
    {
        [Requires("NoSuchPropertyOnThisClass")]
        [Option("only", 'o', false, "único parámetro")]
        public string? Only { get; set; }
    }
}
