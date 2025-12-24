using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Reflection;
using System.Text;

namespace EnumMap_Example_NF_4_8
{
    /* 
     * Ejemplo de uso de EnumMapAttribute
     * 
     * Este ejemplo muestra cómo usar EnumMapAttribute para mapear valores personalizados
     * a enumeraciones, permitiendo aliases cortos y nombres personalizados.
     * 
     * Ejemplos de invocación:
     * 
     * Formato con nombre completo:
     *   >EnumMap_Example_NF_4_8.exe --formato json
     *   >EnumMap_Example_NF_4_8.exe --formato xml
     * 
     * Formato con alias corto:
     *   >EnumMap_Example_NF_4_8.exe --formato j
     *   >EnumMap_Example_NF_4_8.exe --formato x
     * 
     * Formato case-insensitive:
     *   >EnumMap_Example_NF_4_8.exe --formato JSON
     *   >EnumMap_Example_NF_4_8.exe --formato XML
     * 
     * Combinando parámetros:
     *   >EnumMap_Example_NF_4_8.exe --formato json --nivel debug
     *   >EnumMap_Example_NF_4_8.exe -f j -l i
     * 
     * Parámetro opcional (nullable):
     *   >EnumMap_Example_NF_4_8.exe --formato csv
     *   (nivel no se especifica, será null)
     */
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parameters parameters = CommandLine.Parse<Parameters>(args);
                
                Console.WriteLine("=== Parámetros Parseados ===");
                Console.WriteLine(ListProperties(parameters));
                Console.WriteLine();
                
                // Mostrar información adicional sobre los enums
                Console.WriteLine("=== Información Detallada ===");
                Console.WriteLine($"Formato seleccionado: {parameters.Formato} ({(int)parameters.Formato})");
                
                if (parameters.NivelLog.HasValue)
                {
                    Console.WriteLine($"Nivel de log seleccionado: {parameters.NivelLog.Value} ({(int)parameters.NivelLog.Value})");
                }
                else
                {
                    Console.WriteLine("Nivel de log: No especificado (null)");
                }
                
                Console.WriteLine();
                Console.WriteLine("Fin OK!!");
            }
            catch (CommandParserBaseException ex)
            {
                Console.WriteLine($"ERROR al interpretar la linea de comando: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            Console.ReadKey();
        }

        public static string ListProperties(object instancia)
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo property in instancia.GetType().GetProperties())
            {
                object value = property.GetValue(instancia);
                string valueString = value?.ToString() ?? "null";
                
                // Si es un enum nullable y es null, mostrar "null"
                if (value == null && property.PropertyType.IsGenericType && 
                    property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    valueString = "null";
                }
                
                sb.AppendLine($"{property.Name} : {valueString}");
            }
            return sb.ToString();
        }
    }
}

