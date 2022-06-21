using CommandParser.Attributtes;
using System;
using System.Reflection;

namespace CommandParser.DecoratorAttributes.DecoratorFormatterAttributes
{
    //No se podra definir mas de un atributo de formato por property.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class DecoratorFormatterAttributeBase : Attribute
    {
        public string Format { get; set; }

        public DecoratorFormatterAttributeBase(string format)
        {
            this.Format = format;
        }

        internal abstract object ApplyFormat(Argument argument, PropertyInfo property);
    }
}
