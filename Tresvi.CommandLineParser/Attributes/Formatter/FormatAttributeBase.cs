using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Formatter
{
    //No se podra definir mas de un atributo de formato por property.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class FormatterBaseAttribute : Attribute
    {
        public string Format { get; set; }

        protected FormatterBaseAttribute(string format)
        {
            this.Format = format;
        }

        internal abstract object ApplyFormat(KeyValuePair<string, string> parameter, PropertyInfo property);
    }
}
