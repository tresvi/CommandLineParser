using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Attributes.Validation;
using System;

namespace ParamValidation_Example_NF_4_8
{
    /// <summary>
    /// Clase que demuestra el uso de diferentes validadores de parámetros.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Ejemplo de StringListValidationAttribute: Valida que el ambiente sea uno de los valores permitidos.
        /// </summary>
        [StringListValidation(new[] { "dev", "test", "prod" }, false)]
        [Option("environment", 'e', true, helpText : "Ambiente de ejecución (dev, test, prod).")]
        public string Environment { get; set; }

        /// <summary>
        /// Ejemplo de EmailValidationAttribute: Valida que el email tenga un formato válido.
        /// </summary>
        [EmailValidation]
        [Option("email", 'm', true, helpText : "Dirección de correo electrónico del administrador.")]
        public string Email { get; set; }

        /// <summary>
        /// Ejemplo de IPValidationAttribute: Valida que sea una dirección IP válida (IPv4 o IPv6).
        /// </summary>
        [IPValidation(allowIPv4: true, allowIPv6: true)]
        [Option("server-ip", 'i', true, helpText : "Dirección IP del servidor (IPv4 o IPv6).")]
        public string ServerIP { get; set; }

        /// <summary>
        /// Ejemplo de IPValidationAttribute solo IPv4: Valida que sea una dirección IPv4.
        /// </summary>
        [IPValidation(allowIPv4: true, allowIPv6: false)]
        [Option("database-ip", 'd', false, helpText : "Dirección IP de la base de datos (solo IPv4).")]
        public string DatabaseIP { get; set; }

        /// <summary>
        /// Ejemplo de IPValidationAttribute con puerto: Valida que sea una dirección IP válida con puerto (formato: IP:puerto o [IP]:puerto).
        /// </summary>
        [IPValidation(allowIPv4: true, allowIPv6: true, portUsage: PortUsage.Optional)]
        [Option("api-endpoint", 'a', false, HelpText = "Endpoint de la API con puerto (ej: 192.168.1.1:8080 o [::1]:443).")]
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// Ejemplo de RegexValidationAttribute: Valida que el código de producto tenga el formato correcto (3 letras + 4 números).
        /// </summary>
        [RegexValidation(@"^[A-Z]{3}\d{4}$", errorMessage: "El código de producto debe tener el formato: 3 letras mayúsculas seguidas de 4 dígitos (ej: ABC1234).")]
        [Option("product-code", 'p', false, helpText : "Código de producto (formato: ABC1234).")]
        public string ProductCode { get; set; }

        /// <summary>
        /// Ejemplo de RegexValidationAttribute con patrón de teléfono: Valida formato de teléfono.
        /// </summary>
        [RegexValidation(@"^\+?\d{1,3}[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", errorMessage: "El teléfono debe tener un formato válido (ej: +1-555-123-4567).")]
        [Option("phone", 't', false, helpText : "Número de teléfono de contacto.")]
        public string Phone { get; set; }

        /// <summary>
        /// Ejemplo de FileExistsAttribute: Valida que el archivo de configuración exista.
        /// </summary>
        [FileExists]
        [Option("config-file", 'c', true, helpText : "Ruta al archivo de configuración (debe existir).")]
        public string ConfigFile { get; set; }

        /// <summary>
        /// Ejemplo de FileNotExistsAttribute: Valida que el archivo de salida no exista (para evitar sobrescribir).
        /// </summary>
        [FileNotExists]
        [Option("output-file", 'o', false, helpText : "Ruta al archivo de salida (no debe existir).")]
        public string OutputFile { get; set; }

        /// <summary>
        /// Ejemplo de DirectoryExistsAttribute: Valida que el directorio de logs exista.
        /// </summary>
        [DirectoryExists]
        [Option("log-dir", 'l', false, helpText : "Directorio donde se guardarán los logs (debe existir).")]
        public string LogDirectory { get; set; }

        /// <summary>
        /// Ejemplo de DirectoryNotExistsAttribute: Valida que el directorio de trabajo no exista (para crear uno nuevo).
        /// </summary>
        [DirectoryNotExists]
        [Option("work-dir", 'w', false, helpText : "Directorio de trabajo (no debe existir, se creará uno nuevo).")]
        public string WorkDirectory { get; set; }
    }
}

