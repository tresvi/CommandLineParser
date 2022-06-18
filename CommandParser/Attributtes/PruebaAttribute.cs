using System;
using System.Collections.Generic;
using System.Text;

namespace CommandParser.Attributtes
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class PruebaAttribute  : Attribute 
    {
        public string Nombre { get; set; }

        public PruebaAttribute(string nombre)
        { 
            
            this.Nombre = nombre;
        }
    }
}
