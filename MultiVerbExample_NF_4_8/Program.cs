using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using MultiVerbExample_NF_4_8.Verbs;
using System;


/* Comandos para probar:
 *  .\MultiVerbExample_NF_4_8.exe add -d "C:\Temp" -n "test.txt"
 *  .\MultiVerbExample_NF_4_8.exe edit -f "C:\Temp\test.txt" -F 20241129
 *  .\MultiVerbExample_NF_4_8.exe delete -f "C:\Temp\test.txt"
 *  .\MultiVerbExample_NF_4_8.exe commit -f "C:\Temp\test.txt" -m "Primer commit"
 */

namespace MultiVerbExample_NF_4_8
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Ejemplo de uso con múltiples verbos
                // El método Parse puede recibir múltiples tipos de verbos
                object command = CommandLine.Parse(args, 
                    typeof(Add), 
                    typeof(Edit), 
                    typeof(Delete), 
                    typeof(Commit));

                // Usar pattern matching para determinar qué tipo de comando se ejecutó
                switch (command)
                {
                    case Add addCommand:
                        Console.WriteLine("=== Comando ADD ===");
                        Console.WriteLine($"Directorio: {addCommand.Directory}");
                        Console.WriteLine($"Nombre: {addCommand.Nombre}");
                        Console.WriteLine("Archivo agregado exitosamente.");
                        break;

                    case Edit editCommand:
                        Console.WriteLine("=== Comando EDIT ===");
                        Console.WriteLine($"Archivo: {editCommand.File}");
                        Console.WriteLine($"Fecha de edición: {editCommand.FechaEdicion:yyyy-MM-dd}");
                        Console.WriteLine("Archivo editado exitosamente.");
                        break;

                    case Delete deleteCommand:
                        Console.WriteLine("=== Comando DELETE ===");
                        Console.WriteLine($"Archivo: {deleteCommand.File}");
                        Console.WriteLine("Archivo eliminado exitosamente.");
                        break;

                    case Commit commitCommand:
                        Console.WriteLine("=== Comando COMMIT ===");
                        Console.WriteLine($"Archivo: {commitCommand.File}");
                        Console.WriteLine($"Mensaje: {commitCommand.Message}");
                        Console.WriteLine("Cambios confirmados exitosamente.");
                        break;

                    default:
                        Console.WriteLine("Tipo de comando desconocido.");
                        break;
                }

                Console.WriteLine("\nFin OK!!");
            }
            catch (CommandParserBaseException ex)
            {
                Console.WriteLine($"ERROR al interpretar la linea de comando: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}

