using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using System;
using System.Globalization;
using System.Reflection;

namespace CommandParser.Attributes.Formatter
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DateTimeFormatterAttribute : DecoratorFormatterAttributeBase
    {
        public DateTimeFormatterAttribute(string format) : base(format) { }

        internal override object ApplyFormat(Argument argument, PropertyInfo property)
        {
            if (property.PropertyType != typeof(DateTime))
                throw new InvalidadPropertyTypeException($"La propiedad de asignacion {property.Name} no es del tipo DateTime");

            if (DateTime.TryParseExact(argument.Value.Trim(), this.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                return fecha;
            else
                throw new InvalidFormatException($"El valor del argumento {argument.Name} no corresponde al formato de fecha especificado: {this.Format}, " +
                    $"o bien el valor del dato proporcionado \"{argument.Value}\" es inválido como fecha");
        }
    }

}
