using CommandParser;
using CommandParser.Exceptions;
using System;

namespace Ejemplo_NF_4_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //object parametro = CommandLine.Parse<Parameters, int>(args);

                Parameters parametros = CommandLine.Parse<Parameters>(args);
                Console.WriteLine(parametros?.ToString());
                Console.WriteLine("Fin OK!!");
            }

            catch (RequiredParameterNotFoundException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine("Fin Todo Mal!!");
            }
            catch (CommandParserException ex)
            {
                //TODO
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine("Fin Todo Mal!!");
            }

            Console.ReadKey();
        }
    }
}
