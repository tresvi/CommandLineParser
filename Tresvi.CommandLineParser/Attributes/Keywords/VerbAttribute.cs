using System;

namespace Tresvi.CommandParser.Attributtes.Keywords
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class VerbAttribute : Attribute
    {
        internal string Name { get; set; }
        internal string HelpText { get; set; }
        internal bool IsDefault { get; set; }

        public VerbAttribute(string name, string helpText = "", bool isDefault = false)
        {
            this.Name = name;
            this.HelpText = helpText;
            this.IsDefault = isDefault;
        }
    }
}
