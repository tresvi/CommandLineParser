using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    public abstract class ValidationAttributeBase : Attribute
    {
       //!! internal abstract bool Check(Parameter argument, PropertyInfo property);

        internal abstract bool Check(KeyValuePair<string, string> parameter, PropertyInfo property);
    }
}
