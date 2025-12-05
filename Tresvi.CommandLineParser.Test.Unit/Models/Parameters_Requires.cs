using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Attributes.Validation;

namespace Test_CommandParser.Models
{
    internal class Parameters_Requires
    {
        [Option("username", 'u', false, "Nombre de usuario")]
        [Requires(nameof(Password))]
        public string? Username { get; set; }

        [Option("password", 'p', false, "Contraseña")]
        public string? Password { get; set; }

        [Option("output-file", 'o', false, "Archivo de salida")]
        [Requires(nameof(OutputFormat), nameof(Encoding))]
        public string? OutputFile { get; set; }

        [Option("output-format", 'f', false, "Formato de salida")]
        public string? OutputFormat { get; set; }

        [Option("encoding", 'e', false, "Codificación")]
        public string? Encoding { get; set; }

        [Flag("encrypt", 'c', "Encriptar salida")]
        [Requires(nameof(EncryptionKey))]
        public bool Encrypt { get; set; }

        [Option("encryption-key", 'k', false, "Clave de encriptación")]
        public string? EncryptionKey { get; set; }

        [Flag("verbose", 'v', "Modo verbose")]
        [Requires(nameof(LogFile))]
        public bool Verbose { get; set; }

        [Option("log-file", 'l', false, "Archivo de log")]
        public string? LogFile { get; set; }
    }
}

