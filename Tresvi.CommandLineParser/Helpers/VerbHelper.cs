using Tresvi.CommandParser.Attributtes;
using Tresvi.CommandParser.Attributtes.Keywords;
using Tresvi.CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Tresvi.CommandParser.Helpers
{
    internal static class VerbHelper
    {
        internal static bool CheckIfVerbIsInClass<T>(string searchedVerb) where T : new()
        {
            T Tclass = new T();
            Attribute verb = Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute));
            //if (verb == null) throw new NotVerbClassException($"La clase {Tclass.GetType().Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");

            VerbAttribute verbAttribute = (VerbAttribute)verb;

            if (verbAttribute.Name == searchedVerb)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Check if the class was decorated with the Verb attribute 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <exception cref="NotVerbClassException"></exception>
        internal static List<string> ValidateVerbDecoration<T1, T2>()
            where T1 : new()
            where T2 : new()
        {
            List<string> verbsAvailable = new List<string>();

            object Tclass = new T1();
            if (Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute)) == null)
                throw new NotVerbClassException($"La clase {Tclass.GetType().Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");
            verbsAvailable.Add(Tclass.GetType().Name);

            Tclass = new T2();
            if (Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute)) == null)
                throw new NotVerbClassException($"La clase {Tclass.GetType().Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");
            verbsAvailable.Add(Tclass.GetType().Name);

            return verbsAvailable;
        }


        /*
        internal static List<string> DetectDefaultVerbs<T1, T2>()
            where T1 : new()
            where T2 : new()
        {
            List<string> defaultClasses = new List<string>();
            VerbAttribute verbAttribute;
            object Tclass;
            
            Tclass = new T1();
            verbAttribute = (VerbAttribute) Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute));
            if (verbAttribute.IsDefault) defaultClasses.Add(Tclass.GetType().Name);

            Tclass = new T2();
            verbAttribute = (VerbAttribute)Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute));
            if (verbAttribute.IsDefault) defaultClasses.Add(Tclass.GetType().Name);

            return defaultClasses;
        }
        */
    }
}
