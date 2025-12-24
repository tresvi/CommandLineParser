using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    // Enums para pruebas
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public enum Environment
    {
        Development,
        Staging,
        Production
    }

    public enum OutputFormat
    {
        Json,
        Xml,
        Csv,
        Yaml
    }

    public enum Priority
    {
        Low = 1,
        Medium = 5,
        High = 10,
        Critical = 99
    }

    // Modelos de prueba

    // Caso 1: Enum simple sin mapeo (parseo automático)
    internal class Params_With_Enum_Simple
    {
        [Option("level", 'l', false, "Nivel de logging")]
        public LogLevel LogLevel { get; set; }
    }

    // Caso 2: Enum nullable sin mapeo
    internal class Params_With_Enum_Nullable
    {
        [Option("env", 'e', false, "Ambiente de ejecución")]
        public Environment? Environment { get; set; }
    }

    // Caso 3: Enum con mapeo personalizado
    internal class Params_With_Enum_WithMapping
    {
        [Option("format", 'f', false, "Formato de salida")]
        [EnumMap("json", OutputFormat.Json)]
        [EnumMap("xml", OutputFormat.Xml)]
        [EnumMap("csv", OutputFormat.Csv)]
        [EnumMap("j", OutputFormat.Json)]  // Alias corto
        [EnumMap("x", OutputFormat.Xml)]    // Alias corto
        public OutputFormat Format { get; set; }
    }

    // Caso 4: Enum nullable con mapeo personalizado
    internal class Params_With_Enum_Nullable_WithMapping
    {
        [Option("env", 'e', false, "Ambiente")]
        [EnumMap("dev", Test_CommandParser.Models.Environment.Development)]
        [EnumMap("staging", Test_CommandParser.Models.Environment.Staging)]
        [EnumMap("prod", Test_CommandParser.Models.Environment.Production)]
        public Environment? Environment { get; set; }
    }

    // Caso 5: Enum con EnumeratedValidation (restricción adicional)
    internal class Params_With_Enum_WithValidation
    {
        [Option("level", 'l', false, "Nivel de logging")]
        [EnumeratedValidation(new[] { "Debug", "Info", "Warning" })]
        public LogLevel LogLevel { get; set; }
    }

    // Caso 6: Enum con valores numéricos personalizados
    internal class Params_With_Enum_NumericValues
    {
        [Option("priority", 'p', false, "Prioridad")]
        public Priority Priority { get; set; }
    }

    // Caso 7: Enum requerido
    internal class Params_With_Enum_Required
    {
        [Option("level", 'l', true, "Nivel de logging (requerido)")]
        public LogLevel LogLevel { get; set; }
    }

    // Caso 8: Múltiples enums
    internal class Params_With_Enum_Multiple
    {
        [Option("level", 'l', false, "Nivel de logging")]
        public LogLevel LogLevel { get; set; }

        [Option("env", 'e', false, "Ambiente")]
        public Environment Environment { get; set; }

        [Option("format", 'f', false, "Formato")]
        public OutputFormat Format { get; set; }
    }
}

