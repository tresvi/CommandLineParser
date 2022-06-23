using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes.Keywords
{
    internal struct Parameter
    {
        public string Name;
        public string Value;
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseArgumentAttribute : Attribute
    {
        public string Keyword { get; set; }
        public string ShortKeyword { get; set; }
        public string HelpText { get; set; }
        public bool IsRequired { get; set; }


        public BaseArgumentAttribute(string keyword, string shortKeyword, bool isRequired, string helpText = "")
        {
            Keyword = "--" + keyword;
            ShortKeyword = "-" + shortKeyword;
            HelpText = helpText;
            IsRequired = isRequired;
        }

        //  internal abstract Parameter DetectKeyword(List<string> CLI_Arguments);
        //internal abstract void ParseAndAssign(int keywordIndex, PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments);
        internal abstract void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments);
    }
}
