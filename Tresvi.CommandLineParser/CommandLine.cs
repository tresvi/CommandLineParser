using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Attributes.Validation;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tresvi.CommandParser
{
    //TODO: Agregar un attribute nivel clase para documentacion extra en el helptext. Por ejemplo explicacion que que hace el programa.
    //TODO: Agregar el RangeValidationAttribute.
    //TODO: Agregar un metodo de validacion de clases verbo. que ambas no repitan el mismo verbo. NO, SE DESCARTA
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
            CheckIncompatibleParameters(keywordsAlreadyFound, targetObject);
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
        /// Revisa que no haya parámetros incompatibles usados simultáneamente en la línea de comandos.
        /// </summary>
        /// <param name="keywordsFound">Lista de keywords que fueron encontradas en la línea de comandos.</param>
        /// <param name="targetObject">Objeto que contiene las propiedades con atributos de argumentos.</param>
        /// <exception cref="IncompatibleParametersException">Se lanza cuando se detectan parámetros incompatibles.</exception>
        private static void CheckIncompatibleParameters(List<string> keywordsFound, object targetObject)
        {
            // Crear un diccionario que mapea nombres de propiedades a sus PropertyInfo si fueron usadas
            Dictionary<string, PropertyInfo> usedProperties = new Dictionary<string, PropertyInfo>();

            // Recorrer todas las propiedades y encontrar cuáles fueron usadas (basándose en keywordsFound)
            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (BaseArgumentAttribute attribute in property.GetCustomAttributes(typeof(BaseArgumentAttribute), true))
                {
                    // Si alguna de las keywords (larga o corta) está en la lista de encontradas, la propiedad fue usada
                    if (keywordsFound.Contains(attribute.Keyword) || keywordsFound.Contains(attribute.ShortKeyword))
                    {
                        usedProperties[property.Name] = property;
                        break; // Solo necesitamos saber que fue usada, no importa cuántas veces
                    }
                }
            }

            // Para cada propiedad usada, verificar sus incompatibilidades
            foreach (var kvp in usedProperties)
            {
                PropertyInfo property = kvp.Value;

                // Buscar atributos IncompatibleWithAttribute en esta propiedad
                foreach (Attributes.Validation.IncompatibleWithAttribute incompatibleAttr in 
                    property.GetCustomAttributes(typeof(Attributes.Validation.IncompatibleWithAttribute), true))
                {
                    // Verificar cada propiedad incompatible
                    foreach (string incompatiblePropName in incompatibleAttr.IncompatiblePropertyNames)
                    {
                        // Si la propiedad incompatible también fue usada, lanzar excepción
                        if (usedProperties.ContainsKey(incompatiblePropName))
                        {
                            PropertyInfo incompatibleProperty = usedProperties[incompatiblePropName];
                            
                            // Obtener los keywords de ambas propiedades para el mensaje de error
                            string propertyKeyword = GetKeywordForProperty(property);
                            string incompatibleKeyword = GetKeywordForProperty(incompatibleProperty);
                            
                            throw new IncompatibleParametersException(
                                $"Los parámetros \"{propertyKeyword}\" y \"{incompatibleKeyword}\" no pueden usarse juntos.");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Obtiene el keyword largo de una propiedad para mostrar en mensajes de error.
        /// </summary>
        /// <param name="property">PropertyInfo de la propiedad de la cual obtener el keyword.</param>
        /// <returns>El keyword largo de la propiedad.</returns>
        private static string GetKeywordForProperty(PropertyInfo property)
        {
            foreach (BaseArgumentAttribute attribute in property.GetCustomAttributes(typeof(BaseArgumentAttribute), true))
            {
                // Retornar el keyword largo (siempre está disponible)
                return attribute.Keyword;
            }
            
            // Si por alguna razón no hay atributo (no debería pasar), usar el nombre de la propiedad
            return property.Name;
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
            // ============================================================
            // FASE 1: Validaciones iniciales
            // ============================================================
            
            // Validar que se hayan proporcionado tipos de verbos
            if (verbTypes == null || verbTypes.Length == 0)
                throw new ArgumentException("Debe especificar al menos una clase verbo.", nameof(verbTypes));

            // Validar que los argumentos no sean null
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            // ============================================================
            // FASE 2: Manejo de solicitud de ayuda
            // ============================================================
            // Si el usuario solicita ayuda (--help, -h, /?, etc.), mostrar la ayuda
            // y terminar la ejecución del programa
            if (args.Length > 0 && IsHelpRequested(new List<string>(args)))
            {
                Console.WriteLine(GetHelpTextForVerbs(verbTypes));
                Environment.Exit(0);
            }

            // ============================================================
            // FASE 3: Detección y validación del verbo por defecto
            // ============================================================
            // Recorremos todos los tipos de verbos para:
            // 1. Validar que estén correctamente decorados con VerbAttribute
            // 2. Detectar si hay un verbo marcado como por defecto
            // 3. Validar que solo haya un verbo por defecto (no puede haber múltiples)
            
            Type defaultVerbType = null;  // El tipo del verbo por defecto (si existe)
            List<string> verbsAvailable = new List<string>();  // Lista de verbos disponibles (para mensajes de error)
            List<Type> allVerbTypes = new List<Type>();  // Lista de todos los tipos de verbos válidos

            foreach (Type verbType in verbTypes)
            {
                // Saltar tipos null (por si acaso)
                if (verbType == null) continue;

                // Obtener el atributo VerbAttribute de la clase
                Attribute verbAttributeRaw = verbType.GetCustomAttribute(typeof(VerbAttribute));
                if (verbAttributeRaw == null)
                    throw new NotVerbClassException($"La clase {verbType.Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");

                // Convertir el atributo al tipo específico VerbAttribute
                VerbAttribute verbAttribute = (VerbAttribute)verbAttributeRaw;
                
                // Agregar el nombre del verbo a la lista de disponibles (para mensajes de error)
                verbsAvailable.Add(verbAttribute.Name);
                allVerbTypes.Add(verbType);

                // Si este verbo está marcado como por defecto
                if (verbAttribute.IsDefault)
                {
                    // Validar que no haya otro verbo por defecto ya detectado
                    if (defaultVerbType != null)
                        throw new TooManyDefaultVerbsException("Solo puede haber un verbo marcado como por defecto.");
                    
                    defaultVerbType = verbType;
                }
            }
            
            // ============================================================
            // FASE 4: Determinación del verbo a usar
            // ============================================================
            // Determinar qué verbo usar basándose en:
            // - Si no hay argumentos: usar el verbo por defecto (si existe)
            // - Si el primer argumento es un parámetro (-- o -): usar el verbo por defecto (si existe)
            // - Si el primer argumento es un nombre de verbo: buscar y usar ese verbo
            
            Type targetVerbType = null;  // El tipo del verbo que se usará finalmente
            string[] remainingArgs = args;  // Los argumentos que se pasarán al Parse<T> (pueden incluir o no el verbo)

            // Caso 1: No hay argumentos - usar verbo por defecto si existe
            if (args.Length == 0)
            {
                if (defaultVerbType == null)
                    throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos. No hay un verbo por defecto definido.");
                
                targetVerbType = defaultVerbType;
                remainingArgs = new string[0];  // No hay argumentos adicionales
            }
            else
            {
                string firstArg = args[0];

                // Caso 2: El primer argumento es un parámetro (-- o -) - usar verbo por defecto si existe
                // Esto permite invocar: miPrograma.exe --parametro valor (sin especificar el verbo)
                if (firstArg.Trim().StartsWith("--") || firstArg.Trim().StartsWith("-"))
                {
                    if (defaultVerbType == null)
                        throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos. No hay un verbo por defecto definido.");
                    
                    targetVerbType = defaultVerbType;
                    remainingArgs = args;  // Mantener todos los argumentos (incluyendo parámetros)
                }
                else
                {
                    // Caso 3: El primer argumento es un nombre de verbo - buscar ese verbo específico
                    string searchedVerb = firstArg;

                    // Validar que el verbo no esté vacío o sea solo espacios en blanco
                    if (string.IsNullOrWhiteSpace(searchedVerb))
                        throw new NotDefaultVerbException("Debe especificar un verbo en la linea de comandos");

                    // Buscar el verbo solicitado en la lista de verbos disponibles
                    foreach (Type verbType in allVerbTypes)
                    {
                        VerbAttribute verbAttribute = (VerbAttribute)verbType.GetCustomAttribute(typeof(VerbAttribute));
                        if (verbAttribute.Name == searchedVerb)
                        {
                            targetVerbType = verbType;
                            break;
                        }
                    }

                    // Si no se encontró el verbo solicitado, lanzar excepción
                    if (targetVerbType == null)
                        throw new UnknownVerbException($"{searchedVerb} no es un nombre de verbo valido. Debe especificar alguno de los siguientes: {string.Join(" | ", verbsAvailable)}");

                    // Eliminar el verbo de la lista de argumentos (ya que ya lo procesamos)
                    List<string> argsList = new List<string>(args);
                    argsList.RemoveAt(0);  // Eliminar el verbo (primer elemento)
                    remainingArgs = argsList.ToArray();  // Convertir a array para pasarlo al método Parse<T>
                }
            }

            // ============================================================
            // FASE 5: Preparación completada
            // ============================================================
            // En este punto ya tenemos:
            // - targetVerbType: el tipo del verbo a usar (ya sea por defecto o especificado)
            // - remainingArgs: los argumentos restantes (sin el verbo, si fue especificado)

            // ============================================================
            // FASE 6: Invocación dinámica del método Parse<T> genérico
            // ============================================================
            // Como no conocemos el tipo del verbo en tiempo de compilación, necesitamos
            // usar reflexión para invocar el método genérico Parse<T>(string[] args)
            // con el tipo específico del verbo encontrado
            
            // Buscar el método genérico Parse<T>(string[] args) en la clase CommandLine
            MethodInfo genericParseMethod = null;
            foreach (MethodInfo method in typeof(CommandLine).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                // Buscar el método que:
                // - Se llame "Parse"
                // - Sea una definición de método genérico (IsGenericMethodDefinition)
                // - Tenga exactamente 1 argumento de tipo genérico
                // - Tenga exactamente 1 parámetro
                // - Ese parámetro sea de tipo string[]
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

            // Crear una versión concreta del método genérico usando el tipo del verbo encontrado
            // Por ejemplo: Parse<Add>(string[] args) o Parse<Commit>(string[] args)
            MethodInfo concreteParseMethod = genericParseMethod.MakeGenericMethod(targetVerbType);

            // ============================================================
            // FASE 7: Ejecución del parsing y manejo de excepciones
            // ============================================================
            try
            {
                // Invocar el método Parse<T> con los argumentos restantes (sin el verbo)
                // null como primer parámetro porque es un método estático
                return concreteParseMethod.Invoke(null, new object[] { remainingArgs });
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                // Cuando se invoca un método mediante reflexión, las excepciones se envuelven
                // en TargetInvocationException. Necesitamos extraer la excepción real (InnerException)
                // para que el código que llama a este método reciba la excepción correcta
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

                // Mostrar nombre del verbo, su helptext y si es por defecto
                string defaultText = verbAttribute.IsDefault ? " (por defecto)" : "";
                sb.AppendLine($"  {verbAttribute.Name,-12} {verbAttribute.HelpText}{defaultText}");

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
