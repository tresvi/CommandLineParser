using CommandParser;
using CommandParser.Exceptions;
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

        [SetUp]
        public void Setup()
        {
            _addTest = new Add() { Directory = @"C:\Temp\", Nombre = "Salida.txt" };
            _editTest = new Edit() { File = @"C:\Temp\Salida.txt", FechaEdicion = new DateTime(2021, 12, 29) };
        }


        [Ignore("Ignore a fixture")]
        [TestCase(@"add --directory C:\Temp\ -n Salida.txt", "add")]
        [TestCase(@"edit --file C:\Temp\Salida.txt --fecha 20211229", "edit")]
        public void Parse_InputDeclaratedVerbs(string inputLine, string expectedVerb)
        {
            string[] args = inputLine.Split(' ');
            object actualObject = CommandLine.Parse<Add, Edit>(args);

            if (expectedVerb == "add")
            {
                Assert.AreEqual(_addTest?.Nombre, ((Add)actualObject).Nombre);
                Assert.AreEqual(_addTest?.Directory, ((Add)actualObject).Directory);
            }
            else if (expectedVerb == "edit")
            {
                Assert.AreEqual(_editTest?.File, ((Edit)actualObject).File);
                Assert.AreEqual(_editTest?.FechaEdicion, ((Edit)actualObject).FechaEdicion);
            }
            else
                throw new Exception($"Verbo \"{expectedVerb}\" desconocido, revise el caso de uso");
        }


        [Ignore("Ignore a fixture")]
        [TestCase(@"commit --directory C:\Temp\ -n Salida.txt", "commit")]
        [TestCase(@"delete --file C:\Temp\Salida.txt --fecha 20211229", "delete")]
        public void Parse_Throw_NotDeclaratedVerbs(string inputLine, string expectedVerb)
        {
            string[] args = inputLine.Split(' ');

            UnknownVerbException? exception = Assert.Throws<UnknownVerbException>(() => CommandLine.Parse<Add, Edit>(args));

            Assert.That(exception?.Message, Does.Contain(expectedVerb));
        }


        [Ignore("Ignore a fixture")]
        [TestCase(@"add --directory C:\Temp\ -n Salida.txt")]
        public void Parse_Throw_NotVerbClass(string inputLine)
        {
            string[] args = inputLine.Split(' ');

            NotVerbClassException? exception = Assert.Throws<NotVerbClassException>(() => CommandLine.Parse<Add, NotVerbClass>(args));

            Assert.That(exception?.Message, Does.Contain(typeof(NotVerbClass).Name));
        }

    }
}