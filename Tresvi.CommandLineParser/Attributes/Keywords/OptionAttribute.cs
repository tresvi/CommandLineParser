using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : BaseArgumentAttribute
    {
        internal bool IsRequired { get; set; }

        public OptionAttribute(string keyword, char shortKeyword, bool isRequired, string helpText = "")
            : base(keyword, shortKeyword, helpText)
        {
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
                else if (attrib is FormatterBaseAttribute formatterAttrib) //Aplica atributos que modifican el modifican (lo formatean)
                    formattedValue = formatterAttrib.ApplyFormat(parameter, property);
            }

            CLI_Arguments.RemoveRange(0, 2);
            SetValue(targetObject, property, formattedValue, parameter.Key);
        }


        private void SetValue(object targetObject, PropertyInfo property, object value, string argumentName)
        {
            if (value == null) return;

            Type propertyType = property.PropertyType;
            
            // Detectar si el tipo es nullable (int?, bool?, DateTime?, etc.)
            Type underlyingType = Nullable.GetUnderlyingType(propertyType);
            bool isNullable = underlyingType != null;
            
            // Si es nullable, usar el tipo subyacente para el parsing
            if (isNullable)
                propertyType = underlyingType;
/*
            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime))
            {
                if (isNullable) {property.SetValue(targetObject, (propertyType?)value);}
                else {property.SetValue(targetObject, value);}

                return;
            }
*/

            // Manejar string (no nullable, pero puede ser string?)
            if (propertyType == typeof(string))
            {
                property.SetValue(targetObject, value);
                return;
            }

            string rawFieldContent = (string)value;

            // Manejar DateTime (tanto DateTime como DateTime?)
            if (propertyType == typeof(DateTime))
            {
                if (DateTime.TryParse(rawFieldContent, out DateTime dateTimeValue))
                {
                    if (isNullable) {property.SetValue(targetObject, (DateTime?)dateTimeValue);}
                    else {property.SetValue(targetObject, dateTimeValue);}
                }
                else
                    throw new ParseValueException($"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como fecha válida.");

                return;
            }

            ///Reviso si la asignacion se hace a alguna property de algun tipo entero
            string parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor entero válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (propertyType == typeof(char))
            {
                if (!char.TryParse(rawFieldContent, out char parsedValue)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) { property.SetValue(targetObject, (char?)parsedValue);}
                else {property.SetValue(targetObject, parsedValue);}

                return;
            }
            else if (propertyType == typeof(byte))
            {
                if (!byte.TryParse(rawFieldContent, out byte parsedValue)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) {property.SetValue(targetObject, (byte?)parsedValue);}
                else {property.SetValue(targetObject, parsedValue);}

                return;
            }
            else if (propertyType == typeof(sbyte))
            {
                if (!sbyte.TryParse(rawFieldContent, out sbyte valorTemp)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (sbyte?)valorTemp);
                else property.SetValue(targetObject, valorTemp);

                return;
            }
            else if (propertyType == typeof(short))
            {
                if (!short.TryParse(rawFieldContent, out short parsedValue)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (short?)parsedValue);
                else property.SetValue(targetObject, parsedValue);

                return;
            }
            else if (propertyType == typeof(ushort))
            {
                if (!ushort.TryParse(rawFieldContent, out ushort valorTemp)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (ushort?)valorTemp);
                else property.SetValue(targetObject, valorTemp);

                return;
            }
            else if (propertyType == typeof(int))
            {
                if (!int.TryParse(rawFieldContent, out int parsedValue)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (int?)parsedValue);
                else property.SetValue(targetObject, parsedValue);

                return;
            }
            else if (propertyType == typeof(uint))
            {
                if (!uint.TryParse(rawFieldContent, out uint valorTemp)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (uint?)valorTemp);
                else property.SetValue(targetObject, valorTemp);

                return;
            }
            else if (propertyType == typeof(long))
            {
                if (!long.TryParse(rawFieldContent, out long valorTemp)) throw new ParseValueException(parseErrorMessage);

                if (isNullable) property.SetValue(targetObject, (long?)valorTemp);
                else property.SetValue(targetObject, valorTemp);

                return;
            }
            else if (propertyType == typeof(ulong))
            {
                if (!ulong.TryParse(rawFieldContent, out ulong valorTemp))
                    throw new ParseValueException(parseErrorMessage);
                
                if (isNullable) {property.SetValue(targetObject, (ulong?)valorTemp);}
                else {property.SetValue(targetObject, valorTemp);}

                return;
            }

            ///Reviso si la asignacion se hace a una property de tipo punto flotante
            parseErrorMessage = $"El parametro \"{argumentName}\" no acepta el valor \"{rawFieldContent}\" como " +
                $"valor decimal válido. Verifique que el valor sea numerico y esté dentro del rango correspondiente";

            if (propertyType == typeof(float))
            {
                if (!float.TryParse(rawFieldContent, out float valorTemp)) throw new ParseValueException(parseErrorMessage);
                
                if (isNullable) { property.SetValue(targetObject, (float?)valorTemp) ;}
                else { property.SetValue(targetObject, valorTemp) ;}

                return;
            }
            else if (propertyType == typeof(double))
            {
                if (!double.TryParse(rawFieldContent, out double valorTemp)) throw new ParseValueException(parseErrorMessage);
                
                if (isNullable) {property.SetValue(targetObject, (double?)valorTemp);}
                else { property.SetValue(targetObject, valorTemp);}

                return;
            }
            else if (propertyType == typeof(decimal))
            {
                if (!decimal.TryParse(rawFieldContent, out decimal valorTemp)) throw new ParseValueException(parseErrorMessage);
                
                if (isNullable) property.SetValue(targetObject, (decimal?)valorTemp);
                else property.SetValue(targetObject, valorTemp);

                return;
            }

            //Reviso si la asignacion se hace a un booleano
            parseErrorMessage = $"El valor {rawFieldContent} no puede ser reconocido como tipo booleano. " +
                $"Valores Validos: true, false, YES, Y, NO, N, SI, S (case insensitive)";

            if (propertyType == typeof(bool))
            {
                rawFieldContent = rawFieldContent.ToUpper().Trim();

                bool? boolValue = null;
                if (rawFieldContent == "SI" || rawFieldContent == "YES" || rawFieldContent == "TRUE" || 
                    rawFieldContent == "S" || rawFieldContent == "Y")
                {
                    boolValue = true;
                }
                else if (rawFieldContent == "NO" || rawFieldContent == "FALSE" || 
                    rawFieldContent == "N" || rawFieldContent == "F")
                {
                    boolValue = false;
                }
                else
                {
                    throw new ParseValueException(parseErrorMessage);
                }

                if (isNullable) property.SetValue(targetObject, boolValue);
                else property.SetValue(targetObject, boolValue.Value);

                return;
            }

            throw new ParseValueException($"El parametro \"{argumentName}\" de valor \"{rawFieldContent}\" se esta asignando " +
                $"a la \"{property.Name}\" de tipo {property.PropertyType.Name} el cual no es soportado por esta biblioteca");
        }

    }
}
