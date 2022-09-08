using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using CommandParser.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommandParser
{
    //TODO: Agregar el RangeValidationAttribute.
    //TODO: Agregar el StringListValidationAttribute.
    //TODO: Arreglar metodo Help para que incluya informacion de Verbos.
    //TODO: Agregar un metodo de validacion de clases verbo. que ambas no repitan el mismo verbo.
    //TODO: Analizar la posibilidad de definir un verbo por default (que se tome en caso de que no se escriba nada) para futuras versiones.
    //TODO: Reconocer enumeraciones por su nombre.
    public static class CommandLine
    {
        private const string HELP_TEXT = "--help | -h | /?\t\tDespliega la ayuda detallando las funciones de los parametros.";

        /// <summary>
        /// Analiza los argumentos para completar la clase requerida
        /// </summary>
        /// <typeparam name="T">Tipo de Clase a rellenar en base a los argumentos recibidos como parametros</typeparam>
        /// <param name="args">Argumentos obtenidos de la linea de comando</param>
        /// <returns></returns>
        public static T Parse<T>(string[] args) where T : new()
        {
            T targetObject = new T();
            List<string> CLI_Arguments = new List<string>(args);

            if (IsHelpRequested(CLI_Arguments))
            {
                PrintHelp(targetObject);
                Environment.Exit(0);
            }

            CheckForDuplicatedKeywordInClass<T>();
            InitializeFlagsInFalse(targetObject);

            List<string> keywordsAlreadyFound = new List<string>();
            BaseArgumentAttribute attribute;
            PropertyInfo property;

            while (CLI_Arguments.Count != 0)
            {
                string searchedKeyword = CLI_Arguments[0];
                if (keywordsAlreadyFound.Contains(searchedKeyword))
                    throw new MultiInvocationParameterException($"El parametro {searchedKeyword} ya fue especificado en la linea de comando");
                attribute = FindMatchKeywordVsAttribute(searchedKeyword, targetObject, out property);
                attribute.ParseAndAssign(property, targetObject, ref CLI_Arguments);
                keywordsAlreadyFound.Add(searchedKeyword);
            }

            CheckRequiredOptions(keywordsAlreadyFound, targetObject);
            return targetObject;
        }

        /// <summary>
        /// Busca el attributo correspondiente a la palabra clave correspondiente. Valida que haya 1 y solo 1 definicion en la Clase de destino
        /// </summary>
        /// <param name="searchedKeyword"></param>
        /// <param name="targetObject"></param>
        /// <param name="propertyOut"></param>
        /// <returns></returns>
        /// <exception cref="UnknownParameterException"></exception>
        /// <exception cref="MultiDefinitionParameterException"></exception>
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
                        matchCounter++;
                        foundAttribute = attribute;
                        propertyOut = property;

                    }
                }
            }

            if (matchCounter == 0)
                throw new UnknownParameterException($"Parámetro desconocido: \"{searchedKeyword}\"");
            else if (matchCounter > 1)
                throw new MultiDefinitionParameterException($"La Keyword \"{searchedKeyword}\" ya se mapeó con la property \"{propertyOut?.Name}\" " +
                    $"de la clase \"{targetObject.GetType().Name}\". No se puede mapear la misma palabra en mas de una property de la misma clase.");

            return foundAttribute;
        }


        /// <summary>
        /// Revisa que todas las Option marcadas como requeridas en la clase de destino, haya sido completadas
        /// </summary>
        /// <param name="keywordsFound"></param>
        /// <param name="targetObject"></param>
        /// <exception cref="RequiredParameterNotFoundException"></exception>
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


        /// <summary>
        /// Revisa que no haya definiciones duplicadas de keywords y shortkeywords dentro de la clase de destino
        /// </summary>
        /// <param name="keywordsFound"></param>
        /// <param name="targetObject"></param>
        /// <exception cref="RequiredParameterNotFoundException"></exception>
        private static void CheckForDuplicatedKeywordInClass<T>() where T : new()
        {
            T targetObject = new T();
            Dictionary<string, string> shortKeywordsProperties = new Dictionary<string, string>();
            Dictionary<string, string> keywordsProperties = new Dictionary<string, string>();

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (OptionAttribute attribute in property.GetCustomAttributes(typeof(OptionAttribute), true))
                {
                    if (shortKeywordsProperties.ContainsKey(attribute.ShortKeyword))
                        throw new MultiDefinitionParameterException($"La ShortKeyword \"{attribute.ShortKeyword}\" se mapeó con la property \"{property.Name}\" y también con la property \"{shortKeywordsProperties[attribute.ShortKeyword]}\" " +
                            $"de la clase \"{targetObject.GetType().Name}\". No se puede mapear la misma palabra en mas de una property de la misma clase.");

                    shortKeywordsProperties.Add(attribute.ShortKeyword, property.Name);

                    if (keywordsProperties.ContainsKey(attribute.Keyword))
                        throw new MultiDefinitionParameterException($"La Keyword \"{attribute.Keyword}\" se mapeó con la property \"{property.Name}\" y también con la property \"{keywordsProperties[attribute.Keyword]}\" " +
                            $"de la clase \"{targetObject.GetType().Name}\". No se puede mapear la misma palabra en mas de una property de la misma clase.");

                    keywordsProperties.Add(attribute.Keyword, property.Name);
                }
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


        /// <summary>
        /// Analiza los argumentos para completar la clase que corresponda segun el Verbo solicitado
        /// </summary>
        /// <typeparam name="T1">Clase que representa al Verbo 1 a rellenar con los parametros si su verbo es el solicitado</typeparam>
        /// <typeparam name="T2">Clase Verbo 2</typeparam>
        /// <param name="args">Argumentos obtenidos de la linea de comando</param>
        /// <returns></returns>
        /// <exception cref="UnknownVerbException"></exception>
        public static object Parse<T1, T2>(string[] args)
            where T1 : new()
            where T2 : new()
        {
            throw new NotImplementedException();
            /*
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

            List<string> argsList = new List<string>(args);
            argsList.RemoveAt(0);
            args = argsList.ToArray();

            if (VerbHelper.CheckIfVerbIsInClass<T1>(searchedVerb))
                return Parse<T1>(args);
            else if (VerbHelper.CheckIfVerbIsInClass<T2>(searchedVerb))
                return Parse<T2>(args);
            else
                throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido. Debe especificar alguno de los siguientes: {string.Join(" | ", verbsAvailable)}");
            */
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
            sb.AppendLine(HELP_TEXT);
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
