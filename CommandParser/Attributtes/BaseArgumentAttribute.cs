using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes
{
    internal struct Argument
    {
        public string Name;
        public int Index;
        public string Value;
        public bool NotFound;
    }

    public abstract class BaseArgumentAttribute: Attribute
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

        internal abstract Argument DetectKeyword(List<string> CLI_Arguments);
        internal abstract void ParseAndAssign(PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments);
    }
}
