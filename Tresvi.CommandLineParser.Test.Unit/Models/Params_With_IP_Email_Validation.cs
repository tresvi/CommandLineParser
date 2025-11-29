using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Attributtes.Keywords;

namespace Test_CommandParser.Models
{
    internal class Params_With_IPValidation_IPv4_IPv6
    {
        [IPValidation(allowIPv4: true, allowIPv6: true)]
        [Option("ipaddress", 'i', true, HelpText = "Dirección IP (IPv4 o IPv6).")]
        public string? IPAddress { get; set; }
    }

    internal class Params_With_IPValidation_IPv4_Only
    {
        [IPValidation(allowIPv4: true, allowIPv6: false)]
        [Option("ipaddress", 'i', true, HelpText = "Dirección IP (solo IPv4).")]
        public string? IPAddress { get; set; }
    }

    internal class Params_With_IPValidation_IPv6_Only
    {
        [IPValidation(allowIPv4: false, allowIPv6: true)]
        [Option("ipaddress", 'i', true, HelpText = "Dirección IP (solo IPv6).")]
        public string? IPAddress { get; set; }
    }

    internal class Params_With_EmailValidation
    {
        [EmailValidation]
        [Option("email", 'e', true, HelpText = "Dirección de correo electrónico.")]
        public string? Email { get; set; }
    }

    internal class Params_With_IP_And_Email_Validation
    {
        [IPValidation]
        [Option("serverip", 's', true, HelpText = "Dirección IP del servidor.")]
        public string? ServerIP { get; set; }

        [EmailValidation]
        [Option("adminemail", 'a', true, HelpText = "Correo electrónico del administrador.")]
        public string? AdminEmail { get; set; }
    }

    internal class Params_With_IPValidation_WithPort
    {
        [IPValidation(allowIPv4: true, allowIPv6: true, allowPort: true)]
        [Option("ipaddress", 'i', true, HelpText = "Dirección IP con puerto (formato: IP:puerto o [IP]:puerto).")]
        public string? IPAddress { get; set; }
    }
}

