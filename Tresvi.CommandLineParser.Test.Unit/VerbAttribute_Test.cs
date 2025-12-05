using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using Test_CommandParser.Models.Verbs;

namespace Test_CommandParser
{
    [TestFixture]
    public class VerbAttribute_Test
    {

        [Test]
        public void Parse_WithDefaultVerb_NoArguments_UsesDefaultVerb()
        {
            // Arrange
            string[] args = new string[0];

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Status>());
            Status status = (Status)result;
            Assert.That(status.Verbose, Is.False);
        }

        [Test]
        public void Parse_WithDefaultVerb_FirstArgIsParameter_UsesDefaultVerb()
        {
            // Arrange
            string[] args = new[] { "--verbose" };

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Status>());
            Status status = (Status)result;
            Assert.That(status.Verbose, Is.True);
        }

        [Test]
        public void Parse_WithDefaultVerb_FirstArgIsShortParameter_UsesDefaultVerb()
        {
            // Arrange
            string[] args = new[] { "-v" };

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Status>());
            Status status = (Status)result;
            Assert.That(status.Verbose, Is.True);
        }

        [Test]
        public void Parse_WithDefaultVerb_SpecificVerbSpecified_UsesSpecificVerb()
        {
            // Arrange
            string[] args = new[] { "add", "--directory", @"C:\Temp\", "-n", "test.txt" };

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Add>());
            Add add = (Add)result;
            Assert.That(add.Directory, Is.EqualTo(@"C:\Temp\"));
            Assert.That(add.Nombre, Is.EqualTo("test.txt"));
        }

        [Test]
        public void Parse_WithDefaultVerb_DefaultVerbExplicitlySpecified_UsesDefaultVerb()
        {
            // Arrange
            string[] args = new[] { "status", "--verbose" };

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Status>());
            Status status = (Status)result;
            Assert.That(status.Verbose, Is.True);
        }

        [Test]
        public void GetHelpTextForVerbs_WithDefaultVerb_ShowsDefaultIndicator()
        {
            // Arrange & Act
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Status), typeof(Add), typeof(Log));

            // Assert
            Assert.That(helpText, Does.Contain("status"));
            Assert.That(helpText, Does.Contain("(por defecto)"));
            Assert.That(helpText, Does.Contain("add"));
            Assert.That(helpText, Does.Not.Contain("add (por defecto)"));
            Assert.That(helpText, Does.Contain("log"));
            Assert.That(helpText, Does.Not.Contain("log (por defecto)"));
        }


        [Test]
        public void Parse_NoDefaultVerb_NoArguments_ThrowsNotDefaultVerbException()
        {
            // Arrange
            string[] args = new string[0];

            // Act & Assert
            NotDefaultVerbException? exception = Assert.Throws<NotDefaultVerbException>(
                () => CommandLine.Parse(args, typeof(Add), typeof(Edit)));

            Assert.That(exception?.Message, Does.Contain("Debe especificar un verbo"));
            Assert.That(exception?.Message, Does.Contain("No hay un verbo por defecto definido"));
        }

        [Test]
        public void Parse_NoDefaultVerb_FirstArgIsParameter_ThrowsNotDefaultVerbException()
        {
            // Arrange
            string[] args = new[] { "--directory", @"C:\Temp\" };

            // Act & Assert
            NotDefaultVerbException? exception = Assert.Throws<NotDefaultVerbException>(
                () => CommandLine.Parse(args, typeof(Add), typeof(Edit)));

            Assert.That(exception?.Message, Does.Contain("Debe especificar un verbo"));
            Assert.That(exception?.Message, Does.Contain("No hay un verbo por defecto definido"));
        }

        [Test]
        public void Parse_TwoDefaultVerbs_ThrowsTooManyDefaultVerbsException()
        {
            // Arrange
            string[] args = new[] { "default1" };

            // Act & Assert
            TooManyDefaultVerbsException? exception = Assert.Throws<TooManyDefaultVerbsException>(
                () => CommandLine.Parse(args, typeof(DefaultVerb1), typeof(DefaultVerb2)));

            Assert.That(exception?.Message, Does.Contain("Solo puede haber un verbo marcado como por defecto"));
        }

        [Test]
        public void Parse_DefaultVerb_WithOptionParameter_ParsesCorrectly()
        {
            // Arrange
            string[] args = new[] { "--verbose" };

            // Act
            object result = CommandLine.Parse(args, typeof(Status), typeof(Add));

            // Assert
            Assert.That(result, Is.InstanceOf<Status>());
            Status status = (Status)result;
            Assert.That(status.Verbose, Is.True);
        }

        [Test]
        public void Parse_DefaultVerb_UnknownVerbStillThrowsUnknownVerbException()
        {
            // Arrange
            string[] args = new[] { "unknownverb" };

            // Act & Assert
            UnknownVerbException? exception = Assert.Throws<UnknownVerbException>(
                () => CommandLine.Parse(args, typeof(Status), typeof(Add)));

            Assert.That(exception?.Message, Does.Contain("unknownverb"));
            Assert.That(exception?.Message, Does.Contain("status"));
            Assert.That(exception?.Message, Does.Contain("add"));
        }

        [Test]
        public void GetHelpTextForVerbs_NoDefaultVerb_DoesNotShowDefaultIndicator()
        {
            // Arrange & Act
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Add), typeof(Edit));

            // Assert
            Assert.That(helpText, Does.Contain("add"));
            Assert.That(helpText, Does.Contain("edit"));
            Assert.That(helpText, Does.Not.Contain("(por defecto)"));
        }

        [Test]
        public void Parse_DefaultVerb_EmptyStringArgument_ThrowsNotDefaultVerbException()
        {
            // Arrange
            string[] args = new[] { "" };

            // Act & Assert
            NotDefaultVerbException? exception = Assert.Throws<NotDefaultVerbException>(
                () => CommandLine.Parse(args, typeof(Status), typeof(Add)));

            Assert.That(exception?.Message, Does.Contain("Debe especificar un verbo"));
        }

        [Test]
        public void Parse_DefaultVerb_WhitespaceArgument_ThrowsNotDefaultVerbException()
        {
            // Arrange
            string[] args = new[] { "   " };

            // Act & Assert
            NotDefaultVerbException? exception = Assert.Throws<NotDefaultVerbException>(
                () => CommandLine.Parse(args, typeof(Status), typeof(Add)));

            Assert.That(exception?.Message, Does.Contain("Debe especificar un verbo"));
        }
    }
}

