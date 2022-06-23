using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributes.Formatter
{
    //No se podra definir mas de un atributo de formato por property.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class FormatterAttributeBase : Attribute
    {
        public string Format { get; set; }

        public FormatterAttributeBase(string format)
        {
            this.Format = format;
        }

        //!!internal abstract object ApplyFormat(Parameter argument, PropertyInfo property);
        internal abstract object ApplyFormat(KeyValuePair<string, string> parameter, PropertyInfo property);
    }
}
