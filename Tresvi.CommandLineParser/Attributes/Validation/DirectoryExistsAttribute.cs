using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DirectoryExistsAttribute : ValidationAttributeBase
    {

        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string directory = parameter.Value.Trim();
            if (!directory.EndsWith(Path.DirectorySeparatorChar.ToString())) directory += Path.DirectorySeparatorChar;

            if (!Directory.Exists(directory))
                throw new DirectoryNotExistsException($"El directorio especificado en el parametro {parameter.Key}={directory} no existe");

            return true;
        }
    }
}
