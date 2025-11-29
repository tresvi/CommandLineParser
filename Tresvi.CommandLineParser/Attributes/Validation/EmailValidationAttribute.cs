using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tresvi.CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class EmailValidationAttribute : ValidationAttributeBase
    {
        // Patrón de expresión regular para validar emails según RFC 5322 (simplificado pero robusto)
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9](?:[a-zA-Z0-9._+-]*[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9.-]*[a-zA-Z0-9])?\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string email = parameter.Value.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailAddressException(
                    $"El valor del parámetro {parameter.Key} no puede estar vacío.");
            }

            if (!EmailRegex.IsMatch(email))
            {
                throw new InvalidEmailAddressException(
                    $"El valor '{email}' del parámetro {parameter.Key} no es una dirección de correo electrónico válida.");
            }

            // Validación adicional: longitud máxima según RFC 5321
            if (email.Length > 254)
            {
                throw new InvalidEmailAddressException(
                    $"El valor '{email}' del parámetro {parameter.Key} excede la longitud máxima permitida para una dirección de correo electrónico (254 caracteres).");
            }

            // Validar que no tenga espacios
            if (email.Contains(" "))
            {
                throw new InvalidEmailAddressException(
                    $"El valor '{email}' del parámetro {parameter.Key} contiene espacios, lo cual no es válido en una dirección de correo electrónico.");
            }

            return true;
        }
    }
}

