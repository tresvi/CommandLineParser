using Tresvi.CommandParser.Attributtes;
using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Formatter
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DateTimeFormatterAttribute : FormatterBaseAttribute
    {
        public DateTimeFormatterAttribute(string format) : base(format) { }

        internal override object ApplyFormat(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            if (property.PropertyType != typeof(DateTime))
                throw new InvalidadPropertyTypeException($"La propiedad de asignacion {property.Name} no es del tipo DateTime");

            if (DateTime.TryParseExact(parameter.Value.Trim(), this.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                return fecha;
            else
                throw new InvalidFormatException($"El valor del argumento {parameter.Key} no corresponde al formato de fecha especificado: {this.Format}, " +
                    $"o bien el valor del dato proporcionado \"{parameter.Value}\" es inválido como fecha");
        }
    }

}
