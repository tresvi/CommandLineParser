using CommandParser.Attributtes;
using CommandParser.Exceptions;
using CommandParser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommandParser
{
    public static class CommandLine
    {

        public static T Parse<T>(string[] args) where T : new()
        {
            T targetObject = new T();
            List<string> CLI_Arguments = new List<string>(args);
            
            if (IsHelpRequested(CLI_Arguments))
            {
                PrintHelp(CLI_Arguments, targetObject);
                return default(T);
            }

            List<string> ControlCLI_Arguments = new List<string>(CLI_Arguments);

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (BaseArgumentAttribute attribute in property.GetCustomAttributes(typeof(BaseArgumentAttribute), true))
                {
                    attribute.ParseAndAssign(property, targetObject, CLI_Arguments, ref ControlCLI_Arguments);
                }
            }

            if (ControlCLI_Arguments.Count > 0)
                throw new ArgumentException($"Se proporcionaron parametros desconocidos: {string.Join(" & ", ControlCLI_Arguments)}");

            return targetObject;
        }


        public static object Parse<T1, T2>(string[] args) 
            where T1 : new()
            where T2 : new()
        {
            VerbHelper.ValidateVerbDecoration<T1, T2>();
            object defaultVerb = VerbHelper.DetectDefaultVerb<T1, T2>();


            //TODO: UseFirstClassAsDefault, o metodo que pregunta Clase Default
            //Retorno dfe valor por default si fue definido
            /*if (args.Length == 0)
            {
                if (defaultVerb == null)
                    throw new NotDefaultVerbException("No especificó ningun verbo como Default");
                else
                    return Parse<defaultVerb>(args); ;
            }
            */
            if (args.Length == 0)
                throw new NotDefaultVerbException("No especificó ningun verbo como Default");

            string searchedVerb = args[0];

            //Tambien corresponde a la funcion de Verbo por default
            //if (searchedVerb.Trim().StartsWith("--") || searchedVerb.Trim().StartsWith("-"))
            //    throw new NotDefaultVerbException("No especificó ningun verbo como Default");

            List<string> argsList = args.ToList();
            argsList.RemoveAt(0);
            args = argsList.ToArray();

            if (VerbHelper.CheckIfVerbIsInClass<T1>(searchedVerb))
                return Parse<T1>(args);
            else if (VerbHelper.CheckIfVerbIsInClass<T2>(searchedVerb))
                return Parse<T2>(args);
            else
                throw new UnknownVerbException($"No se encontró clase que implemente el verbo {searchedVerb}");
        }


        private static bool IsHelpRequested(List<string> CLI_Arguments)
        {
            if (   CLI_Arguments.IndexOf("-h") != -1 
                || CLI_Arguments.IndexOf("--help") != -1 
                || CLI_Arguments.IndexOf("/?") != -1
                || CLI_Arguments.IndexOf("-help") != -1 )
            {
                return true;
            }
            else
            { 
                return false;
            }
        }


        private static void PrintHelp(List<string> CLI_Arguments, object targetObject)
        {

            StringBuilder sb = new StringBuilder();
            string helpLine = "";
            sb.AppendLine("Comandos:");

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    if (attribute is BaseArgumentAttribute argument)
                    {
                        helpLine = $"{argument.Keyword} | {argument.ShortKeyword}\t\t{argument.HelpText}";
                        sb.AppendLine(helpLine);
                    }
                }
            }
            Console.WriteLine(sb.ToString());
        }


        public static List<string> GetDefinedParameters<T>() where T : new()
        {
            T targetObject = new T();
            List<String> argumentosDefinidos = new List<string>();

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    if (attribute is BaseArgumentAttribute argument)
                    {
                        argumentosDefinidos.Add(argument.Keyword);
                        argumentosDefinidos.Add(argument.ShortKeyword);
                    }
                }
            }
            return argumentosDefinidos;
        }

    }
}
