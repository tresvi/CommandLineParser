using CommandParser.Attributtes;
using CommandParser.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.DecoratorAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DirectoryNotExistsAttribute : DecoratorCheckAttributeBase
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
