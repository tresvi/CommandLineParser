using CommandParser;
using CommandParser.Exceptions;
using System;
using System.Reflection;
using System.Text;

namespace Ejemplo_NF_4_8
{
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
