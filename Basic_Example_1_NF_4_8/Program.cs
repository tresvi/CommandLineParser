using Tresvi.CommandParser;
using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Exceptions;
using System;

namespace Basic_Example_1_NF_4_8
{

    //Clase que Modela los parametros esperados
    public class Parameters
    {
        [Option("archivo-salida", 'a', true, "Nombre del archivo a crear.")]
        public string ArchivoSalida { get; set; }

        [Option("tamano", 't', true,  "Tamaño del archivo en bytes.")]
        public int Tamano { get; set; }

        [Flag("notificar", 'n', "Indica si se debe notificar por mail el resultado del proceso.")]
        public bool NotificarPorMail { get; set; }
    }


    internal class Program
    {
        //Se puede invocar como:
        //  \>Basic_Example_1_NF_4_8.exe --archivo-salida "C:\Temp\archivo1.txt --tamano 120 --notificar
        // o bien reemplazar el nombre largo de los parametros por los cortos
        //  \>Basic_Example_1_NF_4_8.exe -a "C:\Temp\archivo1.txt -t 120 --notificar
        static void Main(string[] args)
        {
            try
            {
                Parameters parametros = CommandLine.Parse<Parameters>(args);
                Console.WriteLine($"Archivo de Salida: {parametros.ArchivoSalida}");
                Console.WriteLine($"Tamano: {parametros.Tamano}");
                Console.WriteLine($"Notificar: {parametros.NotificarPorMail}");
                Console.WriteLine("Fin OK!!");
            }
            catch (CommandParserBaseException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
