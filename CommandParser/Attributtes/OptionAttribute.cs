using CommandParser.DecoratorAttributes;
using CommandParser.DecoratorAttributes.DecoratorFormatterAttributes;
using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParser.Attributtes
{

    //[Option("i", "input", Required = true, HelpText = "Input file to read.")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute: BaseArgumentAttribute
    {

        public string DefaultValue { get; set; }


        public OptionAttribute(string keyword, string shortKeyword, bool isRequired, string defaultValue = "", string helpText = "") 
            : base(keyword, shortKeyword, isRequired, helpText)
        {
            DefaultValue = defaultValue;
        }


        internal override void ParseAndAssign(PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments)
        {
            Argument argument = DetectKeyword(CLI_Arguments);
           
            if (argument.NotFound)
            {
                if (this.IsRequired)
                    throw new RequiredParameterNotFoundException($"El parametro requerido {this.Keyword}/{this.ShortKeyword} no fue definido en la linea de comando");
                else
                    return;
            }

            object value = argument.Value;

            foreach (Attribute attrib in property.GetCustomAttributes())
            {
                if (attrib is DecoratorCheckAttributeBase checkAttrib)              //Aplica atributos que solo chequean el dato
                    checkAttrib.Check(argument, property);
                else if (attrib is DecoratorFormatterAttributeBase formatterAttrib) //Aplica atributos que modifican el modifican (lo formatean)
                    value = formatterAttrib.ApplyFormat(argument, property);
            }

            ControlCLI_Arguments.Remove(argument.Name);
            ControlCLI_Arguments.Remove(argument.Value);

            if (value != null) property.SetValue(targetObject, value);
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

            if (matchCounter > 1) throw new RepeatedArgumentException($"El argumento {this.Keyword}/{this.ShortKeyword} fue definido mas de una vez");
            
            //Detecta si falta el valor de un ultimo parametro
            if (CLI_Arguments.Count < keyword.Index + 2) throw new ValueNotSpecifiedException($"El valor del argumento {keyword.Name} no fue especificado");

            //Detecta si falta el valor de un parametro que no es el ultimo
            keyword.Value = CLI_Arguments[keyword.Index + 1];
            if (keyword.Value.StartsWith("--") || keyword.Value.StartsWith("-"))
                throw new ValueNotSpecifiedException($"El valor del argumento {keyword.Name} no fue especifica");

            return keyword;
        }
    }
}
