using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Params_With_StringListValidation_CaseSensitive
    {
        [StringListValidation(new string[] { "red", "green", "blue" }, caseSensitive: true)]
        [Option("color", 'c', true, HelpText = "Color del tema (red, green, blue).")]
        public string? Color { get; set; }
    }

    internal class Params_With_StringListValidation_CaseInsensitive
    {
        [StringListValidation(new string[] { "dev", "staging", "prod" }, caseSensitive: false)]
        [Option("environment", 'e', true, HelpText = "Ambiente de ejecución (dev, staging, prod).")]
        public string? Environment { get; set; }
    }

    internal class Params_With_StringListValidation_SingleValue
    {
        [StringListValidation(new string[] { "json" }, caseSensitive: true)]
        [Option("format", 'f', true, HelpText = "Formato de salida (solo json).")]
        public string? Format { get; set; }
    }

    internal class Params_With_StringListValidation_MultipleOptions
    {
        [StringListValidation(new string[] { "read", "write", "append" }, caseSensitive: true)]
        [Option("mode", 'm', true, HelpText = "Modo de acceso (read, write, append).")]
        public string? Mode { get; set; }

        [StringListValidation(new string[] { "es", "en", "fr" }, caseSensitive: false)]
        [Option("language", 'l', false, HelpText = "Idioma (es, en, fr).")]
        public string? Language { get; set; }
    }

    internal class Params_With_StringListValidation_DefaultCaseSensitive
    {
        [StringListValidation(new string[] { "option1", "option2", "option3" })]
        [Option("option", 'o', true, HelpText = "Opción (option1, option2, option3).")]
        public string? Option { get; set; }
    }
}

