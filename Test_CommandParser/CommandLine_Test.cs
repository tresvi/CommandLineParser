using CommandParser;
using CommandParser.Attributtes.Keywords;
using CommandParser.Exceptions;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Test_CommandParser.Models;

namespace Test_CommandParser
{
    [TestFixture]
    public class CommandLine_Test
    {
        [SetUp]
        public void Setup()
        {
        }




        [TestCase($"--string EstoEsunString --datetime 20191229 --byte 250 --sbyte 120 --short 32000 --ushort 65000 " +
            "--int 2147483000 --uint 4294967000 --long 9223372036854775000 --ulong 18223372036854775000 " +
            $"--float 2.2 --double 2.3 --decimal 2.4 --bool true")]
        public void Parse_Input_AllDataTypes(string inputLine)
        {
            //Ajuste para cualquier configuracion regional de separador decimal
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            inputLine = inputLine.Replace(".", decimalSeparator); 

            Param_Multi_Type expectedResult = new();
            expectedResult.PropString = "EstoEsunString";
            expectedResult.PropDateTime = new DateTime(2019, 12, 29);
            expectedResult.PropByte = 250;
            expectedResult.PropSByte = 120;
            expectedResult.PropShort = 32000;
            expectedResult.PropUShort = 65000;
            expectedResult.PropInt = 2147483000;
            expectedResult.PropUInt = 4294967000;
            expectedResult.PropLong = 9223372036854775000;
            expectedResult.PropULong = 18223372036854775000;
            expectedResult.PropFloat = 2.2f;
            expectedResult.PropDouble = 2.3;
            expectedResult.PropDecimal = 2.4M;
            expectedResult.PropBool = true; 

            string[] args = inputLine.Split(' ');
            Param_Multi_Type parsedResult = CommandLine.Parse<Param_Multi_Type>(args);

            Assert.Multiple(() =>
            {
                foreach (PropertyInfo property in expectedResult.GetType().GetProperties())
                {
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        if (!(attribute is OptionAttribute)) continue;
                        Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
                    }
                }
            });
        }


        [TestCase($"--string EstoEsunString --datetime 20191229 --byte 250 --sbyte -120 --short -32000 --ushort 65000 " +
     "--int -2147483000 --uint 4294967000 --long -9223372036854775000 --ulong 18223372036854775000 " +
     $"--float -2.2 --double -2.3 --decimal -2.4 --bool true")]
        public void Parse_Input_NumberDataTypesNegatives(string inputLine)
        {
            //Ajuste para cualquier configuracion regional de separador decimal
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            inputLine = inputLine.Replace(".", decimalSeparator);

            Param_Multi_Type expectedResult = new();
            expectedResult.PropString = "EstoEsunString";
            expectedResult.PropDateTime = new DateTime(2019, 12, 29);
            expectedResult.PropByte = 250;
            expectedResult.PropSByte = -120;
            expectedResult.PropShort = -32000;
            expectedResult.PropUShort = 65000;
            expectedResult.PropInt = -2147483000;
            expectedResult.PropUInt = 4294967000;
            expectedResult.PropLong = -9223372036854775000;
            expectedResult.PropULong = 18223372036854775000;
            expectedResult.PropFloat = -2.2f;
            expectedResult.PropDouble = -2.3;
            expectedResult.PropDecimal = -2.4M;
            expectedResult.PropBool = true;

            string[] args = inputLine.Split(' ');
            Param_Multi_Type parsedResult = CommandLine.Parse<Param_Multi_Type>(args);

            Assert.Multiple(() =>
            {
                foreach (PropertyInfo property in expectedResult.GetType().GetProperties())
                {
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        if (!(attribute is OptionAttribute)) continue;
                        Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
                    }
                }
            });
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd --sungutrule hola chau otro", "--sungutrule")]
        [TestCase(@"--sungutrule --inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd hola chau otro", "--sungutrule")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --sungutrule --outputfile C:\Logs\ddd otro chau otro", "--sungutrule")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd otro chau otro", "otro")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt chau --outputfile C:\Logs\ddd otro", "chau")]
        public void Parse_Throw_UnknownParameterException(string inputLine, string? firstUnknonwParameter)
        {
            string[] args = inputLine.Split(' ');
            UnknownParameterException? exceptionDetalle = Assert.Throws<UnknownParameterException>(() => CommandLine.Parse<Parameters>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(firstUnknonwParameter));
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt", "--outputfile", "-o")]
        [TestCase(@"--outputfile C:\Logs\ddd", "--inputfile", "-i")]
        public void Parse_Throw_RequiredParameterNotFound(string inputLine, string missingArgument, string missingShortArgument)
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


        [TestCase(@"--inputfile C:\Logs\salida.txt", "outputfile")]    //Se toma "--oputputfile..." como valor de --inputfile
        [TestCase(@"--outputfile C:\Temp\Archivo.txt", "inputfile")]   //
        //TODO:  //Esta salta como NotValueSpecifiedException. Cambiar el parser para que arranque leyendo la CLI de izq a der. y que esta de ReqParameterNotFoundException
        //[TestCase(@"--outputfile --inputfile C:\Temp\Archivo.txt", "inputfile")]     //Esto tambien beneficiaria a la funcionalidad de verbo por default
        public void Parse_Input_2_Param_ReqParameterNotFoundException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            RequiredParameterNotFoundException? exceptionDetalle = Assert.Throws<RequiredParameterNotFoundException>(() => CommandLine.Parse<Parameters_2_Prop_Required>(args));

            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }


        //[TestCase(@"--inputfile --outputfile C:\Logs\salida.txt --name copia", "outputfile")]
        //[TestCase(@"--inputfile C:\Temp\Archivo.txt --name copia --outputfile", "outputfile")]
        public void Parse_Input_3_Param_ValueNotSpecifiedException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            ValueNotFoundException? exceptionDetalle = Assert.Throws<ValueNotFoundException>(() => CommandLine.Parse<Parameters_3_Prop_Required>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }

    }
}