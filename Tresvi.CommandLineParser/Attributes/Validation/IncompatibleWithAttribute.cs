using System;

namespace Tresvi.CommandParser.Attributes.Validation
{
    /// <summary>
    /// Atributo que indica que esta propiedad es incompatible con otras propiedades especificadas.
    /// Si ambas propiedades están presentes en la línea de comandos, se lanzará una excepción.
    /// </summary>
    /// <example>
    /// <code>
    /// [Option("input", 'i', false, "Archivo de entrada")]
    /// [IncompatibleWith(nameof(Output), nameof(OutputFile))]
    /// public string Input { get; set; }
    /// 
    /// [Option("output", 'o', false, "Archivo de salida")]
    /// public string Output { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IncompatibleWithAttribute : Attribute
    {
        /// <summary>
        /// Nombres de las propiedades con las que esta propiedad es incompatible.
        /// </summary>
        public string[] IncompatiblePropertyNames { get; }

        /// <summary>
        /// Inicializa una nueva instancia del atributo IncompatibleWith.
        /// </summary>
        /// <param name="incompatiblePropertyNames">Nombres de las propiedades incompatibles. Se recomienda usar nameof() para validación en tiempo de compilación.</param>
        public IncompatibleWithAttribute(params string[] incompatiblePropertyNames)
        {
            IncompatiblePropertyNames = incompatiblePropertyNames ?? new string[0];
        }
    }
}

