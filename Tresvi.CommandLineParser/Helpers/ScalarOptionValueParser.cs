using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace Tresvi.CommandParser.Helpers
{
    /// <summary>
    /// Convierte un string en un valor escalar soportado por las opciones (mismos tipos que <see cref="Attributtes.Keywords.OptionAttribute"/>).
    /// </summary>
    internal static class ScalarOptionValueParser
    {
        internal static object ParseScalar(PropertyInfo property, string argumentName, string rawFieldContent, Type propertyType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(propertyType);
            bool isNullable = underlyingType != null;
            if (isNullable)
                propertyType = underlyingType;

            if (propertyType == typeof(string))
                return rawFieldContent;

            if (propertyType == typeof(DateTime))
            {
                if (DateTime.TryParse(rawFieldContent, out DateTime dateTimeValue))
                    return isNullable ? (DateTime?)dateTimeValue : (object)dateTimeValue;
                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como fecha válida.");
            }

            string parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor entero válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (propertyType == typeof(char))
            {
                if (!char.TryParse(rawFieldContent, out char parsedValue)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (char?)parsedValue : (object)parsedValue;
            }
            if (propertyType == typeof(byte))
            {
                if (!byte.TryParse(rawFieldContent, out byte parsedValue)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (byte?)parsedValue : (object)parsedValue;
            }
            if (propertyType == typeof(sbyte))
            {
                if (!sbyte.TryParse(rawFieldContent, out sbyte valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (sbyte?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(short))
            {
                if (!short.TryParse(rawFieldContent, out short parsedValue)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (short?)parsedValue : (object)parsedValue;
            }
            if (propertyType == typeof(ushort))
            {
                if (!ushort.TryParse(rawFieldContent, out ushort valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (ushort?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(int))
            {
                if (!int.TryParse(rawFieldContent, out int parsedValue)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (int?)parsedValue : (object)parsedValue;
            }
            if (propertyType == typeof(uint))
            {
                if (!uint.TryParse(rawFieldContent, out uint valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (uint?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(long))
            {
                if (!long.TryParse(rawFieldContent, out long valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (long?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(ulong))
            {
                if (!ulong.TryParse(rawFieldContent, out ulong valorTemp))
                    throw new ParseValueException(parseErrorMessage);
                return isNullable ? (ulong?)valorTemp : (object)valorTemp;
            }

            parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor decimal válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (propertyType == typeof(float))
            {
                if (!float.TryParse(rawFieldContent, out float valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (float?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(double))
            {
                if (!double.TryParse(rawFieldContent, out double valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (double?)valorTemp : (object)valorTemp;
            }
            if (propertyType == typeof(decimal))
            {
                if (!decimal.TryParse(rawFieldContent, out decimal valorTemp)) throw new ParseValueException(parseErrorMessage);
                return isNullable ? (decimal?)valorTemp : (object)valorTemp;
            }

            string boolParseError = $"El valor {rawFieldContent} no puede ser reconocido como tipo booleano. " +
                $"Valores Validos: true, false, YES, Y, NO, N, SI, S (case insensitive)";

            if (propertyType == typeof(bool))
            {
                string upper = rawFieldContent.ToUpper().Trim();
                bool? boolValue = null;
                if (upper == "SI" || upper == "YES" || upper == "TRUE" ||
                    upper == "S" || upper == "Y")
                    boolValue = true;
                else if (upper == "NO" || upper == "FALSE" ||
                    upper == "N" || upper == "F")
                    boolValue = false;
                else
                    throw new ParseValueException(boolParseError);

                return isNullable ? boolValue : (object)boolValue.Value;
            }

            if (propertyType.IsEnum)
                return ParseEnumValue(property, rawFieldContent, argumentName, propertyType);

            throw new ParseValueException($"El parametro \"{argumentName}\" de valor \"{rawFieldContent}\" se esta asignando " +
                $"a la \"{property.Name}\" de tipo {property.PropertyType.Name} el cual no es soportado por esta biblioteca");
        }

        private static object ParseEnumValue(PropertyInfo property, string rawValue, string argumentName, Type enumType)
        {
            rawValue = rawValue.Trim();

            EnumMapAttribute[] enumMaps = property.GetCustomAttributes(typeof(EnumMapAttribute), false)
                .Cast<EnumMapAttribute>()
                .ToArray();

            if (enumMaps.Length > 0)
            {
                foreach (EnumMapAttribute enumMap in enumMaps)
                {
                    if (string.Equals(enumMap.InputValue, rawValue, StringComparison.OrdinalIgnoreCase))
                    {
                        if (enumMap.EnumValue.GetType() != enumType)
                        {
                            throw new ParseValueException(
                                $"El valor mapeado '{enumMap.EnumValue}' del atributo EnumMap no corresponde al tipo de enum {enumType.Name}.");
                        }
                        return enumMap.EnumValue;
                    }
                }

                string mappedValues = string.Join(", ", enumMaps.Select(m => $"\"{m.InputValue}\""));
                string enumNames = string.Join(", ", Enum.GetNames(enumType));
                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" no acepta el valor \"{rawValue}\". " +
                    $"Valores mapeados permitidos: {mappedValues}. " +
                    $"Valores del enum disponibles: {enumNames}.");
            }

            if (int.TryParse(rawValue, out int numericValue))
            {
                Array enumValues = Enum.GetValues(enumType);
                foreach (object enumValue in enumValues)
                {
                    int enumIntValue = Convert.ToInt32(enumValue);
                    if (enumIntValue == numericValue)
                        return enumValue;
                }

                string[] validNames = Enum.GetNames(enumType);
                string validValuesList = string.Join(", ", validNames);
                string validNumericValues = string.Join(", ", enumValues.Cast<object>().Select(v => Convert.ToInt32(v).ToString()));

                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" no acepta el valor \"{rawValue}\" como valor válido del enum {enumType.Name}. " +
                    $"Valores válidos (nombres, case-insensitive): {validValuesList}. " +
                    $"Valores válidos (numéricos): {validNumericValues}.");
            }

            try
            {
                return Enum.Parse(enumType, rawValue, ignoreCase: true);
            }
            catch (ArgumentException)
            {
                string[] validNames = Enum.GetNames(enumType);
                Array validValues = Enum.GetValues(enumType);
                string validValuesList = string.Join(", ", validNames);
                string validNumericValues = string.Join(", ", validValues.Cast<object>().Select(v => Convert.ToInt32(v).ToString()));

                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" no acepta el valor \"{rawValue}\" como valor válido del enum {enumType.Name}. " +
                    $"Valores válidos (nombres, case-insensitive): {validValuesList}. " +
                    $"Valores válidos (numéricos): {validNumericValues}.");
            }
        }
    }
}
