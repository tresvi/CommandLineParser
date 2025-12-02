using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;
using System.Text.RegularExpressions;

namespace Test_CommandParser.Models
{
    internal class Params_With_RegexValidation_AlphanumericCode
    {
        [RegexValidation(@"^[A-Z]{2}\d{4}$", "Debe ser un código alfanumérico de 2 letras mayúsculas seguidas de 4 dígitos (ej: AB1234)")]
        [Option("code", 'c', true, helpText : "Código alfanumérico (ej: AB1234).")]
        public string? Code { get; set; }
    }

    internal class Params_With_RegexValidation_PhoneNumber
    {
        [RegexValidation(@"^\+?\d{1,3}[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", "Debe ser un número de teléfono válido")]
        [Option("phone", 'p', true, helpText : "Número de teléfono.")]
        public string? Phone { get; set; }
    }

    internal class Params_With_RegexValidation_Date
    {
        [RegexValidation(@"^\d{4}-\d{2}-\d{2}$", "Debe tener el formato YYYY-MM-DD")]
        [Option("date", 'd', true, helpText : "Fecha en formato YYYY-MM-DD.")]
        public string? Date { get; set; }
    }

    internal class Params_With_RegexValidation_CaseInsensitive
    {
        [RegexValidation(@"^[a-z]{3,10}$", "Debe contener solo letras minúsculas (3-10 caracteres)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        [Option("word", 'w', true, helpText : "Palabra en minúsculas.")]
        public string? Word { get; set; }
    }

    internal class Params_With_RegexValidation_DefaultMessage
    {
        [RegexValidation(@"^\d{5}$")]
        [Option("zipcode", 'z', true, helpText : "Código postal (5 dígitos).")]
        public string? ZipCode { get; set; }
    }

    internal class Params_With_RegexValidation_MultipleOptions
    {
        [RegexValidation(@"^[A-Z]{3}$", "Debe ser un código de 3 letras mayúsculas")]
        [Option("country", 'c', true, helpText : "Código de país (3 letras).")]
        public string? Country { get; set; }

        [RegexValidation(@"^\d{2}-\d{2}-\d{4}$", "Debe tener el formato DD-MM-YYYY")]
        [Option("birthdate", 'b', false, helpText : "Fecha de nacimiento (DD-MM-YYYY).")]
        public string? BirthDate { get; set; }
    }
}

