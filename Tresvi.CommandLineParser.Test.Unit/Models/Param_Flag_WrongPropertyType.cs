using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// Flag aplicado a un tipo no bool para probar WrongPropertyTypeException.
    /// </summary>
    internal class Param_Flag_WrongPropertyType
    {
        [Flag("verbose", 'v', "No debería ser int")]
        public int Verbose { get; set; }
    }
}
