using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Attributes.Validation;

namespace Test_CommandParser.Models
{
    internal class Parameters_Incompatible
    {
        [Option("out", 'o', false, "Archivo de salida")]
        [IncompatibleWith(nameof(Overwrite), nameof(Append))]
        public string? OutputPath { get; set; }

        [Flag("overwrite", 'w', "Sobrescribir archivo existente")]
        [IncompatibleWith(nameof(Append))]
        public bool Overwrite { get; set; }

        [Flag("append", 'a', "Agregar al final del archivo")]
        public bool Append { get; set; }

        [Option("input", 'i', false, "Archivo de entrada")]
        [IncompatibleWith(nameof(OutputPath))]
        public string? Input { get; set; }
    }
}

