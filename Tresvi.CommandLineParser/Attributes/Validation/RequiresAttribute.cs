using System;

namespace Tresvi.CommandParser.Attributes.Validation
{
    /// <summary>
    /// Atributo que indica que esta propiedad requiere que otras propiedades también estén presentes en la línea de comandos.
    /// Si esta propiedad está presente pero alguna de las propiedades requeridas no lo está, se lanzará una excepción.
    /// </summary>
    /// <example>
    /// <code>
    /// [Option("username", 'u', false, "Nombre de usuario")]
    /// [Requires(nameof(Password))]
    /// public string Username { get; set; }
    /// 
    /// [Option("password", 'p', false, "Contraseña")]
    /// public string Password { get; set; }
    /// </code>
    /// En este ejemplo, si se usa --username, también se debe usar --password.
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class RequiresAttribute : Attribute
    {
        /// <summary>
        /// Nombres de las propiedades que son requeridas cuando esta propiedad está presente.
        /// </summary>
        public string[] RequiredPropertyNames { get; }

        /// <summary>
        /// Inicializa una nueva instancia del atributo Requires.
        /// </summary>
        /// <param name="requiredPropertyNames">Nombres de las propiedades requeridas. Se recomienda usar nameof() para validación en tiempo de compilación.</param>
        public RequiresAttribute(params string[] requiredPropertyNames)
        {
            RequiredPropertyNames = requiredPropertyNames ?? new string[0];
        }
    }
}

