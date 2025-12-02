using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tresvi.CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseArgumentAttribute : Attribute
    {
        internal string Keyword { get; set; }
        internal string ShortKeyword { get; set; }
        internal string HelpText { get; set; }


        protected BaseArgumentAttribute(string keyword, char shortKeyword, string helpText = "")
        {
            Keyword = "--" + keyword;
            ShortKeyword = "-" + shortKeyword.ToString();
            HelpText = helpText;
        }

       internal abstract void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments);
    }
}
