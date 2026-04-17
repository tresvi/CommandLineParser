using Tresvi.CommandParser.Attributes.Formatter;
using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Exceptions;
using Tresvi.CommandParser.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : BaseArgumentAttribute
    {
        internal bool IsRequired { get; set; }

        /// <summary>
        /// Si es true, la misma opción puede aparecer varias veces y los valores se acumulan en una
        /// propiedad <see cref="System.Collections.Generic.IDictionary{TKey,TValue}"/> con clave string
        /// (cada valor con forma clave=valor) o en una secuencia <see cref="System.Collections.Generic.IEnumerable{T}"/> (excluye <see cref="string"/>).
        /// </summary>
        public bool AllowMultiple { get; set; }

        public OptionAttribute(string keyword, char shortKeyword, bool isRequired, string helpText = "", bool allowMultiple = false)
            : base(keyword, shortKeyword, helpText)
        {
            IsRequired = isRequired;
            AllowMultiple = allowMultiple;
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
                if (attrib is ValidationAttributeBase checkAttrib)
                    checkAttrib.Check(parameter, property);
                else if (attrib is FormatterBaseAttribute formatterAttrib)
                    formattedValue = formatterAttrib.ApplyFormat(parameter, property);
            }

            CLI_Arguments.RemoveRange(0, 2);

            if (AllowMultiple)
            {
                if (!RepeatableOptionBinder.IsRepeatableCompatibleType(property.PropertyType))
                {
                    throw new InvalidOperationException(
                        $"La propiedad \"{property.Name}\" no admite AllowMultiple: use " +
                        $"IDictionary<string, TValue> o una secuencia IEnumerable<T> distinta de string.");
                }

                if (!(formattedValue is string rawRepeatable))
                {
                    throw new ParseValueException(
                        $"El parámetro repetible \"{parameter.Key}\" solo admite valores de texto en la línea de comandos.");
                }

                if (RepeatableOptionBinder.TryGetStringKeyedDictionaryValueType(property.PropertyType, out _))
                    RepeatableOptionBinder.ApplyDictionaryIncrement(property, targetObject, parameter.Key, rawRepeatable);
                else
                    RepeatableOptionBinder.ApplySequenceIncrement(property, targetObject, parameter.Key, rawRepeatable);

                return;
            }

            SetValue(targetObject, property, formattedValue, parameter.Key);
        }


        private void SetValue(object targetObject, PropertyInfo property, object value, string argumentName)
        {
            if (value == null) return;

            Type declaredType = property.PropertyType;
            Type underlyingNullable = Nullable.GetUnderlyingType(declaredType);
            bool isNullable = underlyingNullable != null;

            if (declaredType == typeof(string))
            {
                property.SetValue(targetObject, value);
                return;
            }

            if (declaredType == typeof(DateTime) || underlyingNullable == typeof(DateTime))
            {
                if (value is DateTime dt)
                {
                    if (isNullable) property.SetValue(targetObject, (DateTime?)dt);
                    else property.SetValue(targetObject, dt);
                }
                else
                {
                    string s = value as string;
                    if (s == null)
                        throw new ParseValueException(
                            $"El parametro \"{argumentName}\" no pudo interpretarse como fecha para la propiedad \"{property.Name}\".");
                    object parsed = ScalarOptionValueParser.ParseScalar(property, argumentName, s, declaredType);
                    property.SetValue(targetObject, parsed);
                }

                return;
            }

            string rawFieldContent = value as string;
            if (rawFieldContent == null)
            {
                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" no pudo interpretarse para la propiedad \"{property.Name}\".");
            }

            object parsedScalar = ScalarOptionValueParser.ParseScalar(property, argumentName, rawFieldContent, declaredType);
            property.SetValue(targetObject, parsedScalar);
        }
    }
}
