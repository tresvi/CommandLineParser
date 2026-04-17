using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Helpers
{
    /// <summary>
    /// Acumula valores en propiedades <see cref="System.Collections.Generic.IDictionary{TKey,TValue}"/> o secuencias
    /// cuando la misma opción aparece varias veces en la línea de comandos.
    /// </summary>
    internal static class RepeatableOptionBinder
    {
        internal static bool TryGetStringKeyedDictionaryValueType(Type propertyType, out Type valueType)
        {
            foreach (Type itf in propertyType.GetInterfaces())
            {
                if (itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    Type[] args = itf.GetGenericArguments();
                    if (args[0] == typeof(string))
                    {
                        valueType = args[1];
                        return true;
                    }
                }
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] args = propertyType.GetGenericArguments();
                if (args[0] == typeof(string))
                {
                    valueType = args[1];
                    return true;
                }
            }

            valueType = null;
            return false;
        }

        internal static bool TryGetSequenceElementType(Type propertyType, out Type elementType)
        {
            if (propertyType == typeof(string))
            {
                elementType = null;
                return false;
            }

            if (TryGetStringKeyedDictionaryValueType(propertyType, out _))
            {
                elementType = null;
                return false;
            }

            Type seqInterface = null;
            foreach (Type itf in propertyType.GetInterfaces())
            {
                if (itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    seqInterface = itf;
                    break;
                }
            }

            if (seqInterface == null && propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                seqInterface = propertyType;

            if (seqInterface == null)
            {
                elementType = null;
                return false;
            }

            elementType = seqInterface.GetGenericArguments()[0];
            return true;
        }

        internal static bool IsRepeatableCompatibleType(Type propertyType)
        {
            return TryGetStringKeyedDictionaryValueType(propertyType, out _) || TryGetSequenceElementType(propertyType, out _);
        }

        internal static void ApplyDictionaryIncrement(PropertyInfo property, object targetObject, string argumentName, string rawKeyValue)
        {
            if (!TryGetStringKeyedDictionaryValueType(property.PropertyType, out Type valueType))
                throw new InvalidOperationException("Tipo de propiedad no compatible con acumulación en diccionario.");

            int eq = rawKeyValue.IndexOf('=');
            if (eq <= 0 || eq == rawKeyValue.Length - 1)
                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" espera pares clave=valor (por ejemplo Proceso1=3). Valor recibido: \"{rawKeyValue}\".");

            string key = rawKeyValue.Substring(0, eq).Trim();
            if (key.Length == 0)
                throw new ParseValueException(
                    $"El parametro \"{argumentName}\" tiene una clave vacía en \"{rawKeyValue}\".");

            string valuePart = rawKeyValue.Substring(eq + 1);
            object parsedValue = ScalarOptionValueParser.ParseScalar(property, argumentName, valuePart, valueType);

            Type dictOpen = typeof(Dictionary<,>);
            Type dictType = dictOpen.MakeGenericType(typeof(string), valueType);
            object dictObj = property.GetValue(targetObject);
            if (dictObj == null)
            {
                dictObj = Activator.CreateInstance(dictType);
                property.SetValue(targetObject, dictObj);
            }

            PropertyInfo indexer = dictType.GetProperty("Item", new[] { typeof(string) });
            indexer.SetValue(dictObj, parsedValue, new object[] { key });
        }

        internal static void ApplySequenceIncrement(PropertyInfo property, object targetObject, string argumentName, string rawElement)
        {
            if (!TryGetSequenceElementType(property.PropertyType, out Type elementType))
                throw new InvalidOperationException("Tipo de propiedad no compatible con acumulación en secuencia.");

            object parsedElement = ScalarOptionValueParser.ParseScalar(property, argumentName, rawElement, elementType);

            Type listType = typeof(List<>).MakeGenericType(elementType);
            object existing = property.GetValue(targetObject);
            object listObj;

            if (existing == null)
                listObj = Activator.CreateInstance(listType);
            else if (listType.IsInstanceOfType(existing))
                listObj = existing;
            else
            {
                Type enumerableOfElem = typeof(IEnumerable<>).MakeGenericType(elementType);
                if (!enumerableOfElem.IsInstanceOfType(existing))
                {
                    throw new ParseValueException(
                        $"La propiedad \"{property.Name}\" no pudo acumularse: se esperaba null, {listType.Name} o una secuencia compatible.");
                }

                listObj = Activator.CreateInstance(listType);
                MethodInfo addRange = listType.GetMethod("AddRange", new[] { enumerableOfElem });
                addRange.Invoke(listObj, new object[] { existing });
            }

            MethodInfo add = listType.GetMethod("Add", new[] { elementType });
            add.Invoke(listObj, new[] { parsedElement });

            Type propType = property.PropertyType;
            object toAssign = listObj;
            if (propType.IsArray && propType.GetElementType() == elementType)
            {
                MethodInfo toArray = listType.GetMethod("ToArray", Type.EmptyTypes);
                toAssign = toArray.Invoke(listObj, null);
            }
            else if (!propType.IsAssignableFrom(listObj.GetType()))
            {
                throw new ParseValueException(
                    $"La propiedad \"{property.Name}\" de tipo {propType.Name} no admite asignación desde la lista acumulada.");
            }

            property.SetValue(targetObject, toAssign);
        }
    }
}
