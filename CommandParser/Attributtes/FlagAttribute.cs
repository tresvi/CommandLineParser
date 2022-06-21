﻿using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FlagAttribute : BaseArgumentAttribute
    {
        public bool DefaultValue { get; set; }

        public FlagAttribute(string keyword, string shortKeyword, bool defaultValue = false, string helpText = "")
            : base(keyword, shortKeyword, false, helpText)
        {
            this.DefaultValue = defaultValue;
        }

        internal override void ParseAndAssign(PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments)
        {
            if (property.PropertyType != typeof(bool)) throw new WrongPropertyTypeException($"La propiedad de asignacion de un argumento flag ({property.Name}), debe ser de tipo booleano");

            Argument argument = DetectKeyword(CLI_Arguments);

            if (argument.NotFound)
                property.SetValue(targetObject, false);
            else
                property.SetValue(targetObject, true);

            ControlCLI_Arguments.Remove(argument.Name);
        }


        internal override Argument DetectKeyword(List<string> CLI_Arguments)
        {
            Argument keyword = new Argument() { NotFound = true };
            int matchCounter = 0;

            for (int i = 0; i < CLI_Arguments.Count; i++)
            {
                if (CLI_Arguments[i] == this.Keyword || CLI_Arguments[i] == this.ShortKeyword)
                {
                    keyword.Name = CLI_Arguments[i];
                    keyword.Index = i;
                    keyword.NotFound = false;
                    matchCounter++;
                }
            }

            if (keyword.NotFound)
            {
                keyword.Name = this.Keyword;
                return keyword;
            }

            if (matchCounter > 1) throw new RepeatedArgumentException($"El flag {this.Keyword}/{this.ShortKeyword} fue definido mas de una vez");

            return keyword;
        }

    }
}