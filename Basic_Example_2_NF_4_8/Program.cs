using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Reflection;
using System.Text;

namespace Basic_Example_2_NF_4_8
{
    /* Ejemplo de invocación:
     * \>Basic_Example_2_NF_4_8.exe --fechaproceso 20211229 --inputfile "C:\Temp\Archivo.txt" --outputfile "C:\Logs\ddd"  --reintentos 2
     */
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parameters parameters = CommandLine.Parse<Parameters>(args);
                Console.WriteLine(ListProperties(parameters));
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
                sb.AppendLine($"{property.Name} : {property.GetValue(instancia)}");
            }
            return sb.ToString();
        }
    }
}
