﻿using CommandParser.Attributtes;

namespace Test_CommandParser.Models.Verbs
{
    [Verb("delete", "Comando para eliminar archivo", true)]
    internal class Commit
    {
        [Option("file", "f", true, "", "Ruta del archivo a confirmar")]
        public string? File { get; set; }


        [Option("message", "m", true, "", "Breve descripcion de las modificaciones")]
        public string? Message { get; set; }
    }
}
