using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace MultiVerbExample_NF_4_8.Verbs
{
    [Verb("edit", "Comando para editar")]
    public class Edit
    {
        [Option("file", 'f', true, "", "Ruta del archivo a editar")]
        public string File { get; set; }

        [DateTimeFormatter("yyyyMMdd")]
        [Option("fecha", 'F', true, "", "Fecha de edición en formato yyyyMMdd")]
        public DateTime FechaEdicion { get; set; }
    }
}

