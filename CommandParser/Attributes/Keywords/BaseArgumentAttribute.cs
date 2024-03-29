﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseArgumentAttribute : Attribute
    {
        public string Keyword { get; set; }
        public string ShortKeyword { get; set; }
        public string HelpText { get; set; }


        protected BaseArgumentAttribute(string keyword, char shortKeyword, string helpText = "")
        {
            Keyword = "--" + keyword;
            ShortKeyword = "-" + shortKeyword.ToString();
            HelpText = helpText;
        }

       internal abstract void ParseAndAssign(PropertyInfo property, object targetObject, ref List<string> CLI_Arguments);
    }
}
