using CommandParser.Attributtes;
using System;
using System.IO;
using System.Reflection;

namespace CommandParser.DecoratorAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileExistsAttribute : DecoratorCheckAttributeBase
    {
        internal override bool Check(Argument argument, PropertyInfo property)
        {
            string filePath = argument.Value.Trim();
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo especificado en el parametro {argument.Name}={filePath} no fue hallado en la ruta especificada");
            
            return true;
        }

    }
}
