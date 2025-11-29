using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using Test_CommandParser.Models.Verbs;

namespace Test_CommandParser
{
    [TestFixture]
    public class CommandLine_T1_T2_WithVerbs_Test
    {
        static Add? _addTest;
        static Edit? _editTest;
        static Delete? _deleteTest;
        static Commit? _commitTest;

        [SetUp]
        public void Setup()
        {
            _addTest = new Add() { Directory = @"C:\Temp\", Nombre = "Salida.txt" };
            _editTest = new Edit() { File = @"C:\Temp\Salida.txt", FechaEdicion = new DateTime(2021, 12, 29) };
            _deleteTest = new Delete() { File = @"C:\Temp\Salida.txt" };
            _commitTest = new Commit() { File = @"C:\Temp\Salida.txt", Message = "InitialCommit" };
        }


        /// <summary>
        /// Verifies that the input line is parsed correctly and the expected verb is returned
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="expectedVerb"></param>
        /// <exception cref="Exception"></exception>
        [TestCase(@"add --directory C:\Temp\ -n Salida.txt", "add")]
        [TestCase(@"edit --file C:\Temp\Salida.txt --fecha 20211229", "edit")]
        public void Parse_InputDeclaratedVerbs(string inputLine, string expectedVerb)
        {
            string[] args = inputLine.Split(' ');
            object actualObject = CommandLine.Parse(args, typeof(Add), typeof(Edit));

            if (expectedVerb == "add")
            {
                Add addVerb = (Add)actualObject;
                Assert.AreEqual(_addTest?.Nombre, addVerb.Nombre);
                Assert.AreEqual(_addTest?.Directory, addVerb.Directory);
            }
            else if (expectedVerb == "edit")
            {
                Edit editVerb = (Edit)actualObject;
                Assert.AreEqual(_editTest?.File, editVerb.File);
                Assert.AreEqual(_editTest?.FechaEdicion, editVerb.FechaEdicion);
            }
            else
                throw new Exception($"Verbo \"{expectedVerb}\" desconocido, revise el caso de uso");
        }


        /// <summary>
        /// Verifies that a UnknownVerbException is thrown when attempting to parse a verb that is not declared
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="expectedVerb"></param>
        [TestCase(@"commit --directory C:\Temp\ -n Salida.txt", "commit")]
        [TestCase(@"delete --file C:\Temp\Salida.txt --fecha 20211229", "delete")]
        public void Parse_Throw_NotDeclaratedVerbs(string inputLine, string expectedVerb)
        {
            string[] args = inputLine.Split(' ');

            UnknownVerbException? exception = Assert.Throws<UnknownVerbException>(() => CommandLine.Parse(args, typeof(Add), typeof(Edit)));

            Assert.That(exception?.Message, Does.Contain(expectedVerb));
        }


        /// <summary>
        /// Verifies that a NotVerbClassException is thrown when attempting to parse a verb that is not declared
        /// </summary>
        /// <param name="inputLine"></param>
        [TestCase(@"add --directory C:\Temp\ -n Salida.txt")]
        public void Parse_Throw_NotVerbClass(string inputLine)
        {
            string[] args = inputLine.Split(' ');

            NotVerbClassException? exception = Assert.Throws<NotVerbClassException>(() => CommandLine.Parse(args, typeof(Add), typeof(NotVerbClass)));

            Assert.That(exception?.Message, Does.Contain(typeof(NotVerbClass).Name));
        }


        /// <summary>
        /// Verifies that the input line is parsed correctly with 4 verbs and the expected verb is returned
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="expectedVerb"></param>
        /// <exception cref="Exception"></exception>
        [TestCase(@"add --directory C:\Temp\ -n Salida.txt", "add")]
        [TestCase(@"edit --file C:\Temp\Salida.txt --fecha 20211229", "edit")]
        [TestCase(@"delete --file C:\Temp\Salida.txt", "delete")]
        [TestCase(@"commit --file C:\Temp\Salida.txt -m InitialCommit", "commit")]
        public void Parse_InputDeclaratedVerbs_WithFourVerbs(string inputLine, string expectedVerb)
        {
            string[] args = inputLine.Split(' ');
            object actualObject = CommandLine.Parse(args, typeof(Add), typeof(Edit), typeof(Delete), typeof(Commit));

            if (expectedVerb == "add")
            {
                Add addVerb = (Add)actualObject;
                Assert.AreEqual(_addTest?.Nombre, addVerb.Nombre);
                Assert.AreEqual(_addTest?.Directory, addVerb.Directory);
            }
            else if (expectedVerb == "edit")
            {
                Edit editVerb = (Edit)actualObject;
                Assert.AreEqual(_editTest?.File, editVerb.File);
                Assert.AreEqual(_editTest?.FechaEdicion, editVerb.FechaEdicion);
            }
            else if (expectedVerb == "delete")
            {
                Delete deleteVerb = (Delete)actualObject;
                Assert.AreEqual(_deleteTest?.File, deleteVerb.File);
            }
            else if (expectedVerb == "commit")
            {
                Commit commitVerb = (Commit)actualObject;
                Assert.AreEqual(_commitTest?.File, commitVerb.File);
                Assert.AreEqual(_commitTest?.Message, commitVerb.Message);
            }
            else
                throw new Exception($"Verbo \"{expectedVerb}\" desconocido, revise el caso de uso");
        }

        [Test]
        public void GetHelpTextForVerbs_WithTwoVerbs_ContainsAllVerbsAndParameters()
        {
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Add), typeof(Edit));

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("Verbos disponibles:"));
                Assert.That(helpText, Does.Contain("add"));
                Assert.That(helpText, Does.Contain("Agrega una instancia"));
                Assert.That(helpText, Does.Contain("--directory"));
                Assert.That(helpText, Does.Contain("-d"));
                Assert.That(helpText, Does.Contain("Carpeta donde se creará el archivo"));
                Assert.That(helpText, Does.Contain("--name"));
                Assert.That(helpText, Does.Contain("-n"));
                Assert.That(helpText, Does.Contain("Nombre del archivo a crear con su extensión"));
                Assert.That(helpText, Does.Contain("edit"));
                Assert.That(helpText, Does.Contain("Comando para editar"));
                Assert.That(helpText, Does.Contain("--file"));
                Assert.That(helpText, Does.Contain("-f"));
                Assert.That(helpText, Does.Contain("Ruta del archivo a editar"));
                Assert.That(helpText, Does.Contain("--fecha"));
                Assert.That(helpText, Does.Contain("-F"));
                Assert.That(helpText, Does.Contain("--help | -h | /?"));
            });
        }

        [Test]
        public void GetHelpTextForVerbs_WithFourVerbs_ContainsAllVerbs()
        {
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Add), typeof(Edit), typeof(Delete), typeof(Commit));

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("add"));
                Assert.That(helpText, Does.Contain("edit"));
                Assert.That(helpText, Does.Contain("delete"));
                Assert.That(helpText, Does.Contain("commit"));
            });
        }

        [Test]
        public void GetHelpTextForVerbs_WithRequiredParameters_ShowsRequiredText()
        {
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Add));

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("(Requerido)"));
                // Verificar que aparece junto a los parámetros requeridos
                int requiredIndex = helpText.IndexOf("(Requerido)");
                int directoryIndex = helpText.IndexOf("--directory");
                int nameIndex = helpText.IndexOf("--name");
                
                Assert.That(requiredIndex, Is.GreaterThan(-1), "Debe contener '(Requerido)'");
                Assert.That(directoryIndex, Is.GreaterThan(-1), "Debe contener '--directory'");
                Assert.That(nameIndex, Is.GreaterThan(-1), "Debe contener '--name'");
            });
        }

        [Test]
        public void GetHelpTextForVerbs_WithMultipleVerbs_ShowsCorrectFormatting()
        {
            string helpText = CommandLine.GetHelpTextForVerbs(typeof(Add), typeof(Edit));

            // Verificar que cada verbo tiene su sección con indentación correcta
            int addIndex = helpText.IndexOf("add");
            int editIndex = helpText.IndexOf("edit");
            
            Assert.Multiple(() =>
            {
                Assert.That(addIndex, Is.GreaterThan(-1), "Debe contener 'add'");
                Assert.That(editIndex, Is.GreaterThan(-1), "Debe contener 'edit'");
                Assert.That(editIndex, Is.GreaterThan(addIndex), "edit debe aparecer después de add");
            });
        }

    }
}