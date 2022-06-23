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
    public class FileNotExistsAttribute : ValidationAttributeBase
    {
        internal override bool Check(KeyValuePair<string, string> parameter, PropertyInfo property)
        {
            string filePath = parameter.Value.Trim();
            if (File.Exists(filePath))
                throw new FileAlreadyExistsException($"El archivo especificado en el parametro {parameter.Key}={filePath} ya existe");

            return true;
        }

    }
}
