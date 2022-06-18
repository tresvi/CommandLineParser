using CommandParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejemplo_NF_4_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parameters parametros = CommandLine.Parse<Parameters>(args);
                Console.WriteLine(parametros?.ToString());
                Console.WriteLine("Fin OK!!");
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
