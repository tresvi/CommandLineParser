using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

namespace Test_CommandParser
{
    [TestFixture]
    public class EmailValidationAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region EmailValidationAttribute Tests

        [TestCase(@"--email user@example.com")]
        [TestCase(@"--email test.email@domain.co.uk")]
        [TestCase(@"--email user.name+tag@example.com")]
        [TestCase(@"--email user_name@example-domain.com")]
        [TestCase(@"--email user123@test123.com")]
        [TestCase(@"--email a@b.co")]
        [TestCase(@"--email user@subdomain.example.com")]
        public void Parse_EmailValidation_ValidEmails_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_EmailValidation result = CommandLine.Parse<Params_With_EmailValidation>(args);

            Assert.IsNotNull(result.Email);
            Assert.AreEqual(args[1], result.Email);
        }

        [TestCase(@"--email invalid.email", "invalid.email")]
        [TestCase(@"--email @example.com", "@example.com")]
        [TestCase(@"--email user@", "user@")]
        [TestCase(@"--email user@.com", "user@.com")]
        [TestCase(@"--email user@example", "user@example")]
        [TestCase(@"--email user@@example.com", "user@@example.com")]
        [TestCase(@"--email .user@example.com", ".user@example.com")]
        [TestCase(@"--email user.@example.com", "user.@example.com")]
        [TestCase(@"--email ", "")]
        public void Parse_EmailValidation_InvalidEmails_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEmailAddressException? exception = Assert.Throws<InvalidEmailAddressException>(
                () => CommandLine.Parse<Params_With_EmailValidation>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío").Or.Contain("espacios"));
        }

        [Test]
        public void Parse_EmailValidation_EmailWithSpaces_Throws_InvalidEmailAddressException()
        {
            string[] args = { "--email", "user name@example.com" };

            var ex = Assert.Throws<InvalidEmailAddressException>(() =>
                CommandLine.Parse<Params_With_EmailValidation>(args));

            // El regex rechaza el espacio antes de la rama Contains(" "); el mensaje es el de valor no válido.
            Assert.That(ex!.Message, Does.Contain("user name@example.com").Or.Contain("espacios"));
        }

        [Test]
        public void Parse_EmailValidation_EmailLongerThan254Chars_Throws_InvalidEmailAddressException()
        {
            string local = new string('a', 250);
            string email = local + "@bc.co";
            Assert.That(email.Length, Is.GreaterThan(254));

            string[] args = { "--email", email };

            var ex = Assert.Throws<InvalidEmailAddressException>(() =>
                CommandLine.Parse<Params_With_EmailValidation>(args));

            Assert.That(ex!.Message, Does.Contain("254"));
        }

        #endregion
    }
}

