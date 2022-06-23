using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    public abstract class ValidationAttributeBase : Attribute
    {
        internal abstract bool Check(KeyValuePair<string, string> parameter, PropertyInfo property);
    }
}
