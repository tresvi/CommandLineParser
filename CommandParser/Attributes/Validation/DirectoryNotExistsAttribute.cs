using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DirectoryNotExistsAttribute : ValidationAttributeBase
    {
        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string directory = parameter.Value.Trim();
            if (Directory.Exists(directory))
                throw new DirectoryAlreadyExistsException($"El directorio especificado en el parametro {parameter.Key}={directory} ya existe");

            return true;
        }
    }
}
