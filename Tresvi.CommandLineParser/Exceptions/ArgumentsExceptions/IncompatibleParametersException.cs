using System;

namespace Tresvi.CommandParser.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando se detectan parámetros incompatibles en la línea de comandos.
    /// </summary>
    public class IncompatibleParametersException : CommandParserBaseException
    {
        public IncompatibleParametersException() { }
        public IncompatibleParametersException(string message) : base(message) { }
        public IncompatibleParametersException(string message, Exception inner) : base(message, inner) { }
    }
}

