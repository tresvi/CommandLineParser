using CommandParser.Attributes.Formatter;
using CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("edit", "Comando para editar")]
    internal class Edit
    {
        [Option("file", 'f', true, "", "Ruta del archivo a editar")]
        public string? File { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("fecha", 'F', true, "", "Ruta del archivo a editar")]
        public DateTime FechaEdicion { get; set; }
    }
}
