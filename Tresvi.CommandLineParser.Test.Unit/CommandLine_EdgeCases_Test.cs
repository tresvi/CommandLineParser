using System;
using NUnit.Framework;
using Test_CommandParser.Models;
using Test_CommandParser.Models.Verbs;
using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;

namespace Test_CommandParser
{
    /// <summary>
    /// Casos que no usan Split(' ') en la línea de comando, validación defensiva del API
    /// y tipos de propiedad inválidos para flags/opciones.
    /// </summary>
    [TestFixture]
    public class CommandLine_EdgeCases_Test
    {
        [Test]
        public void Parse_Generic_NullArgs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CommandLine.Parse<Parameters>(null!));
        }

        [Test]
        public void Parse_Verbs_NullArgs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                CommandLine.Parse(null!, typeof(Add), typeof(Edit)));
        }

        [Test]
        public void Parse_Verbs_EmptyVerbTypes_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                CommandLine.Parse(Array.Empty<string>(), Array.Empty<Type>()));

            Assert.That(ex!.ParamName, Is.EqualTo("verbTypes"));
            Assert.That(ex.Message, Does.Contain("al menos una clase verbo"));
        }

        [Test]
        public void Parse_Verbs_NullVerbTypes_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                CommandLine.Parse(Array.Empty<string>(), null!));

            Assert.That(ex!.ParamName, Is.EqualTo("verbTypes"));
        }

        [Test]
        public void Parse_T1T2_NullArgs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CommandLine.Parse<Add, Edit>(null!));
        }

        [Test]
        public void Parse_WithPreSplitArgs_PathsWithSpaces_ParsesCorrectly()
        {
            // Simula argv cuando el usuario pone comillas en consola: --inputfile "C:\My Docs\a b.txt"
            string[] args =
            {
                "--inputfile", @"C:\My Docs\input file.txt",
                "--outputfile", @"D:\Out Folder\result out.txt"
            };

            Parameters result = CommandLine.Parse<Parameters>(args);

            Assert.Multiple(() =>
            {
                Assert.That(result.InputFile, Is.EqualTo(@"C:\My Docs\input file.txt"));
                Assert.That(result.OutputFile, Is.EqualTo(@"D:\Out Folder\result out.txt"));
            });
        }

        [Test]
        public void Parse_WithPreSplitArgs_ShortOptionsAndSpaces_ParsesCorrectly()
        {
            string[] args =
            {
                "-i", @"\\server\share\folder name\file (1).txt",
                "-o", @"C:\temp\out file\final.txt"
            };

            Parameters result = CommandLine.Parse<Parameters>(args);

            Assert.Multiple(() =>
            {
                Assert.That(result.InputFile, Is.EqualTo(@"\\server\share\folder name\file (1).txt"));
                Assert.That(result.OutputFile, Is.EqualTo(@"C:\temp\out file\final.txt"));
            });
        }

        [Test]
        public void Parse_Verb_WithOptionValuesContainingSpaces_ParsesCorrectly()
        {
            string[] args =
            {
                "add",
                "--directory", @"C:\Projects\My App\Data\",
                "-n", "report final.txt"
            };

            object parsed = CommandLine.Parse(args, typeof(Add), typeof(Edit));
            var add = (Add)parsed;

            Assert.Multiple(() =>
            {
                Assert.That(add.Directory, Is.EqualTo(@"C:\Projects\My App\Data\"));
                Assert.That(add.Nombre, Is.EqualTo("report final.txt"));
            });
        }

        [Test]
        public void Parse_FlagOnNonBool_ThrowsWrongPropertyTypeException()
        {
            string[] args = { "--verbose" };

            var ex = Assert.Throws<WrongPropertyTypeException>(() =>
                CommandLine.Parse<Param_Flag_WrongPropertyType>(args));

            Assert.That(ex!.Message, Does.Contain("booleano"));
            Assert.That(ex.Message, Does.Contain("Verbose"));
        }

        [Test]
        public void Parse_OptionUnsupportedPropertyType_ThrowsParseValueException()
        {
            string[] args = { "--id", "{E6219258-4E2B-4F3A-9C1D-8A7B6C5D4E3F}" };

            var ex = Assert.Throws<ParseValueException>(() =>
                CommandLine.Parse<Param_Option_UnsupportedType>(args));

            Assert.That(ex!.Message, Does.Contain("Guid"));
            Assert.That(ex.Message, Does.Contain("no es soportado"));
        }

        [Test]
        public void Parse_DateTimeFormatterOnNonDateTimeProperty_ThrowsInvalidadPropertyTypeException()
        {
            string[] args = { "--when", "20240101" };

            var ex = Assert.Throws<InvalidadPropertyTypeException>(() =>
                CommandLine.Parse<Param_DateTimeFormatter_WrongPropertyType>(args));

            Assert.That(ex!.Message, Does.Contain("When"));
            Assert.That(ex.Message, Does.Contain("DateTime"));
        }

        [Test]
        public void Parse_Verbs_SkipsNullEntryInVerbTypesArray_UsesRemainingTypes()
        {
            string[] args = { "add", "--directory", @"C:\Temp\", "-n", "x.txt" };

            object parsed = CommandLine.Parse(args, null!, typeof(Add));

            Assert.That(parsed, Is.InstanceOf<Add>());
        }
    }
}
