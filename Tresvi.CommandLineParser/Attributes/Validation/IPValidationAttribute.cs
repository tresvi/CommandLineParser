using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IPValidationAttribute : ValidationAttributeBase
    {
        private readonly bool _allowIPv4;
        private readonly bool _allowIPv6;
        private readonly bool _allowPort;

        /// <summary>
        /// Valida que el valor del parámetro sea una dirección IP válida (IPv4 o IPv6), opcionalmente con puerto.
        /// </summary>
        /// <param name="allowIPv4">Indica si se permiten direcciones IPv4. Por defecto es true.</param>
        /// <param name="allowIPv6">Indica si se permiten direcciones IPv6. Por defecto es true.</param>
        /// <param name="allowPort">Indica si se permite incluir el puerto en el formato IP:puerto o [IP]:puerto. Por defecto es false.</param>
        public IPValidationAttribute(bool allowIPv4 = true, bool allowIPv6 = true, bool allowPort = false)
        {
            if (!allowIPv4 && !allowIPv6)
                throw new ArgumentException("Debe permitir al menos IPv4 o IPv6.", nameof(allowIPv4));

            _allowIPv4 = allowIPv4;
            _allowIPv6 = allowIPv6;
            _allowPort = allowPort;
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

            // Si se permite puerto, extraerlo del string
            if (_allowPort)
            {
                // Para IPv6 con puerto, el formato es [::1]:8080
                if (inputValue.StartsWith("[") && inputValue.Contains("]:"))
                {
                    int closingBracketIndex = inputValue.IndexOf("]:");
                    if (closingBracketIndex > 0)
                    {
                        ipAddress = inputValue.Substring(1, closingBracketIndex - 1);
                        port = inputValue.Substring(closingBracketIndex + 2);
                    }
                    else
                    {
                        throw new InvalidIPAddressException(
                            $"El valor '{inputValue}' del parámetro {parameter.Key} tiene un formato de puerto inválido para IPv6. Use el formato [IP]:puerto (ej: [::1]:8080).");
                    }
                }
                // Para IPv4 con puerto, el formato es 192.168.1.1:8080
                else if (inputValue.Contains(":"))
                {
                    int lastColonIndex = inputValue.LastIndexOf(':');
                    if (lastColonIndex > 0)
                    {
                        ipAddress = inputValue.Substring(0, lastColonIndex);
                        port = inputValue.Substring(lastColonIndex + 1);
                    }
                }

                // Validar el puerto si se especificó
                if (!string.IsNullOrEmpty(port))
                {
                    if (!int.TryParse(port, out int portNumber) || portNumber < 1 || portNumber > 65535)
                    {
                        throw new InvalidIPAddressException(
                            $"El puerto '{port}' del parámetro {parameter.Key} no es válido. Debe ser un número entre 1 y 65535.");
                    }
                }
                else
                {
                    throw new InvalidIPAddressException(
                        $"El valor '{inputValue}' del parámetro {parameter.Key} debe incluir un puerto en el formato IP:puerto o [IP]:puerto.");
                }
            }
            else
            {
                // Si no se permite puerto, verificar que no haya formato IP:puerto
                // Primero verificar formato IPv6 con puerto: [::1]:8080
                if (inputValue.StartsWith("[") && inputValue.Contains("]:"))
                {
                    throw new InvalidIPAddressException(
                        $"El valor '{inputValue}' del parámetro {parameter.Key} incluye un puerto, pero el validador no permite puertos. Use allowPort: true para permitir puertos.");
                }
                // Luego intentar parsear como IP completa (puede ser IPv6 con múltiples :)
                else if (IPAddress.TryParse(inputValue, out _))
                {
                    // Es una IP válida sin puerto, continuar con la validación normal
                }
                else
                {
                    // No es una IP válida completa, podría ser IPv4:puerto
                    // Para IPv4 con puerto: formato es 192.168.1.1:8080
                    if (inputValue.Contains(":"))
                    {
                        int lastColonIndex = inputValue.LastIndexOf(':');
                        if (lastColonIndex > 0)
                        {
                            string possibleIP = inputValue.Substring(0, lastColonIndex);
                            string possiblePort = inputValue.Substring(lastColonIndex + 1);
                            
                            // Si la parte después del último : es un número válido de puerto
                            if (int.TryParse(possiblePort, out int portNum) && portNum >= 1 && portNum <= 65535)
                            {
                                // Verificar si la parte antes del : es una IP válida
                                if (IPAddress.TryParse(possibleIP, out _))
                                {
                                    throw new InvalidIPAddressException(
                                        $"El valor '{inputValue}' del parámetro {parameter.Key} incluye un puerto, pero el validador no permite puertos. Use allowPort: true para permitir puertos.");
                                }
                            }
                        }
                    }
                }
            }

            // Validar la dirección IP
            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIP))
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} no es una dirección IP válida.");
            }

            // Validar tipo de IP permitido
            if (parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !_allowIPv4)
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} es una dirección IPv4, pero solo se permiten direcciones IPv6.");
            }

            if (parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && !_allowIPv6)
            {
                throw new InvalidIPAddressException(
                    $"El valor '{ipAddress}' del parámetro {parameter.Key} es una dirección IPv6, pero solo se permiten direcciones IPv4.");
            }

            return true;
        }
    }
}

