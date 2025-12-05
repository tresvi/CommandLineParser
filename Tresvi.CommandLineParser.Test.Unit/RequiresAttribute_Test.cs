using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    [TestFixture]
    public class RequiresAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        // ============================================================
        // CASOS EXITOSOS: Cuando se usan parámetros con sus requeridos
        // ============================================================

        [TestCase("--username admin --password secret123")]
        [TestCase("-u admin -p secret123")]
        [TestCase("--username admin -p secret123")]
        [TestCase("-u admin --password secret123")]
        public void Parse_RequiresAttribute_UsernameWithPassword_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.Username, Is.EqualTo("admin"));
            Assert.That(result.Password, Is.EqualTo("secret123"));
        }

        [TestCase("--output-file result.txt --output-format json --encoding utf8")]
        [TestCase("-o result.txt -f json -e utf8")]
        public void Parse_RequiresAttribute_OutputFileWithRequiredParams_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.OutputFile, Is.EqualTo("result.txt"));
            Assert.That(result.OutputFormat, Is.EqualTo("json"));
            Assert.That(result.Encoding, Is.EqualTo("utf8"));
        }

        [TestCase("--encrypt --encryption-key mykey123")]
        [TestCase("-c --encryption-key mykey123")]
        [TestCase("--encrypt -k mykey123")]
        [TestCase("-c -k mykey123")]
        public void Parse_RequiresAttribute_EncryptWithKey_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.Encrypt, Is.True);
            Assert.That(result.EncryptionKey, Is.EqualTo("mykey123"));
        }

        [TestCase("--verbose --log-file app.log")]
        [TestCase("-v --log-file app.log")]
        [TestCase("--verbose -l app.log")]
        [TestCase("-v -l app.log")]
        public void Parse_RequiresAttribute_VerboseWithLogFile_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.Verbose, Is.True);
            Assert.That(result.LogFile, Is.EqualTo("app.log"));
        }

        // ============================================================
        // CASOS DE ERROR: Cuando se usa un parámetro sin su requerido
        // ============================================================

        [TestCase("--username admin")]
        [TestCase("-u admin")]
        public void Parse_RequiresAttribute_UsernameWithoutPassword_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RequiredParameterNotFoundException ex = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Parameters_Requires>(args))!;

            Assert.That(ex.Message, Does.Contain("--username"));
            Assert.That(ex.Message, Does.Contain("--password"));
            Assert.That(ex.Message, Does.Contain("requiere"));
        }

        [TestCase("--output-file result.txt")]
        [TestCase("-o result.txt")]
        public void Parse_RequiresAttribute_OutputFileWithoutRequiredParams_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RequiredParameterNotFoundException ex = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Parameters_Requires>(args))!;

            Assert.That(ex.Message, Does.Contain("--output-file"));
            Assert.That(ex.Message, Does.Contain("requiere"));
        }

        [TestCase("--output-file result.txt --output-format json")]
        [TestCase("-o result.txt -f json")]
        public void Parse_RequiresAttribute_OutputFileWithoutEncoding_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RequiredParameterNotFoundException ex = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Parameters_Requires>(args))!;

            Assert.That(ex.Message, Does.Contain("requiere"));
        }

        [TestCase("--encrypt")]
        [TestCase("-c")]
        public void Parse_RequiresAttribute_EncryptWithoutKey_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RequiredParameterNotFoundException ex = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Parameters_Requires>(args))!;

            Assert.That(ex.Message, Does.Contain("--encrypt"));
            Assert.That(ex.Message, Does.Contain("--encryption-key"));
            Assert.That(ex.Message, Does.Contain("requiere"));
        }

        [TestCase("--verbose")]
        [TestCase("-v")]
        public void Parse_RequiresAttribute_VerboseWithoutLogFile_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RequiredParameterNotFoundException ex = Assert.Throws<RequiredParameterNotFoundException>(
                () => CommandLine.Parse<Parameters_Requires>(args))!;

            Assert.That(ex.Message, Does.Contain("--verbose"));
            Assert.That(ex.Message, Does.Contain("--log-file"));
            Assert.That(ex.Message, Does.Contain("requiere"));
        }

        // ============================================================
        // CASOS ADICIONALES: Parámetros sin requerimientos
        // ============================================================

        [TestCase("--password secret123")]
        [TestCase("-p secret123")]
        public void Parse_RequiresAttribute_OnlyPassword_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.Password, Is.EqualTo("secret123"));
            Assert.That(result.Username, Is.Null);
        }

        [TestCase("--output-format json")]
        [TestCase("-f json")]
        public void Parse_RequiresAttribute_OnlyOutputFormat_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.OutputFormat, Is.EqualTo("json"));
            Assert.That(result.OutputFile, Is.Null);
        }

        [TestCase("")]
        public void Parse_RequiresAttribute_NoParameters_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Requires result = CommandLine.Parse<Parameters_Requires>(args);

            Assert.That(result.Username, Is.Null);
            Assert.That(result.Password, Is.Null);
            Assert.That(result.OutputFile, Is.Null);
        }

    }
}

