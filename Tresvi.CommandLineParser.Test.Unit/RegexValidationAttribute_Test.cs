using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

namespace Test_CommandParser
{
    [TestFixture]
    public class RegexValidationAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Alphanumeric Code Tests

        [TestCase(@"--code AB1234")]
        [TestCase(@"--code XY9876")]
        [TestCase(@"--code ZZ0000")]
        public void Parse_RegexValidation_AlphanumericCode_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_AlphanumericCode result = CommandLine.Parse<Params_With_RegexValidation_AlphanumericCode>(args);

            Assert.IsNotNull(result.Code);
            Assert.AreEqual(args[1], result.Code);
        }

        [TestCase(@"--code ab1234", "ab1234")]
        [TestCase(@"--code AB12", "AB12")]
        [TestCase(@"--code AB12345", "AB12345")]
        [TestCase(@"--code A1234", "A1234")]
        [TestCase(@"--code ABC1234", "ABC1234")]
        [TestCase(@"--code 1234", "1234")]
        [TestCase(@"--code ", "")]
        public void Parse_RegexValidation_AlphanumericCode_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_AlphanumericCode>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("AB1234"));
        }

        #endregion

        #region Phone Number Tests

        [TestCase(@"--phone +1-555-123-4567")]
        [TestCase(@"--phone 555-123-4567")]
        [TestCase(@"--phone 555.123.4567")]
        [TestCase(@"--phone +34912345678")]
        public void Parse_RegexValidation_PhoneNumber_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_PhoneNumber result = CommandLine.Parse<Params_With_RegexValidation_PhoneNumber>(args);

            Assert.IsNotNull(result.Phone);
            Assert.AreEqual(args[1], result.Phone);
        }

        [TestCase(@"--phone abc-def-ghij", "abc-def-ghij")]
        [TestCase(@"--phone 123", "123")]
        [TestCase(@"--phone ", "")]
        public void Parse_RegexValidation_PhoneNumber_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_PhoneNumber>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("teléfono"));
        }

        #endregion

        #region Date Format Tests

        [TestCase(@"--date 2024-01-15")]
        [TestCase(@"--date 2023-12-31")]
        [TestCase(@"--date 2000-01-01")]
        public void Parse_RegexValidation_Date_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_Date result = CommandLine.Parse<Params_With_RegexValidation_Date>(args);

            Assert.IsNotNull(result.Date);
            Assert.AreEqual(args[1], result.Date);
        }

        [TestCase(@"--date 2024/01/15", "2024/01/15")]
        [TestCase(@"--date 24-01-15", "24-01-15")]
        [TestCase(@"--date 2024-1-15", "2024-1-15")]
        [TestCase(@"--date 2024-01-5", "2024-01-5")]
        [TestCase(@"--date invalid", "invalid")]
        [TestCase(@"--date ", "")]
        public void Parse_RegexValidation_Date_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_Date>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("YYYY-MM-DD"));
        }

        #endregion

        #region Case Insensitive Tests

        [TestCase(@"--word hello")]
        [TestCase(@"--word HELLO")]
        [TestCase(@"--word Hello")]
        [TestCase(@"--word test")]
        [TestCase(@"--word TEST")]
        public void Parse_RegexValidation_CaseInsensitive_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_CaseInsensitive result = CommandLine.Parse<Params_With_RegexValidation_CaseInsensitive>(args);

            Assert.IsNotNull(result.Word);
            Assert.AreEqual(args[1], result.Word);
        }

        [TestCase(@"--word HELLO123", "HELLO123")]
        [TestCase(@"--word ab", "ab")]
        [TestCase(@"--word toolongword", "toolongword")]
        [TestCase(@"--word ", "")]
        public void Parse_RegexValidation_CaseInsensitive_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_CaseInsensitive>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("minúsculas"));
        }

        #endregion

        #region Default Error Message Tests

        [TestCase(@"--zipcode 12345")]
        [TestCase(@"--zipcode 00000")]
        [TestCase(@"--zipcode 99999")]
        public void Parse_RegexValidation_DefaultMessage_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_DefaultMessage result = CommandLine.Parse<Params_With_RegexValidation_DefaultMessage>(args);

            Assert.IsNotNull(result.ZipCode);
            Assert.AreEqual(args[1], result.ZipCode);
        }

        [TestCase(@"--zipcode 1234", "1234")]
        [TestCase(@"--zipcode 123456", "123456")]
        [TestCase(@"--zipcode abcde", "abcde")]
        [TestCase(@"--zipcode ", "")]
        public void Parse_RegexValidation_DefaultMessage_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_DefaultMessage>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("patrón"));
        }

        #endregion

        #region Multiple Options Tests

        [TestCase(@"--country USA --birthdate 15-01-1990")]
        [TestCase(@"--country ESP --birthdate 31-12-2000")]
        public void Parse_RegexValidation_MultipleOptions_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_MultipleOptions result = CommandLine.Parse<Params_With_RegexValidation_MultipleOptions>(args);

            Assert.IsNotNull(result.Country);
            Assert.IsNotNull(result.BirthDate);
        }

        [TestCase(@"--country US --birthdate 15-01-1990", "US")]
        [TestCase(@"--country USA --birthdate 1990-01-15", "1990-01-15")]
        public void Parse_RegexValidation_MultipleOptions_InvalidValues_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exception = Assert.Throws<InvalidFormatException>(
                () => CommandLine.Parse<Params_With_RegexValidation_MultipleOptions>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue).Or.Contain("patrón").Or.Contain("formato").Or.Contain("letras"));
        }

        #endregion

        #region Edge Cases Tests

        [TestCase(@"--code AB1234")]
        public void Parse_RegexValidation_TrimsValue_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_RegexValidation_AlphanumericCode result = CommandLine.Parse<Params_With_RegexValidation_AlphanumericCode>(args);

            Assert.IsNotNull(result.Code);
            Assert.That(result.Code, Is.EqualTo("AB1234"));
        }

        #endregion
    }
}

