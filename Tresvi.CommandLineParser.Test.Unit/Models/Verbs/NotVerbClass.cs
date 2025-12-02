using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models.Verbs
{
    internal class NotVerbClass
    {
        [Option("file", 'f', false, "Ruta del archivo a confirmar")]
        public string? File { get; set; } = @"C:\";
    }
}
