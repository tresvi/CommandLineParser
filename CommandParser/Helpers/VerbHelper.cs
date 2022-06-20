using CommandParser.Attributtes;
using CommandParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace CommandParser.Helpers
{
    internal class VerbHelper
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
        internal static void ValidateVerbDecoration<T1, T2>() 
            where T1 : new()
            where T2 : new()
        {
            object Tclass = new T1();
            if (Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute)) ==  null)
                throw new NotVerbClassException($"La clase {Tclass.GetType().Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");

            Tclass = new T2();
            if (Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute)) == null)
                throw new NotVerbClassException($"La clase {Tclass.GetType().Name} fue utilizada como un verbo, pero no fue decorada como {typeof(VerbAttribute).Name}");
        }


        internal static object DetectDefaultVerb<T1, T2>()
            where T1 : new()
            where T2 : new()
        {
            VerbAttribute verbAttribute;
            object Tclass;
            
            Tclass = new T1();
            verbAttribute = (VerbAttribute) Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute));
            if (verbAttribute.IsDefault) return Tclass;

            Tclass = new T2();
            verbAttribute = (VerbAttribute)Tclass.GetType().GetCustomAttribute(typeof(VerbAttribute));
            if (verbAttribute.IsDefault) return Tclass;

            return null;
        }

    }
}
