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


        public OptionAttribute(string keyword, string shortKeyword, bool isRequired, string defaultValue = "", string helpText = "")
            : base(keyword, shortKeyword, isRequired, helpText)
        {
            DefaultValue = defaultValue;
        }


        // internal override void ParseAndAssign(int keywordIndex, PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments)
        internal override void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments)
        {
            ////Argument argument = DetectKeyword(CLI_Arguments);
            //Argument argument = DetectKeyword(ControlCLI_Arguments);

            //if (argument.NotFound)
            //{
            //    if (this.IsRequired)
            //        throw new RequiredParameterNotFoundException($"El parametro requerido {this.Keyword}/{this.ShortKeyword} no fue definido en la linea de comando");
            //    else
            //        return;
            //}

            //object value = argument.Value;

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
            //ControlCLI_Arguments.Remove(argument.Name);
            //ControlCLI_Arguments.Remove(argument.Value);

            SetValue(property, targetObject, formattedValue, parameter.Key);
        }


        //internal override void ParseAndAssign(PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments)
        //{
        //    ////Argument argument = DetectKeyword(CLI_Arguments);
        //    Argument argument = DetectKeyword(ControlCLI_Arguments);

        //    if (argument.NotFound)
        //    {
        //        if (this.IsRequired)
        //            throw new RequiredParameterNotFoundException($"El parametro requerido {this.Keyword}/{this.ShortKeyword} no fue definido en la linea de comando");
        //        else
        //            return;
        //    }

        //    object value = argument.Value;

        //    foreach (Attribute attrib in property.GetCustomAttributes())
        //    {
        //        if (attrib is ValidationAttributeBase checkAttrib)              //Aplica atributos que solo chequean el dato
        //            checkAttrib.Check(argument, property);
        //        else if (attrib is DecoratorFormatterAttributeBase formatterAttrib) //Aplica atributos que modifican el modifican (lo formatean)
        //            value = formatterAttrib.ApplyFormat(argument, property);
        //    }

        //    ControlCLI_Arguments.Remove(argument.Name);
        //    ControlCLI_Arguments.Remove(argument.Value);

        //    SetValue(property, targetObject, value, argument.Name);
        //}


        //internal override Parameter DetectKeyword(List<string> CLI_Arguments)
        //{
        //    Parameter keyword = new Parameter() { NotFound = true };
        //    int matchCounter = 0;

        //    for (int i = 0; i < CLI_Arguments.Count; i++)
        //    {
        //        if (CLI_Arguments[i] == this.Keyword || CLI_Arguments[i] == this.ShortKeyword)
        //        {
        //            keyword.Name = CLI_Arguments[i];
        //            keyword.Index = i;
        //            keyword.NotFound = false;
        //            matchCounter++;
        //        }
        //    }

        //    if (keyword.NotFound)
        //    {
        //        keyword.Name = this.Keyword;
        //        return keyword;
        //    }

        //    if (matchCounter > 1) throw new RepeatedKeywordDefinitionException($"El argumento {this.Keyword}/{this.ShortKeyword} fue definido mas de una vez");

        //    //Detecta si falta el valor de un ultimo parametro
        //    if (CLI_Arguments.Count < keyword.Index + 2) throw new ValueNotFoundException($"El valor del argumento {keyword.Name} no fue especificado");

        //    //Detecta si falta el valor de un parametro que no es el ultimo
        //    keyword.Value = CLI_Arguments[keyword.Index + 1];

        //    //TODO: Esto imposibilita que se puedan pasar valores que comeincen con "--". Definir si esto esta del todo bien en proximas versiones
        //    // if (keyword.Value.StartsWith("--")  || keyword.Value.StartsWith("-"))    //La ultima condicion, imposibilita leer nros negativos
        //    //     throw new ValueNotSpecifiedException($"El valor del argumento {keyword.Name} no fue especificado");

        //    return keyword;
        //}


        private void SetValue(PropertyInfo property, object targetObject, object value, string argumentName)
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

        internal /*override*/ Parameter DetectKeyword(List<string> CLI_Arguments)
        {
            throw new NotImplementedException();
        }
    }
}
