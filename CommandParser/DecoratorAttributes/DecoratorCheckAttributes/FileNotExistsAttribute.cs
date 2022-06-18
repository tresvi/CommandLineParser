using CommandParser.Attributtes;
using CommandParser.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.DecoratorAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileNotExistsAttribute : DecoratorCheckAttributeBase
    {
        internal override bool Check(Argument argument, PropertyInfo property)
        {
            string filePath = argument.Value.Trim();
            if (File.Exists(filePath))
                throw new FileAlreadyExistsException($"El archivo especificado en el parametro {argument.Name}={filePath} ya existe");
            
            return true;
        }
    }
}
