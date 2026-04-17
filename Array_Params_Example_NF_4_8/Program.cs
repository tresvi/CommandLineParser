using System;
using System.Collections.Generic;
using System.Linq;
using Tresvi.CommandParser;
using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Exceptions;

namespace Array_Params_Example_NF_4_8
{
    /// <summary>
    /// Modelo con opciones repetibles: la misma bandera puede aparecer varias veces.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Ejemplo: --puertos Web=80 --puertos Ssh=22  o  -p Db=5432
        /// </summary>
        [Option("puertos", 'p', true, "Pares nombreServicio=puerto (repetible).", true)]
        public Dictionary<string, int> PuertosPorServicio { get; set; }

        /// <summary>
        /// Ejemplo: --etiqueta dev --etiqueta argentina  o  -e prod
        /// </summary>
        [Option("etiqueta", 'e', false, "Etiquetas de texto (repetible).", true)]
        public IEnumerable<string> Etiquetas { get; set; }
    }

    internal class Program
    {
        // Ejemplo:
        //   Array_Params_Example_NF_4_8.exe --puertos Web=80 -p Ssh=22 --etiqueta dev -e qa
        static void Main(string[] args)
        {
            try
            {
                Parameters p = CommandLine.Parse<Parameters>(args);

                Console.WriteLine("Puertos por servicio:");
                foreach (KeyValuePair<string, int> kv in p.PuertosPorServicio)
                    Console.WriteLine($"  {kv.Key,-12} -> {kv.Value}");

                IEnumerable<string> tags = p.Etiquetas ?? Enumerable.Empty<string>();
                Console.WriteLine("Etiquetas: " + string.Join(", ", tags));
                Console.WriteLine("Fin OK.");
            }
            catch (CommandParserBaseException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }

            try
            {
                Console.ReadKey();
            }
            catch (InvalidOperationException)
            {
                // Sin consola interactiva (p. ej. salida redirigida): omitir pausa.
            }
        }
    }
}
