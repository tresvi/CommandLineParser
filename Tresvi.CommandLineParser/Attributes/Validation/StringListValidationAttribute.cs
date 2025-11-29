using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StringListValidationAttribute : ValidationAttributeBase
    {
        private readonly string[] _allowedValues;
        private readonly bool _caseSensitive;

        /// <summary>
        /// Valida que el valor del parámetro esté dentro de una lista de valores permitidos.
        /// </summary>
        /// <param name="allowedValues">Lista de valores permitidos para el parámetro.</param>
        /// <param name="caseSensitive">Indica si la comparación debe ser case-sensitive. Por defecto es true.</param>
        public StringListValidationAttribute(string[] allowedValues, bool caseSensitive = true)
        {
            if (allowedValues == null || allowedValues.Length == 0)
                throw new ArgumentException("Debe especificar al menos un valor permitido.", nameof(allowedValues));

            _allowedValues = allowedValues;
            _caseSensitive = caseSensitive;
        }

        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string value = parameter.Value.Trim();

            bool isValid;
            if (_caseSensitive)
            {
                isValid = _allowedValues.Contains(value);
            }
            else
            {
                isValid = _allowedValues.Any(allowedValue => 
                    string.Equals(allowedValue, value, StringComparison.OrdinalIgnoreCase));
            }

            if (!isValid)
            {
                string allowedValuesList = string.Join(", ", _allowedValues);
                throw new InvalidStringListValueException(
                    $"El valor '{value}' del parámetro {parameter.Key} no es válido. " +
                    $"Valores permitidos: {allowedValuesList}");
            }

            return true;
        }
    }
}

