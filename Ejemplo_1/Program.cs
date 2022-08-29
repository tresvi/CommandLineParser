using CommandParser;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using System;

namespace Ejemplo_1
{
    internal class Program
    {
        //Se puede invocar como:
        //  \>Ejemplo_1.exe --archivo-entrada "C:\Temp\archivo1.txt --tamano 120 --notificar
        // o bien reemplazar el nombre largo de los parametros por los cortos
        //  \>Ejemplo_1.exe -a "C:\Temp\archivo1.txt -t 120 --notificar
        static void Main(string[] args)
        {
            try
            {
                Parameters parametros = CommandLine.Parse<Parameters>(args);
                Console.WriteLine($"Archivo de Entrada: {parametros.ArchivoEntrada}");
                Console.WriteLine($"Tamano: {parametros.Tamano}");
                Console.WriteLine($"Notificar: {parametros.NotificarPorMail}");
                Console.WriteLine("Fin OK!!");
            }
            catch (CommandParserException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            Console.ReadKey();
        }


        //Clase que Modela los parametros esperados
        public class Parameters
        {
            [Option("archivo-entrada", 'a', true, HelpText = "Nombre del archivo a crear.")]
            public string ArchivoEntrada { get; set; }


            [Option("tamano", 't', true, HelpText = "Tamaño del archivo en bytes.")]
            public int Tamano { get; set; }


            [Flag("notificar", 'n', HelpText = "Indica si se debe notificar por mail el resultado del proceso.")]
            public bool NotificarPorMail { get; set; }
        }
    }
}
