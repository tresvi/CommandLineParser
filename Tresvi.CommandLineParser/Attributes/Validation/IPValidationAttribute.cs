using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Validation
{
    public enum PortUsage
    {
        Never = 0,
        Optional = 1,
        Required = 2
    }

    /// <summary>
    /// Indica qué familias de dirección IP acepta el validador.
    /// </summary>
    public enum AllowedIpVersion
    {
        IPv4 = 0,
        IPv6 = 1,
        Both = 2
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IPValidationAttribute : ValidationAttributeBase
    {
        private readonly AllowedIpVersion _allowedIpVersion;
        private readonly PortUsage _portUsage;

        /// <summary>
        /// Valida que el valor del parámetro sea una dirección IP válida (IPv4 o IPv6), opcionalmente con puerto.
        /// </summary>
        /// <param name="allowedIpVersion">Indica si se permiten solo IPv4, solo IPv6 o ambas. Por defecto es <see cref="AllowedIpVersion.Both"/>.</param>
        /// <param name="portUsage">Controla si el puerto está prohibido, es opcional o es obligatorio. Por defecto es Never.</param>
        public IPValidationAttribute(AllowedIpVersion allowedIpVersion = AllowedIpVersion.Both, PortUsage portUsage = PortUsage.Never)
        {
            _allowedIpVersion = allowedIpVersion;
            _portUsage = portUsage;
        }

        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string inputValue = parameter.Value.Trim();

            if (string.IsNullOrWhiteSpace(inputValue))
            {
                throw new InvalidIPAddressException(
                    $"El valor del parámetro {parameter.Key} no puede estar vacío.");
            }

            string ipAddress = inputValue;
            string port = null;
            bool hasPort = false;

            if (_portUsage != PortUsage.Never)
            {
                // IPv6 con formato [IP]:puerto
                if (inputValue.StartsWith("[") && inputValue.Contains("]:"))
                {
                    int closingBracketIndex = inputValue.IndexOf("]:");
                    if (closingBracketIndex > 0)
                    {
                        ipAddress = inputValue.Substring(1, closingBracketIndex - 1);
                        port = inputValue.Substring(closingBracketIndex + 2);
                        hasPort = true;
                    }
                    else
                    {
                        throw new InvalidIPAddressException(
                            $"El valor '{inputValue}' del parámetro {parameter.Key} tiene un formato de puerto inválido para IPv6. Use el formato [IP]:puerto (ej: [::1]:8080).");
                    }
                }
                // IPv4 con puerto: 192.168.1.1:8080
                else
                {
                    int colonCount = inputValue.Split(':').Length - 1;
                    int lastColonIndex = inputValue.LastIndexOf(':');
                    if (colonCount == 1 && lastColonIndex > 0)
                    {
                        ipAddress = inputValue.Substring(0, lastColonIndex);
                        port = inputValue.Substring(lastColonIndex + 1);
                        hasPort = true;
                    }
                }

                if (hasPort)
                {
                    ValidatePort(port, parameter.Key);
                }
                else if (_portUsage == PortUsage.Required)
                {
                    throw new InvalidIPAddressException(
                        $"El valor '{inputValue}' del parámetro {parameter.Key} debe incluir un puerto en el formato IP:puerto o [IP]:puerto.");
                }
            }
            else
            {
                // Si no se permite puerto, verificar que no haya formato con puerto
                if (ContainsPortNotation(inputValue))
                {
                    throw new InvalidIPAddressException(
                        $"El valor '{inputValue}' del parámetro {parameter.Key} incluye un puerto, pero el validador no permite puertos.");
                }
            }

            // Validar la dirección IP
            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIP))
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} no es una dirección IP válida.");
            }

            // Validar tipo de IP permitido
            bool allowIPv4 = _allowedIpVersion == AllowedIpVersion.Both || _allowedIpVersion == AllowedIpVersion.IPv4;
            bool allowIPv6 = _allowedIpVersion == AllowedIpVersion.Both || _allowedIpVersion == AllowedIpVersion.IPv6;

            if (parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !allowIPv4)
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} es una dirección IPv4, pero solo se permiten direcciones IPv6.");
            }

            if (parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && !allowIPv6)
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} es una dirección IPv6, pero solo se permiten direcciones IPv4.");
            }

            return true;
        }

        private static void ValidatePort(string port, string parameterKey)
        {
            if (string.IsNullOrWhiteSpace(port))
            {
                throw new InvalidIPAddressException(
                    $"El parámetro {parameterKey} requiere un puerto válido.");
            }

            if (!int.TryParse(port, out int portNumber) || portNumber < 1 || portNumber > 65535)
            {
                throw new InvalidIPAddressException(
                    $"El puerto '{port}' del parámetro {parameterKey} no es válido. Debe ser un número entre 1 y 65535.");
            }
        }

        private static bool ContainsPortNotation(string inputValue)
        {
            if (inputValue.StartsWith("[") && inputValue.Contains("]:"))
                return true;

            int colonCount = inputValue.Split(':').Length - 1;
            if (colonCount == 1)
            {
                int lastColonIndex = inputValue.LastIndexOf(':');
                if (lastColonIndex > 0)
                {
                    string possibleIP = inputValue.Substring(0, lastColonIndex);
                    string possiblePort = inputValue.Substring(lastColonIndex + 1);

                    if (int.TryParse(possiblePort, out int portNum) && portNum >= 1 && portNum <= 65535
                        && IPAddress.TryParse(possibleIP, out _))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

