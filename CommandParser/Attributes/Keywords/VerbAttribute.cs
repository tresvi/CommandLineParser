﻿using System;

namespace CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class VerbAttribute : Attribute
    {
        public string Name { get; set; }
        public string HelpText { get; set; }
        //public bool IsDefault { get; set; }


        public VerbAttribute(string name, string helpText = "" /*, bool isDefault = false*/)
        {
            this.Name = name;
            this.HelpText = helpText;
            //this.IsDefault = isDefault;
        }


    }
}
