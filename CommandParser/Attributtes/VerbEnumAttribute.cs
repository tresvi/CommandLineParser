using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Attributtes
{

    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public class VerbEnumAttribute: Attribute
    {
    }
}
