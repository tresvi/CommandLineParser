using Tresvi.CommandParser.Attributtes.Keywords;
using System.Collections.Generic;

namespace Tresvi.CommandLineParser.Test.Unit.Models
{
    public class Params_Repeatable_Dictionary
    {
        [Option("define", 'd', true, "Pares clave=valor (repetible).", true)]
        public Dictionary<string, int> Puertos { get; set; } = null!;
    }

    public class Params_Repeatable_Sequence
    {
        [Option("numero", 'n', true, "Números (repetible).", true)]
        public List<int> Numeros { get; set; } = null!;

        [Option("tag", 't', false, "Etiquetas (repetible).", true)]
        public IEnumerable<string> Tags { get; set; } = null!;
    }

    public class Params_Repeatable_IntArray
    {
        [Option("id", 'i', false, "Identificadores (repetible).", true)]
        public int[] Ids { get; set; } = null!;
    }

    public class Params_Repeatable_InvalidScalar
    {
        [Option("x", 'x', false, "", true)]
        public int Valor { get; set; }
    }
}
