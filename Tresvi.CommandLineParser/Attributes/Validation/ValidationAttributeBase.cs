using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Attributes.Validation
{
    public abstract class ValidationAttributeBase : Attribute
    {
        internal abstract bool Check(KeyValuePair<string, string> parameter, PropertyInfo property);
    }
}
