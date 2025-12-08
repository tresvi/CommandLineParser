using Tresvi.CommandParser.Attributtes.Keywords;
using System;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// Modelo para probar el caso donde el mismo keyword aparece en múltiples atributos
    /// de diferentes propiedades, causando matchCounter > 1 en FindMatchKeywordVsAttribute.
    /// Esto debería lanzar MultiDefinitionParameterException en la línea 99.
    /// 
    /// Nota: Este caso es teóricamente imposible con la validación actual de CheckForDuplicatedKeywordInClass,
    /// pero el test verifica que la rama esté cubierta si alguna vez se alcanza.
    /// </summary>
    internal class Param_MultipleAttributes_SameKeyword
    {
        [Option("test", 't', false, "Test 1")]
        public string? Test1 { get; set; }

        [Option("test", 'x', false, "Test 2")]
        public string? Test2 { get; set; }
    }
}

