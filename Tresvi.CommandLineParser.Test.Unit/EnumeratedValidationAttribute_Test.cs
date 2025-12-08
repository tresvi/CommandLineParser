using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

namespace Test_CommandParser
{
    [TestFixture]
    public class EnumeratedValidationAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Case Sensitive Tests

        [TestCase(@"--color red")]
        [TestCase(@"--color green")]
        [TestCase(@"--color blue")]
        public void Parse_EnumeratedValidation_CaseSensitive_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_CaseSensitive result = CommandLine.Parse<Params_With_EnumeratedValidation_CaseSensitive>(args);

            Assert.AreEqual(args[1], result.Color);
        }

        [TestCase(@"--color RED", "RED")]
        [TestCase(@"--color Green", "Green")]
        [TestCase(@"--color BLUE", "BLUE")]
        [TestCase(@"--color yellow", "yellow")]
        [TestCase(@"--color purple", "purple")]
        public void Parse_EnumeratedValidation_CaseSensitive_InvalidValues_Throws_InvalidEnumeratedValue(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_EnumeratedValidation_CaseSensitive>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()));
            Assert.That(exception?.Message, Does.Contain("red, green, blue"));
        }

        #endregion

        #region Case Insensitive Tests

        [TestCase(@"--environment dev")]
        [TestCase(@"--environment DEV")]
        [TestCase(@"--environment Dev")]
        [TestCase(@"--environment staging")]
        [TestCase(@"--environment STAGING")]
        [TestCase(@"--environment Staging")]
        [TestCase(@"--environment prod")]
        [TestCase(@"--environment PROD")]
        [TestCase(@"--environment Prod")]
        public void Parse_EnumeratedValidation_CaseInsensitive_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_CaseInsensitive result = CommandLine.Parse<Params_With_EnumeratedValidation_CaseInsensitive>(args);

            Assert.AreEqual(args[1], result.Environment);
        }

        [TestCase(@"--environment development", "development")]
        [TestCase(@"--environment production", "production")]
        [TestCase(@"--environment test", "test")]
        [TestCase(@"--environment qa", "qa")]
        public void Parse_EnumeratedValidation_CaseInsensitive_InvalidValues_Throws_InvalidEnumeratedValue(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_EnumeratedValidation_CaseInsensitive>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("dev, staging, prod"));
        }

        #endregion

        #region Single Value Tests

        [TestCase(@"--format json")]
        public void Parse_EnumeratedValidation_SingleValue_Valid_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_SingleValue result = CommandLine.Parse<Params_With_EnumeratedValidation_SingleValue>(args);

            Assert.AreEqual("json", result.Format);
        }

        [TestCase(@"--format xml", "xml")]
        [TestCase(@"--format csv", "csv")]
        [TestCase(@"--format yaml", "yaml")]
        [TestCase(@"--format JSON", "JSON")]
        public void Parse_EnumeratedValidation_SingleValue_Invalid_Throws_InvalidEnumeratedValue(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_EnumeratedValidation_SingleValue>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("json"));
        }

        #endregion

        #region Multiple Options Tests

        [TestCase(@"--mode read --language es")]
        [TestCase(@"--mode write --language EN")]
        [TestCase(@"--mode append --language fr")]
        public void Parse_EnumeratedValidation_MultipleOptions_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_MultipleOptions result = CommandLine.Parse<Params_With_EnumeratedValidation_MultipleOptions>(args);

            Assert.IsNotNull(result.Mode);
            Assert.IsNotNull(result.Language);
        }

        [TestCase(@"--mode read --language invalid", "invalid")]
        [TestCase(@"--mode invalid --language es", "invalid")]
        public void Parse_EnumeratedValidation_MultipleOptions_InvalidValues_Throws_InvalidEnumeratedValue(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_EnumeratedValidation_MultipleOptions>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
        }

        #endregion

        #region Default Case Sensitive Tests

        [TestCase(@"--option option1")]
        [TestCase(@"--option option2")]
        [TestCase(@"--option option3")]
        public void Parse_EnumeratedValidation_DefaultCaseSensitive_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_DefaultCaseSensitive result = CommandLine.Parse<Params_With_EnumeratedValidation_DefaultCaseSensitive>(args);

            Assert.AreEqual(args[1], result.Option);
        }

        [TestCase(@"--option Option1", "Option1")]
        [TestCase(@"--option OPTION1", "OPTION1")]
        [TestCase(@"--option option4", "option4")]
        public void Parse_EnumeratedValidation_DefaultCaseSensitive_InvalidValues_Throws_InvalidEnumeratedValue(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEnumeratedValueException? exception = Assert.Throws<InvalidEnumeratedValueException>(
                () => CommandLine.Parse<Params_With_EnumeratedValidation_DefaultCaseSensitive>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("option1, option2, option3"));
        }

        #endregion

        #region Edge Cases Tests

        [TestCase(@"--color red")]
        [TestCase(@"--color green")]
        [TestCase(@"--color blue")]
        public void Parse_EnumeratedValidation_TrimsValue_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_CaseSensitive result = CommandLine.Parse<Params_With_EnumeratedValidation_CaseSensitive>(args);

            Assert.IsNotNull(result.Color);
            Assert.That(result.Color, Is.EqualTo("red").Or.EqualTo("green").Or.EqualTo("blue"));
        }

        [TestCase(@"--environment dev")]
        [TestCase(@"--environment staging")]
        public void Parse_EnumeratedValidation_CaseInsensitive_TrimsValue_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EnumeratedValidation_CaseInsensitive result = CommandLine.Parse<Params_With_EnumeratedValidation_CaseInsensitive>(args);

            Assert.IsNotNull(result.Environment);
        }

        #endregion
    }
}

