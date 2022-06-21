using CommandParser.Attributtes;
using System;
using System.Reflection;

namespace CommandParser.DecoratorAttributes
{
    public abstract class DecoratorCheckAttributeBase : Attribute
    {
        internal abstract bool Check(Argument argument, PropertyInfo property);
    }
}
