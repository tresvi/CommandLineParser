using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace EnumMap_Example_NF_4_8
{
    // Enumeraciones de ejemplo
    public enum FormatoSalida
    {
        Json,
        Xml,
        Csv,
        Yaml
    }

    public enum NivelLog
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public class Parameters
    {
        // Ejemplo 1: Enum con mapeo personalizado completo
        // Permite usar aliases cortos y nombres personalizados
        [Option("formato", 'f', false, "Formato de salida del archivo")]
        [EnumMap("json", FormatoSalida.Json)]
        [EnumMap("xml", FormatoSalida.Xml)]
        [EnumMap("csv", FormatoSalida.Csv)]
        [EnumMap("yaml", FormatoSalida.Yaml)]
        [EnumMap("j", FormatoSalida.Json)]      // Alias corto para Json
        [EnumMap("x", FormatoSalida.Xml)]       // Alias corto para Xml
        [EnumMap("c", FormatoSalida.Csv)]       // Alias corto para Csv
        [EnumMap("y", FormatoSalida.Yaml)]      // Alias corto para Yaml
        public FormatoSalida Formato { get; set; }

        // Ejemplo 2: Enum nullable con mapeo personalizado
        // Útil para parámetros opcionales
        [Option("nivel", 'l', false, "Nivel de logging")]
        [EnumMap("debug", EnumMap_Example_NF_4_8.NivelLog.Debug)]
        [EnumMap("info", EnumMap_Example_NF_4_8.NivelLog.Info)]
        [EnumMap("warning", EnumMap_Example_NF_4_8.NivelLog.Warning)]
        [EnumMap("error", EnumMap_Example_NF_4_8.NivelLog.Error)]
        [EnumMap("d", EnumMap_Example_NF_4_8.NivelLog.Debug)]          // Alias corto
        [EnumMap("i", EnumMap_Example_NF_4_8.NivelLog.Info)]           // Alias corto
        [EnumMap("w", EnumMap_Example_NF_4_8.NivelLog.Warning)]        // Alias corto
        [EnumMap("e", EnumMap_Example_NF_4_8.NivelLog.Error)]          // Alias corto
        public NivelLog? NivelLog { get; set; }
    }
}

