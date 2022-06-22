using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DirectoryNotExistsAttribute : ValidationAttributeBase
    {
        internal override bool Check(Argument argument, PropertyInfo property)
        {
            string directory = argument.Value.Trim();
            if (Directory.Exists(directory))
                throw new DirectoryAlreadyExistsException($"El directorio especificado en el parametro {argument.Name}={directory} ya existe");

            return true;
        }
    }
}
