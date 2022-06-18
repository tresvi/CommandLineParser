using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_CommandParser.Models
{
    internal class Param_OutFileNotExists
    {
        [DirectoryNotExists]
        [Option("outputfile", "o", true, HelpText = "Archivo de salida resultante del procesamiento.")]
        public string? OutputFile { get; set; }


    }
}
