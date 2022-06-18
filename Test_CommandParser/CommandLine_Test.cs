using CommandParser;
using CommandParser.Exceptions;
using NUnit.Framework;
using System;
using System.IO;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    public class CommandLine_Test
    {
        public void Setup()
        {
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd --sungutrule hola chau otro")]
        [TestCase(@"--sungutrule --inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd hola chau otro")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --sungutrule --outputfile C:\Logs\ddd hola chau otro")]
        public void Parse_InputNotDeclaratedCommands(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            ArgumentException? exceptionDetalle = Assert.Throws<ArgumentException>(() => CommandLine.Parse<Parameters>(args));
            Assert.AreEqual("Se proporcionaron parametros desconocidos: --sungutrule & hola & chau & otro", exceptionDetalle?.Message);
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt", "--outputfile", "-o")]
        [TestCase(@"--outputfile C:\Logs\ddd", "--inputfile", "-i")]
        public void Parse_ArgumentNotFound(string inputLine, string missingArgument, string missingShortArgument)
        {
            string[] args = inputLine.Split(' ');
            ArgumentNotFoundException? exceptionDetalle = Assert.Throws<ArgumentNotFoundException>(() => CommandLine.Parse<Parameters>(args));

            Assert.Multiple(() =>
            {
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingArgument));
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingShortArgument));
            });
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\dddaaaaaa")]
        public void Parse_DirectoryNotExistsException(string inputLine)
        {
            inputLine += Path.GetRandomFileName();
            string[] args = inputLine.Split(' ');

            Assert.Throws<DirectoryNotFoundException>(() => CommandLine.Parse<Parameters>(args));
        }


        [TestCase(@"--outputfile C:\Logs\dddaaaaaa")]
        public void Parse_DirectoryExistsException(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Directory.CreateDirectory(args[1]);
            Assert.Throws<DirectoryAlreadyExistsException>(() => CommandLine.Parse<Param_OutFileNotExists>(args));
            Directory.Delete(args[1]);
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile", "outputfile")]
        [TestCase(@"--inputfile --outputfile C:\Logs\ddd", "inputfile")]
        public void Parse_ValueNotFoundException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            ValueNotFoundException? exceptionDetalle = Assert.Throws<ValueNotFoundException>(() => CommandLine.Parse<Parameters>(args));

            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }
    }
}