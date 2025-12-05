using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    [TestFixture]
    public class IncompatibleWithAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("--out output.txt")]
        [TestCase("-o output.txt")]
        public void Parse_IncompatibleParameters_OnlyOneParameter_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Incompatible result = CommandLine.Parse<Parameters_Incompatible>(args);

            Assert.That(result.OutputPath, Is.EqualTo("output.txt"));
            Assert.That(result.Overwrite, Is.False);
            Assert.That(result.Append, Is.False);
        }

        [TestCase("--overwrite")]
        [TestCase("-w")]
        public void Parse_IncompatibleParameters_OnlyFlag_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Incompatible result = CommandLine.Parse<Parameters_Incompatible>(args);

            Assert.That(result.Overwrite, Is.True);
            Assert.That(result.Append, Is.False);
            Assert.That(result.OutputPath, Is.Null);
        }

        [TestCase("--append")]
        [TestCase("-a")]
        public void Parse_IncompatibleParameters_OnlyAppend_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Incompatible result = CommandLine.Parse<Parameters_Incompatible>(args);

            Assert.That(result.Append, Is.True);
            Assert.That(result.Overwrite, Is.False);
            Assert.That(result.OutputPath, Is.Null);
        }

        [TestCase("--out output.txt --overwrite")]
        [TestCase("--out output.txt -w")]
        [TestCase("-o output.txt --overwrite")]
        [TestCase("-o output.txt -w")]
        public void Parse_IncompatibleParameters_OutputWithOverwrite_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IncompatibleParametersException ex = Assert.Throws<IncompatibleParametersException>(
                () => CommandLine.Parse<Parameters_Incompatible>(args))!;

            Assert.That(ex.Message, Does.Contain("no pueden usarse juntos"));
        }

        [TestCase("--out output.txt --append")]
        [TestCase("--out output.txt -a")]
        [TestCase("-o output.txt --append")]
        [TestCase("-o output.txt -a")]
        public void Parse_IncompatibleParameters_OutputWithAppend_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IncompatibleParametersException ex = Assert.Throws<IncompatibleParametersException>(
                () => CommandLine.Parse<Parameters_Incompatible>(args))!;

            Assert.That(ex.Message, Does.Contain("no pueden usarse juntos"));
        }

        [TestCase("--overwrite --append")]
        [TestCase("--overwrite -a")]
        [TestCase("-w --append")]
        [TestCase("-w -a")]
        public void Parse_IncompatibleParameters_OverwriteWithAppend_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IncompatibleParametersException ex = Assert.Throws<IncompatibleParametersException>(
                () => CommandLine.Parse<Parameters_Incompatible>(args))!;

            Assert.That(ex.Message, Does.Contain("no pueden usarse juntos"));
        }

        [TestCase("--out output.txt --overwrite --append")]
        [TestCase("-o output.txt -w -a")]
        public void Parse_IncompatibleParameters_AllThree_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IncompatibleParametersException ex = Assert.Throws<IncompatibleParametersException>(
                () => CommandLine.Parse<Parameters_Incompatible>(args))!;

            Assert.That(ex.Message, Does.Contain("no pueden usarse juntos"));
        }

        [TestCase("--input input.txt --out output.txt")]
        [TestCase("--input input.txt -o output.txt")]
        [TestCase("-i input.txt --out output.txt")]
        [TestCase("-i input.txt -o output.txt")]
        public void Parse_IncompatibleParameters_InputWithOutput_ThrowsException(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IncompatibleParametersException ex = Assert.Throws<IncompatibleParametersException>(
                () => CommandLine.Parse<Parameters_Incompatible>(args))!;

            Assert.That(ex.Message, Does.Contain("no pueden usarse juntos"));
        }

        [TestCase("--input input.txt")]
        [TestCase("-i input.txt")]
        public void Parse_IncompatibleParameters_OnlyInput_OK(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Parameters_Incompatible result = CommandLine.Parse<Parameters_Incompatible>(args);

            Assert.That(result.Input, Is.EqualTo("input.txt"));
            Assert.That(result.OutputPath, Is.Null);
        }
    }
}

