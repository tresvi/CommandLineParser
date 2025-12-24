using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    [TestFixture]
    public class EnumOptionAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Parseo Automático - Casos Felices

        [TestCase(@"--level Debug", LogLevel.Debug)]
        [TestCase(@"--level Info", LogLevel.Info)]
        [TestCase(@"--level Warning", LogLevel.Warning)]
        [TestCase(@"--level Error", LogLevel.Error)]
        public void Parse_Enum_Simple_ValidNames_CaseSensitive_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Simple result = CommandLine.Parse<Params_With_Enum_Simple>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        [TestCase(@"--level debug", LogLevel.Debug)]
        [TestCase(@"--level INFO", LogLevel.Info)]
        [TestCase(@"--level warning", LogLevel.Warning)]
        [TestCase(@"--level ERROR", LogLevel.Error)]
        [TestCase(@"--level DeBuG", LogLevel.Debug)]
        public void Parse_Enum_Simple_ValidNames_CaseInsensitive_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Simple result = CommandLine.Parse<Params_With_Enum_Simple>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        [TestCase(@"--level 0", LogLevel.Debug)]
        [TestCase(@"--level 1", LogLevel.Info)]
        [TestCase(@"--level 2", LogLevel.Warning)]
        [TestCase(@"--level 3", LogLevel.Error)]
        public void Parse_Enum_Simple_ValidNumericIndices_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Simple result = CommandLine.Parse<Params_With_Enum_Simple>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        #endregion

        #region Enum Nullable - Casos Felices

        [TestCase(@"--env Development", Test_CommandParser.Models.Environment.Development)]
        [TestCase(@"--env Staging", Test_CommandParser.Models.Environment.Staging)]
        [TestCase(@"--env Production", Test_CommandParser.Models.Environment.Production)]
        public void Parse_Enum_Nullable_ValidNames_OK(string inputLine, Test_CommandParser.Models.Environment expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Nullable result = CommandLine.Parse<Params_With_Enum_Nullable>(args);

            Assert.AreEqual(expectedValue, result.Environment);
            Assert.IsNotNull(result.Environment);
        }

        [TestCase(@"--env development", Test_CommandParser.Models.Environment.Development)]
        [TestCase(@"--env STAGING", Test_CommandParser.Models.Environment.Staging)]
        [TestCase(@"--env production", Test_CommandParser.Models.Environment.Production)]
        public void Parse_Enum_Nullable_CaseInsensitive_OK(string inputLine, Test_CommandParser.Models.Environment expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Nullable result = CommandLine.Parse<Params_With_Enum_Nullable>(args);

            Assert.AreEqual(expectedValue, result.Environment);
        }

        [TestCase(@"--env 0", Test_CommandParser.Models.Environment.Development)]
        [TestCase(@"--env 1", Test_CommandParser.Models.Environment.Staging)]
        [TestCase(@"--env 2", Test_CommandParser.Models.Environment.Production)]
        public void Parse_Enum_Nullable_ValidNumericIndices_OK(string inputLine, Test_CommandParser.Models.Environment expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Nullable result = CommandLine.Parse<Params_With_Enum_Nullable>(args);

            Assert.AreEqual(expectedValue, result.Environment);
        }

        #endregion

        #region Enum con Mapeo Personalizado - Casos Felices

        [TestCase(@"--format json", OutputFormat.Json)]
        [TestCase(@"--format xml", OutputFormat.Xml)]
        [TestCase(@"--format csv", OutputFormat.Csv)]
        public void Parse_Enum_WithMapping_ValidMappedValues_OK(string inputLine, OutputFormat expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_WithMapping result = CommandLine.Parse<Params_With_Enum_WithMapping>(args);

            Assert.AreEqual(expectedValue, result.Format);
        }

        [TestCase(@"--format j", OutputFormat.Json)]
        [TestCase(@"--format x", OutputFormat.Xml)]
        public void Parse_Enum_WithMapping_ValidAliases_OK(string inputLine, OutputFormat expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_WithMapping result = CommandLine.Parse<Params_With_Enum_WithMapping>(args);

            Assert.AreEqual(expectedValue, result.Format);
        }

        [TestCase(@"--format J", OutputFormat.Json)]
        [TestCase(@"--format X", OutputFormat.Xml)]
        [TestCase(@"--format JSON", OutputFormat.Json)]
        public void Parse_Enum_WithMapping_CaseInsensitive_OK(string inputLine, OutputFormat expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_WithMapping result = CommandLine.Parse<Params_With_Enum_WithMapping>(args);

            Assert.AreEqual(expectedValue, result.Format);
        }

        #endregion

        #region Enum Nullable con Mapeo - Casos Felices

        [TestCase(@"--env dev", Test_CommandParser.Models.Environment.Development)]
        [TestCase(@"--env staging", Test_CommandParser.Models.Environment.Staging)]
        [TestCase(@"--env prod", Test_CommandParser.Models.Environment.Production)]
        public void Parse_Enum_Nullable_WithMapping_ValidMappedValues_OK(string inputLine, Test_CommandParser.Models.Environment expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Nullable_WithMapping result = CommandLine.Parse<Params_With_Enum_Nullable_WithMapping>(args);

            Assert.AreEqual(expectedValue, result.Environment);
            Assert.IsNotNull(result.Environment);
        }

        #endregion

        #region Enum con Valores Numéricos Personalizados - Casos Felices

        [TestCase(@"--priority Low", Priority.Low)]
        [TestCase(@"--priority Medium", Priority.Medium)]
        [TestCase(@"--priority High", Priority.High)]
        [TestCase(@"--priority Critical", Priority.Critical)]
        public void Parse_Enum_NumericValues_ValidNames_OK(string inputLine, Priority expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_NumericValues result = CommandLine.Parse<Params_With_Enum_NumericValues>(args);

            Assert.AreEqual(expectedValue, result.Priority);
        }

        [TestCase(@"--priority 1", Priority.Low)]
        [TestCase(@"--priority 5", Priority.Medium)]
        [TestCase(@"--priority 10", Priority.High)]
        [TestCase(@"--priority 99", Priority.Critical)]
        public void Parse_Enum_NumericValues_ValidNumericValues_OK(string inputLine, Priority expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_NumericValues result = CommandLine.Parse<Params_With_Enum_NumericValues>(args);

            Assert.AreEqual(expectedValue, result.Priority);
        }

        #endregion

        #region Enum con EnumeratedValidation - Casos Felices

        [TestCase(@"--level Debug", LogLevel.Debug)]
        [TestCase(@"--level Info", LogLevel.Info)]
        [TestCase(@"--level Warning", LogLevel.Warning)]
        public void Parse_Enum_WithValidation_ValidValues_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_WithValidation result = CommandLine.Parse<Params_With_Enum_WithValidation>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        #endregion

        #region Múltiples Enums - Casos Felices

        [Test]
        public void Parse_Enum_Multiple_AllValid_OK()
        {
            string[] args = new[] { "--level", "Info", "--env", "Staging", "--format", "Json" };
            Params_With_Enum_Multiple result = CommandLine.Parse<Params_With_Enum_Multiple>(args);

            Assert.AreEqual(LogLevel.Info, result.LogLevel);
            Assert.AreEqual(Test_CommandParser.Models.Environment.Staging, result.Environment);
            Assert.AreEqual(OutputFormat.Json, result.Format);
        }

        #endregion

        #region Enum Requerido - Casos Felices

        [TestCase(@"--level Debug", LogLevel.Debug)]
        [TestCase(@"--level Info", LogLevel.Info)]
        public void Parse_Enum_Required_ValidValue_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(' ');
            Params_With_Enum_Required result = CommandLine.Parse<Params_With_Enum_Required>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        #endregion

        #region Casos No Felices - Valores Inválidos

        [TestCase(@"--level InvalidValue", "InvalidValue")]
        [TestCase(@"--level DebugInfo", "DebugInfo")]
        [TestCase(@"--level ", "")]
        public void Parse_Enum_Simple_InvalidName_Throws_ParseValueException(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            ParseValueException? exception = Assert.Throws<ParseValueException>(
                () => CommandLine.Parse<Params_With_Enum_Simple>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()));
            Assert.That(exception?.Message, Does.Contain("LogLevel"));
        }

        [TestCase(@"--level 99", "99")]
        [TestCase(@"--level -1", "-1")]
        [TestCase(@"--level 100", "100")]
        public void Parse_Enum_Simple_InvalidNumericIndex_Throws_ParseValueException(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            ParseValueException? exception = Assert.Throws<ParseValueException>(
                () => CommandLine.Parse<Params_With_Enum_Simple>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("LogLevel"));
        }

        #endregion

        #region Casos No Felices - Mapeo Personalizado

        [TestCase(@"--format yaml", "yaml")]
        [TestCase(@"--format invalid", "invalid")]
        public void Parse_Enum_WithMapping_InvalidValue_NotInMapping_Throws_ParseValueException(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            ParseValueException? exception = Assert.Throws<ParseValueException>(
                () => CommandLine.Parse<Params_With_Enum_WithMapping>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("mapeados permitidos"));
        }

        // Nota: "Json", "XML" y "Csv" funcionan porque coinciden con los mapeos "json", "xml", "csv" 
        // (case-insensitive). Esto es el comportamiento esperado.

        #endregion

        #region Casos No Felices - EnumeratedValidation

        [TestCase(@"--level Error", "Error")]
        public void Parse_Enum_WithValidation_InvalidValue_Throws_InvalidEnumeratedValueException(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_Enum_WithValidation>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("Debug, Info, Warning"));
        }

        #endregion

        #region Casos No Felices - Enum Requerido

        [Test]
        public void Parse_Enum_Required_MissingValue_Throws_ValueNotFoundException()
        {
            string[] args = new[] { "--level" };
            ValueNotFoundException? exception = Assert.Throws<ValueNotFoundException>(
                () => CommandLine.Parse<Params_With_Enum_Required>(args));

            Assert.That(exception?.Message, Does.Contain("--level"));
        }

        [Test]
        public void Parse_Enum_Required_NotProvided_Throws_RequiredParameterNotFoundException()
        {
            string[] args = new string[0];
            RequiredParameterNotFoundException? exception = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Params_With_Enum_Required>(args));

            Assert.IsNotNull(exception);
            Assert.That(exception?.Message, Does.Contain("--level"));
        }

        #endregion

        #region Casos Edge - Valores con Espacios

        [TestCase(@"--level Debug", LogLevel.Debug)]
        [TestCase(@"--level Info", LogLevel.Info)]
        public void Parse_Enum_Simple_WithSpaces_Trims_OK(string inputLine, LogLevel expectedValue)
        {
            string[] args = inputLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Params_With_Enum_Simple result = CommandLine.Parse<Params_With_Enum_Simple>(args);

            Assert.AreEqual(expectedValue, result.LogLevel);
        }

        #endregion

        #region Casos Edge - Valores Numéricos con Valores Personalizados

        [TestCase(@"--priority 0", "0")]
        [TestCase(@"--priority 2", "2")]
        [TestCase(@"--priority 50", "50")]
        public void Parse_Enum_NumericValues_InvalidNumericValue_Throws_ParseValueException(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            ParseValueException? exception = Assert.Throws<ParseValueException>(
                () => CommandLine.Parse<Params_With_Enum_NumericValues>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("Priority"));
        }

        #endregion
    }
}

