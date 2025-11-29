using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using System.IO;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

namespace Test_CommandParser
{
    [TestFixture]
    public class ValidationAttributes_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region FileExists and DirectoryExists Tests

        [TestCase(@"--inputfile ..\Archivo.txt --outputdir ..\salida\")]
        public void Parse_FileAndDirectoryExists_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string inputFile = args[1];
            string outputDir = args[3];
            File.Create(inputFile).Close();
            Directory.CreateDirectory(outputDir);

            _ = CommandLine.Parse<Params_With_File_Dir_Exists>(args);

            File.Delete(inputFile);
            Directory.Delete(outputDir);

            Assert.Pass();
        }

        [TestCase(@"--inputfile ..\Archivo.txt --outputdir ..\qwerty2134s.sdssrWE\")]
        public void Parse_DirectoryExists_Throws_DirectoryNotExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            try { Directory.Delete(args[3]); }      //Por las dudas borro el directorio en caso de existir
            catch { }
            File.Create(args[1]).Close();           //Creo el input file para que no falle por eso.

            Assert.Throws<DirectoryNotExistsException>(() => CommandLine.Parse<Params_With_File_Dir_Exists>(args));

            File.Delete(args[1]);
        }

        [TestCase(@"--inputfile ..\1234asdcvft.jjj --outputdir ..\salida\")]
        public void Parse_FileExists_Throws_FileNotExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string outputDir = args[3];
            try { File.Delete(args[1]); }           //Por las dudas borro el archivo en caso de existir
            catch { }
            Directory.CreateDirectory(outputDir);   //Creo el dir para que no falle por eso.

            Assert.Throws<FileNotExistsException>(() => CommandLine.Parse<Params_With_File_Dir_Exists>(args));

            Directory.Delete(outputDir);
        }

        #endregion

        #region FileNotExists and DirectoryNotExists Tests

        [TestCase(@"--inputfile ..\12345qwer.txt --outputdir ..\carpetaqwer\")]
        public void Parse_FileAndDirectoryNOTExists_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string inputFile = args[1];
            string outputDir = args[3];

            try { File.Delete(inputFile); }           //Por las dudas borro el archivo en caso de existir
            catch { }

            try { Directory.Delete(outputDir); }      //Por las dudas borro la carpeta en caso de existir
            catch { }

            _ = CommandLine.Parse<Params_With_File_Dir_NotExists>(args);

            Assert.Pass();
        }

        [TestCase(@"--inputfile ..\salida.txt --outputdir ..\carpetaqwer\")]
        public void Parse_FileAndDirectoryNOTExists_Throws_FileAlreadyExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string inputFile = args[1];
            string outputDir = args[3];

            try { Directory.Delete(outputDir); }      //Por las dudas borro la carpeta en caso de existir
            catch { }

            File.Create(inputFile).Close();

            Assert.Throws<FileAlreadyExistsException>(() => CommandLine.Parse<Params_With_File_Dir_NotExists>(args));

            File.Delete(inputFile);
        }

        [TestCase(@"--inputfile ..\salida.txt --outputdir ..\carpetaqwer\")]
        public void Parse_FileAndDirectoryNOTExists_Throw_DirectoryAlreadyExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string inputFile = args[1];
            string outputDir = args[3];

            try { File.Delete(inputFile); }      //Por las dudas borro el archivo en caso de existir
            catch { }

            Directory.CreateDirectory(outputDir);

            Assert.Throws<DirectoryAlreadyExistsException>(() => CommandLine.Parse<Params_With_File_Dir_NotExists>(args));

            Directory.Delete(outputDir);
        }

        #endregion
    }
}

