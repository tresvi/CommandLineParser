using CommandParser.Attributtes;
using CommandParser.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.DecoratorAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DirectoryExistsAttribute : DecoratorCheckAttributeBase
    {
      
        internal override bool Check(Argument argument, PropertyInfo property)
        {
            string directory = argument.Value.Trim();
            if (!directory.EndsWith(Path.DirectorySeparatorChar.ToString())) directory += Path.DirectorySeparatorChar;

            if (!Directory.Exists(directory))
                throw new DirectoryNotExistsException($"El directorio requerido especificado en el parametro {argument.Name}={directory} no existe");

            return true;
        }
    }
}
