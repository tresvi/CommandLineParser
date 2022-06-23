using CommandParser.Attributtes;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using CommandParser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommandParser
{
    //TODO: Arreglar metodo Help para que soporte multiclase.
    //TODO: Agregar un metodo de validacion de estrucura de las clases. Que no haya nombres ni nombres cortos repetidos dentro de una misma clase.
    //TODO: Agregar un metodo de validacion de clases verbo. que ambas no repitan el mismo verbo.
    //TODO: Analizar la posibilidad de definir un verbo por default (que se tome en caso de que no se escriba nada) para futuras versiones.
    public static class CommandLine
    {

        public static T Parse<T>(string[] args) where T : new()
        {
            T targetObject = new T();
            List<string> CLI_Arguments = new List<string>(args);

            if (IsHelpRequested(CLI_Arguments))
            {
                PrintHelp(targetObject);
                return default(T);
            }
           
            List<string> keywordsFound = new List<string>();
            BaseArgumentAttribute attribute;
            PropertyInfo property;

            while (CLI_Arguments.Count != 0)
            {
                string searchedKeyword = CLI_Arguments[0];
                attribute = FindMatchKeywordVsAttribute(searchedKeyword, targetObject, out property);
                attribute.ParseAndAssign(property, targetObject, ref CLI_Arguments);
                keywordsFound.Add(searchedKeyword);
            }

            CheckRequiredOptions(keywordsFound, targetObject);
            InitializeFlagsInFalse(targetObject);
            return targetObject;
        }


        private static BaseArgumentAttribute FindMatchKeywordVsAttribute(string searchedKeyword, object targetObject, out PropertyInfo propertyOut)
        {
            BaseArgumentAttribute foundAttribute = null;
            propertyOut = null;
            int matchCounter = 0;

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (BaseArgumentAttribute attribute in property.GetCustomAttributes(typeof(BaseArgumentAttribute), true))
                {
                    if (attribute.Keyword == searchedKeyword || attribute.ShortKeyword == searchedKeyword)
                    {
                        foundAttribute = attribute;
                        propertyOut = property;
                        matchCounter++;
                    }
                }
            }

            if (matchCounter == 0)
                throw new UnknownParameterException($"Parámetro desconocido: \"{searchedKeyword}\"");

            if (matchCounter > 1)
                 throw new RepeatedParameterDefinitionException($"El parámetro \"{searchedKeyword}\" fue definido en mas de una Option en la clase \"{targetObject.GetType().Name}\"");

            return foundAttribute;
        }


        private static void CheckRequiredOptions(List<string> keywordsFound, object targetObject)
        {
            List<BaseArgumentAttribute> requiredAttributes = new List<BaseArgumentAttribute>();

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (OptionAttribute attribute in property.GetCustomAttributes(typeof(OptionAttribute), true))
                {
                    if (attribute.IsRequired) requiredAttributes.Add(attribute);
                }
            }

            foreach (BaseArgumentAttribute attribute in requiredAttributes)
            {
                if (!keywordsFound.Contains(attribute.Keyword) && !keywordsFound.Contains(attribute.ShortKeyword))
                    throw new RequiredParameterNotFoundException($"No se encontró definicion para el parámetro requerido: \"{attribute.Keyword} / {attribute.ShortKeyword}\"");
            }
        }

        private static void InitializeFlagsInFalse(object targetObject)
        {
            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (FlagAttribute attribute in property.GetCustomAttributes(typeof(FlagAttribute), true))
                {
                    attribute.Clear(targetObject, property);
                }
            }
        }

        public static object Parse<T1, T2>(string[] args)
            where T1 : new()
            where T2 : new()
        {
            List<string> verbsAvailable = VerbHelper.ValidateVerbDecoration<T1, T2>();
            //List<string> defaultVerbs = VerbHelper.DetectDefaultVerbs<T1, T2>();


            //if (defaultVerbs.Count > 1)
            //    throw new ToManyDefaultVerbsException($"Se han especificado mas de un verbo como Default, solo puede haber 1: Verbos Default: {string.Join( " | ", defaultVerbs) }");

            //TODO: UseFirstClassAsDefault, o metodo que pregunta Clase Default
            //Retorno de valor por default si fue definido
            //if (args.Length == 0)
            //{
            //    if (defaultVerbs.Count == 0)
            //        throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos");
            //    else
            //    {
            //        //TODO: Recorrer cada TX en busca del primer default y llamo a Parse con su tipo.

            //    } 
            //}

            //if (args.Length == 0) throw new NotDefaultVerbException("No especificó ningun verbo como Default");

            string searchedVerb = args[0];

            if (searchedVerb.Trim().StartsWith("--") || searchedVerb.Trim().StartsWith("-"))
                throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido. Debe especificar alguno de los siguientes: {string.Join(" | ", verbsAvailable)}");

            List<string> argsList = args.ToList();
            argsList.RemoveAt(0);
            args = argsList.ToArray();

            if (VerbHelper.CheckIfVerbIsInClass<T1>(searchedVerb))
                return Parse<T1>(args);
            else if (VerbHelper.CheckIfVerbIsInClass<T2>(searchedVerb))
                return Parse<T2>(args);
            else
                throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido. Debe especificar alguno de los siguientes: {string.Join(" | ", verbsAvailable)}");
        }


        private static bool IsHelpRequested(List<string> CLI_Arguments)
        {
            if (CLI_Arguments.IndexOf("-h") != -1
                || CLI_Arguments.IndexOf("--help") != -1
                || CLI_Arguments.IndexOf("/?") != -1
                || CLI_Arguments.IndexOf("-help") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static void PrintHelp(object targetObject)
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
