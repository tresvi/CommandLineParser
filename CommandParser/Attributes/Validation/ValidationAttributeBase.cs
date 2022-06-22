using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using System;
using System.Reflection;

namespace CommandParser.Attributes.Validation
{
    public abstract class ValidationAttributeBase : Attribute
    {
        internal abstract bool Check(Argument argument, PropertyInfo property);
    }
}
