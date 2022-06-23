using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FlagAttribute : BaseArgumentAttribute
    {
        public FlagAttribute(string keyword, string shortKeyword, string helpText = "")
            : base(keyword, shortKeyword, helpText)
        {
        }

        internal override void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments)
        {
            if (property.PropertyType != typeof(bool)) throw new WrongPropertyTypeException($"La propiedad de asignacion de un argumento flag ({property.Name}), debe ser de tipo booleano");
            property.SetValue(targetObject, true);
            CLI_Arguments.RemoveRange(0, 1);
        }

        internal void Clear(object targetObject, PropertyInfo property) 
        {
            if (property.PropertyType != typeof(bool)) throw new WrongPropertyTypeException($"La propiedad de asignacion de un argumento flag ({property.Name}), debe ser de tipo booleano");
            property.SetValue(targetObject, false);
        }

    }
}
