using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tresvi.CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RegexValidationAttribute : ValidationAttributeBase
    {
        private readonly Regex _regex;
        private readonly string _errorMessage;

        /// <summary>
        /// Valida que el valor del parámetro cumpla con el patrón de expresión regular especificado.
        /// </summary>
        /// <param name="pattern">Patrón de expresión regular a validar.</param>
        /// <param name="errorMessage">Mensaje de error personalizado. Si no se especifica, se usará un mensaje genérico.</param>
        /// <param name="regexOptions">Opciones de la expresión regular (por defecto: Compiled).</param>
        public RegexValidationAttribute(string pattern, string errorMessage = null, RegexOptions regexOptions = RegexOptions.Compiled)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("El patrón de expresión regular no puede estar vacío.", nameof(pattern));

            try
            {
                _regex = new Regex(pattern, regexOptions);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"El patrón de expresión regular '{pattern}' no es válido: {ex.Message}", nameof(pattern), ex);
            }

            _errorMessage = errorMessage;
        }

        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string value = parameter.Value.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidFormatException(
                    $"El valor del parámetro {parameter.Key} no puede estar vacío.");
            }

            if (!_regex.IsMatch(value))
            {
                string message = _errorMessage ?? 
                    $"El valor '{value}' del parámetro {parameter.Key} no cumple con el patrón requerido.";
                
                throw new InvalidFormatException(message);
            }

            return true;
        }
    }
}

