using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    /// <summary>
    /// Flag y Option comparten la misma keyword larga para ejercitar matchCounter &gt; 1 en FindMatchKeywordVsAttribute.
    /// </summary>
    internal class Param_DuplicateKeyword_FlagAndOption
    {
        [Flag("shared", 'f', "flag")]
        public bool Shared { get; set; }

        [Option("shared", 'o', false, "opción")]
        public string? Opt { get; set; }
    }
}
