using CommandParser.Attributtes;
using CommandParser.DecoratorAttributes;
using CommandParser.DecoratorAttributes.DecoratorFormatterAttributes;
using System;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("edit", "Comando para editar")]
    internal class Edit
    {
        [Option("file", "f", true, "", "Ruta del archivo a editar")]
        public string? File { get; set; }
    
        [DateTimeFormatter("yyyyMMdd")]
        [Option("fecha", "fe", true, "", "Ruta del archivo a editar")]
        public DateTime FechaEdicion { get; set; }
    }
}
