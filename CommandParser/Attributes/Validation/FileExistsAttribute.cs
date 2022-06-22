using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileExistsAttribute : ValidationAttributeBase
    {
        internal override bool Check(Argument argument, PropertyInfo property)
        {
            string filePath = argument.Value.Trim();
            if (!File.Exists(filePath))
                throw new FileNotExistsException($"El archivo especificado en el parametro {argument.Name}={filePath} no fue hallado en la ruta especificada");

            return true;
        }

    }
}
