using CommandParser;
using CommandParser.Exceptions;
using NUnit.Framework;
using System;
using System.IO;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    [TestFixture]
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
            RequiredParameterNotFoundException? exceptionDetalle = Assert.Throws<RequiredParameterNotFoundException>(() => CommandLine.Parse<Parameters>(args));

            Assert.Multiple(() =>
            {
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingArgument));
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingShortArgument));
            });
        }


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
        public void Parse_DirectoryExists_Throw_DirectoryNotExists(string inputLine)
        { 
            string[] args = inputLine.Split(' ');
            try { Directory.Delete(args[3]); }      //Por las dudas borro el directorio en caso de existir
            catch { }
            File.Create(args[1]).Close();           //Creo el input file para que no falle por eso.

            Assert.Throws<DirectoryNotExistsException>(() => CommandLine.Parse<Params_With_File_Dir_Exists>(args));

            File.Delete(args[1]);
        }


        [TestCase(@"--inputfile ..\1234asdcvft.jjj --outputdir ..\salida\")]
        public void Parse_FileExists_Throw_FileNotExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string outputDir = args[3];
            try { File.Delete(args[1]); }           //Por las dudas borro el archivo en caso de existir
            catch { }
            Directory.CreateDirectory(outputDir);   //Creo el dir para que no falle por eso.

            Assert.Throws<FileNotExistsException>(() => CommandLine.Parse<Params_With_File_Dir_Exists>(args));

            Directory.Delete(outputDir);
        }


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
        public void Parse_FileAndDirectoryNOTExists_Throw_FileAlreadyExists(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            string inputFile = args[1];
            string outputDir = args[3];

            try { Directory.Delete(outputDir); }      //Por las dudas borro la carpeta en caso de existir
            catch { }

            File.Create(inputFile).Close();

            Assert.Throws<FileAlreadyExistsException>(() => CommandLine.Parse<Params_With_File_Dir_NotExists>(args));
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
        }


        [TestCase(@"--inputfile --outputfile C:\Logs\salida.txt", "inputfile")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile", "outputfile")]
        public void Parse_Input_2_Param_ArgumentNotFoundException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            ValueNotSpecifiedException? exceptionDetalle = Assert.Throws<ValueNotSpecifiedException>(() => CommandLine.Parse<Parameters_2_Prop_Required>(args));

            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }

        [TestCase(@"--inputfile --outputfile C:\Logs\salida.txt --name copia", "inputfile")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile --name copia", "outputfile")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\salida.txt --name", "name")]
        public void Parse_Input_3_Param_ArgumentNotFoundException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            ValueNotSpecifiedException? exceptionDetalle = Assert.Throws<ValueNotSpecifiedException>(() => CommandLine.Parse<Parameters_3_Prop_Required>(args));

            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }
    }
}