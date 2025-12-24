using System;

namespace Tresvi.CommandParser.Attributes.Validation
{
    /// <summary>
    /// Permite mapear valores de entrada personalizados a valores de enumeración.
    /// Útil cuando se desea usar aliases o valores diferentes a los nombres del enum.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class EnumMapAttribute : Attribute
    {
        /// <summary>
        /// Valor de entrada que se mapeará al valor del enum.
        /// </summary>
        public string InputValue { get; }

        /// <summary>
        /// Valor del enum al que se mapea el InputValue.
        /// </summary>
        public object EnumValue { get; }

        /// <summary>
        /// Inicializa una nueva instancia de EnumMapAttribute.
        /// </summary>
        /// <param name="inputValue">Valor de entrada que se mapeará al enum (case-insensitive).</param>
        /// <param name="enumValue">Valor del enum al que se mapea.</param>
        public EnumMapAttribute(string inputValue, object enumValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
                throw new ArgumentException("El valor de entrada no puede ser nulo o vacío.", nameof(inputValue));

            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue), "El valor del enum no puede ser nulo.");

            InputValue = inputValue.Trim();
            EnumValue = enumValue;
        }
    }
}

