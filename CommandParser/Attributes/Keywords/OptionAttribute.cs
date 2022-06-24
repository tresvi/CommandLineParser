using CommandParser.Attributes.Formatter;
using CommandParser.Attributes.Validation;
using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : BaseArgumentAttribute
    {
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }

        public OptionAttribute(string keyword, char shortKeyword, bool isRequired, string defaultValue = "", string helpText = "")
            : base(keyword, shortKeyword, helpText)
        {
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }


       internal override void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments)
        {
            string keyword = CLI_Arguments[0];

            if (CLI_Arguments.Count < 2)
                throw new ValueNotFoundException($"No se especificó el valor del parámetro \"{keyword}\"");
            
            string value = CLI_Arguments[1];

            KeyValuePair<string, string> parameter = new KeyValuePair<string, string>(keyword, value);
            object formattedValue = value;

            foreach (Attribute attrib in property.GetCustomAttributes())
            {
                if (attrib is ValidationAttributeBase checkAttrib)              //Aplica atributos que solo chequean el dato
                    checkAttrib.Check(parameter, property);
                else if (attrib is FormatterAttributeBase formatterAttrib) //Aplica atributos que modifican el modifican (lo formatean)
                    formattedValue = formatterAttrib.ApplyFormat(parameter, property);
            }

            CLI_Arguments.RemoveRange(0, 2);
            SetValue(targetObject, property, formattedValue, parameter.Key);
        }


        private void SetValue(object targetObject, PropertyInfo property, object value, string argumentName)
        {
            if (value == null) return;

            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime))
            {
                property.SetValue(targetObject, value);
                return;
            }

            string rawFieldContent = (string)value;

            //Reviso si la asignacion se hace a alguna property de algun tipo entero
            string parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor entero válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (property.PropertyType == typeof(char))
            {
                if (!char.TryParse(rawFieldContent, out char parsedValue)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, parsedValue);
                return;
            }
            else if (property.PropertyType == typeof(byte))
            {
                if (!byte.TryParse(rawFieldContent, out byte parsedValue)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, parsedValue);
                return;
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                if (!sbyte.TryParse(rawFieldContent, out sbyte valorTemp)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(short))
            {
                if (!short.TryParse(rawFieldContent, out short parsedValue)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, parsedValue);
                return;
            }
            else if (property.PropertyType == typeof(ushort))
            {
                if (!ushort.TryParse(rawFieldContent, out ushort valorTemp)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(int))
            {
                if (!int.TryParse(rawFieldContent, out int parsedValue)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, parsedValue);
                return;
            }
            else if (property.PropertyType == typeof(uint))
            {
                if (!uint.TryParse(rawFieldContent, out uint valorTemp)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(long))
            {
                if (!long.TryParse(rawFieldContent, out long valorTemp)) throw new ParseValueException(parseErrorMessage);

                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(ulong))
            {
                if (!ulong.TryParse(rawFieldContent, out ulong valorTemp))
                    throw new ParseValueException(parseErrorMessage);
                property.SetValue(targetObject, valorTemp);
                return;
            }

            //Reviso si la asignacion se hace a una property de tipo punto flotante
            parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor decimal válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (property.PropertyType == typeof(float))
            {
                if (!float.TryParse(rawFieldContent, out float valorTemp)) throw new ParseValueException(parseErrorMessage);
                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(double))
            {
                if (!double.TryParse(rawFieldContent, out double valorTemp)) throw new ParseValueException(parseErrorMessage);
                property.SetValue(targetObject, valorTemp);
                return;
            }
            else if (property.PropertyType == typeof(decimal))
            {
                if (!decimal.TryParse(rawFieldContent, out decimal valorTemp)) throw new ParseValueException(parseErrorMessage);
                property.SetValue(targetObject, valorTemp);
                return;
            }

            //Reviso si la asignacion se hace a un booleano
            parseErrorMessage = $"El valor {rawFieldContent} no puede ser reconocido como tipo booleano. " +
                $"Valores Validos: true, false, YES, Y, NO, N, SI, S (case insensitive)";

            if (property.PropertyType == typeof(bool))
            {
                rawFieldContent = rawFieldContent.ToUpper().Trim();

                if (rawFieldContent == "SI" || rawFieldContent == "YES" || rawFieldContent == "TRUE" || 
                    rawFieldContent == "S" || rawFieldContent == "Y")
                {
                    property.SetValue(targetObject, true);
                    return;
                }
                else if (rawFieldContent == "NO" || rawFieldContent == "FALSE" || 
                    rawFieldContent == "N" || rawFieldContent == "F")
                {
                    property.SetValue(targetObject, false);
                    return;
                }
                else
                    throw new ParseValueException(parseErrorMessage);
            }

            throw new ParseValueException($"El parametro \"{argumentName}\" de valor \"{rawFieldContent}\" se esta asignando " +
                $"a la \"{property.Name}\" de tipo {property.PropertyType.Name} el cual no es soportado por esta biblioteca");
        }

    }
}
