using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tresvi.CommandParser
{
    //TODO: Agregar un attribute nivel clase para documentacion extra en el helptext. Prto ejemplo explicacion que que hace el programa.
    //TODO: Agregar el RangeValidationAttribute.
    //TODO: Agregar un metodo de validacion de clases verbo. que ambas no repitan el mismo verbo. NO, SE DESCARTA
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
                Console.WriteLine(GetHelpText(targetObject));
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
                attribute = FindMatchKeywordVsAttribute(searchedKeyword, targetObject, out property);
                
                // Verificar si cualquiera de las keywords del atributo (larga o corta) ya fue procesada
                if (keywordsAlreadyFound.Contains(attribute.Keyword) || keywordsAlreadyFound.Contains(attribute.ShortKeyword))
                {
                    string alreadyUsedKeyword = keywordsAlreadyFound.Contains(attribute.Keyword) ? attribute.Keyword : attribute.ShortKeyword;
                    throw new MultiInvocationParameterException($"El parametro {searchedKeyword} (equivalente a {alreadyUsedKeyword}) ya fue especificado en la linea de comando");
                }
                
                attribute.ParseAndAssign(property, targetObject, ref CLI_Arguments);
                
                // Agregar ambas keywords del atributo a la lista para prevenir uso futuro de cualquiera de ellas
                keywordsAlreadyFound.Add(attribute.Keyword);
                keywordsAlreadyFound.Add(attribute.ShortKeyword);
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
        /// <exception cref="MultiDefinitionParameterException"></exception>
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
        /// Analiza los argumentos para completar la clase que corresponda según el Verbo solicitado
        /// entre las clases especificadas.
        /// </summary>
        /// <param name="args">Argumentos obtenidos de la linea de comando</param>
        /// <param name="verbTypes">Conjunto de tipos que representan clases Verbo decoradas con <see cref="VerbAttribute"/>.</param>
        /// <returns>Instancia de la clase Verbo correspondiente al verbo detectado en la línea de comandos.</returns>
        /// <exception cref="NotDefaultVerbException">Si no se especifica ningún verbo en la línea de comandos.</exception>
        /// <exception cref="NotVerbClassException">Si alguna de las clases indicadas no está decorada con <see cref="VerbAttribute"/>.</exception>
        /// <exception cref="UnknownVerbException">Si el verbo indicado no coincide con ninguno de los verbos disponibles.</exception>
        public static object Parse(string[] args, params Type[] verbTypes)
        {
            if (verbTypes == null || verbTypes.Length == 0)
                throw new ArgumentException("Debe especificar al menos una clase verbo.", nameof(verbTypes));

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length > 0 && IsHelpRequested(new List<string>(args)))
            {
                Console.WriteLine(GetHelpTextForVerbs(verbTypes));
                Environment.Exit(0);
            }

            if (args.Length == 0)
                throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos");

            string searchedVerb = args[0];

            if (string.IsNullOrWhiteSpace(searchedVerb))
                throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos");

            if (searchedVerb.Trim().StartsWith("--") || searchedVerb.Trim().StartsWith("-"))
                throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido.");

            // Recorremos los tipos candidatos buscando el que tenga el VerbAttribute con el nombre adecuado
            List<string> verbsAvailable = new List<string>();
            Type targetVerbType = null;

            foreach (Type verbType in verbTypes)
            {
                if (verbType == null) continue;

                Attribute verbAttributeRaw = verbType.GetCustomAttribute(typeof(VerbAttribute));
                if (verbAttributeRaw == null)
                    throw new NotVerbClassException($"La clase {verbType.Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");

                VerbAttribute verbAttribute = (VerbAttribute)verbAttributeRaw;
                verbsAvailable.Add(verbAttribute.Name);

                if (verbAttribute.Name == searchedVerb)
                    targetVerbType = verbType;
            }

            if (targetVerbType == null)
                throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido. Debe especificar alguno de los siguientes: {string.Join(" | ", verbsAvailable)}");

            // Elimino el verbo de la lista de argumentos
            List<string> argsList = new List<string>(args);
            argsList.RemoveAt(0);
            string[] remainingArgs = argsList.ToArray();

            // Reutilizo el Parse<T>(string[] args) existente vía reflexión para no duplicar lógica.
            MethodInfo genericParseMethod = null;
            foreach (MethodInfo method in typeof(CommandLine).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.Name == nameof(Parse)
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 1
                    && method.GetParameters().Length == 1
                    && method.GetParameters()[0].ParameterType == typeof(string[]))
                {
                    genericParseMethod = method;
                    break;
                }
            }
            if (genericParseMethod == null)
                throw new InvalidOperationException("No se encontró el método genérico Parse<T>(string[] args).");

            MethodInfo concreteParseMethod = genericParseMethod.MakeGenericMethod(targetVerbType);

            try
            {
                return concreteParseMethod.Invoke(null, new object[] { remainingArgs });
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                // Propaga la excepción real lanzada dentro de Parse<T>
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Analiza los argumentos para completar la clase que corresponda segun el Verbo solicitado,
        /// considerando dos posibles clases Verbo.
        /// </summary>
        /// <typeparam name="T1">Clase que representa al Verbo 1 a rellenar con los parametros si su verbo es el solicitado</typeparam>
        /// <typeparam name="T2">Clase Verbo 2</typeparam>
        /// <param name="args">Argumentos obtenidos de la linea de comando</param>
        /// <returns>Instancia de la clase Verbo correspondiente.</returns>
        public static object Parse<T1, T2>(string[] args)
            where T1 : new()
            where T2 : new()
        {
            return Parse(args, typeof(T1), typeof(T2));
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


        /// <summary>
        /// Genera el texto de ayuda para una clase de parámetros sin verbos.
        /// </summary>
        /// <param name="targetObject">Instancia de la clase de parámetros.</param>
        /// <returns>String con el texto de ayuda formateado.</returns>
        public static string GetHelpText(object targetObject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Comandos:");

            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    if (attribute is BaseArgumentAttribute argument)
                    {
                        sb.AppendLine($"{argument.Keyword} | {argument.ShortKeyword}\t\t{argument.HelpText}");
                    }
                }
            }
            sb.AppendLine(HELP_TEXT);
            return sb.ToString();
        }



        /// <summary>
        /// Genera el texto de ayuda para todos los verbos disponibles con sus parámetros.
        /// </summary>
        /// <param name="verbTypes">Tipos de clases verbo para los cuales generar la ayuda.</param>
        /// <returns>String con el texto de ayuda formateado.</returns>
        public static string GetHelpTextForVerbs(params Type[] verbTypes)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Verbos disponibles:");
            sb.AppendLine();

            foreach (Type verbType in verbTypes)
            {
                if (verbType == null) continue;

                Attribute verbAttributeRaw = verbType.GetCustomAttribute(typeof(VerbAttribute));
                if (verbAttributeRaw == null) continue;

                VerbAttribute verbAttribute = (VerbAttribute)verbAttributeRaw;

                // Mostrar nombre del verbo y su helptext
                sb.AppendLine($"  {verbAttribute.Name,-12} {verbAttribute.HelpText}");

                // Mostrar parámetros del verbo
                foreach (PropertyInfo property in verbType.GetProperties())
                {
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        if (attribute is BaseArgumentAttribute argument)
                        {
                            string requiredText = "";
                            if (argument is OptionAttribute option && option.IsRequired)
                            {
                                requiredText = "(Requerido) ";
                            }

                            sb.AppendLine($"    {argument.Keyword} | {argument.ShortKeyword,-3}\t{requiredText}{argument.HelpText}");
                        }
                    }
                }

                sb.AppendLine();
            }

            sb.AppendLine(HELP_TEXT);
            return sb.ToString();
        }




        /// <summary>
        /// Obtiene la lista de todos los parámetros definidos en una clase de parámetros.
        /// </summary>
        /// <typeparam name="T">Tipo de la clase que contiene las propiedades decoradas con atributos de argumentos (OptionAttribute, FlagAttribute, etc.).</typeparam>
        /// <returns>Lista de strings que contiene todos los keywords (nombres largos y cortos) de los parámetros definidos en la clase.</returns>
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
