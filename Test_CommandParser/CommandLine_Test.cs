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
                    Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
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
                    Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
                }
            });
        }


        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd --sungutrule hola chau otro", "--sungutrule")]
        [TestCase(@"--sungutrule --inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd hola chau otro", "--sungutrule")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --sungutrule --outputfile C:\Logs\ddd otro chau otro", "--sungutrule")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --outputfile C:\Logs\ddd otro chau otro", "otro")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt chau --outputfile C:\Logs\ddd otro", "chau")]
        public void Parse_Throws_UnknownParameterException(string inputLine, string? firstUnknonwParameter)
        {
            string[] args = inputLine.Split(' ');
            UnknownParameterException? exceptionDetalle = Assert.Throws<UnknownParameterException>(() => CommandLine.Parse<Parameters>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(firstUnknonwParameter));
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


        [TestCase(@"--inputfile C:\Temp\Archivo.txt", "--outputfile", "-o")]
        [TestCase(@"--outputfile C:\Logs\ddd", "--inputfile", "-i")]
        [TestCase(@"--name proc1 --outputfile C:\Logs\ddd", "--inputfile", "-i")]
        [TestCase(@"--outputfile C:\Logs\ddd --name proc1", "--inputfile", "-i")]
        public void Parse_Input_2_Param_Throws_RequiredParameterNotFound(string inputLine, string missingArgument, string missingShortArgument)
        {
            string[] args = inputLine.Split(' ');
            RequiredParameterNotFoundException? exceptionDetalle = Assert.Throws<RequiredParameterNotFoundException>(() => CommandLine.Parse<Param_2_Prop_Req_1_Prop_NoReq>(args));

            Assert.Multiple(() =>
            {
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingArgument));
                Assert.That(exceptionDetalle?.Message, Does.Contain(missingShortArgument));
            });
        }



        [TestCase(@"--inputfile C:\Logs\salida.txt --outputfile", "outputfile")]
        [TestCase(@"--inputfile C:\Temp\Archivo.txt --name copia --outputfile", "outputfile")]
        public void Parse_Input_3_Param_Throws_ValueNotSpecifiedException(string inputLine, string parametroFaltante)
        {
            string[] args = inputLine.Split(' ');
            ValueNotFoundException? exceptionDetalle = Assert.Throws<ValueNotFoundException>(() => CommandLine.Parse<Parameters_3_Prop_Required>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(parametroFaltante));
        }

        [TestCase(@"--sendmail --inputfile C:\Temp\in.txt --outputfile C:\Temp\out.txt")]
        [TestCase(@"--inputfile C:\Temp\in.txt --sendmail --outputfile C:\Temp\out.txt")]
        [TestCase(@"--inputfile C:\Temp\in.txt --outputfile C:\Temp\out.txt --sendmail")]
        [TestCase(@"--inputfile C:\Temp\in.txt --outputfile C:\Temp\out.txt")]
        public void Parse_Input_ParamWithFlag(string inputLine)
        {
            Parameter_With_Flag expectedResult = new Parameter_With_Flag();
            expectedResult.InputFile = @"C:\Temp\in.txt";
            expectedResult.OutputFile = @"C:\Temp\out.txt";
            expectedResult.SendMail = inputLine.Contains("--sendmail");

            string[] args = inputLine.Split(' ');
            Parameter_With_Flag parsedResult = CommandLine.Parse<Parameter_With_Flag>(args);

            Assert.Multiple(() =>
            {
                foreach (PropertyInfo property in expectedResult.GetType().GetProperties())
                {
                    Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
                }
            });
        }


        [TestCase(@"--inputfile C:\Logs\in.txt --outputfile C:\Logs\out.txt --inputfile C:\Logs\in2.txt", "inputfile")]
        [TestCase(@"--inputfile C:\Logs\in.txt --sendmail --outputfile C:\Logs\out.txt --inputfile C:\Logs\in2.txt", "inputfile")]
        public void Parse_Input_2_Param_Throws_MultiInvocationParameter(string inputLine, string repeatedParameter)
        {
            string[] args = inputLine.Split(' ');
            MultiInvocationParameterException? exceptionDetalle = Assert.Throws<MultiInvocationParameterException>(() => CommandLine.Parse<Parameter_With_Flag>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(repeatedParameter));
        }


        [TestCase(@"--fecha-inicial 19811229 --fecha-final 206710")]
        [TestCase(@"--fecha-inicial 19811229 -F 206710")]
        [TestCase(@"-f 19811229 -F 206710")]
        [TestCase(@"--fecha-final 206710 --fecha-inicial 19811229")]
        [TestCase(@"--fecha-final 206710 -f 19811229")]
        [TestCase(@"-F 206710 -f 19811229")]
        public void Parse_Input_2_Dates_WithFormat(string inputLine)
        {
            Param_With_Format expectedResult = new Param_With_Format();
            expectedResult.FechaInicio = new DateTime(1981, 12, 29);
            expectedResult.FechaFinal = new DateTime(2067, 10, 01);
            string[] args = inputLine.Split(' ');
            Param_With_Format parsedResult = CommandLine.Parse<Param_With_Format>(args);

            Assert.Multiple(() =>
            {
                foreach (PropertyInfo property in expectedResult.GetType().GetProperties())
                {
                    Assert.AreEqual(property.GetValue(expectedResult), property.GetValue(parsedResult));
                }
            });
        }


        [TestCase(@"--fecha-inicial 19811229 --fecha-final 20671012", "fecha-final")]
        [TestCase(@"--fecha-inicial 198112 -F 206710", "fecha-inicial")]
        public void Parse_Input_2_Dates_Throws_InvalidFormatException(string inputLine, string failedParameter)
        {
            string[] args = inputLine.Split(' ');
            InvalidFormatException? exceptionDetalle = Assert.Throws<InvalidFormatException>(() => CommandLine.Parse<Param_With_Format>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain(failedParameter));
            Assert.That(exceptionDetalle?.Message, Does.Contain("fecha"));
        }


        [Test]
        public void Parse_Input_2_Dates_Throws_MultiDefinitionParameter()
        {
            string inputLine = $"--string EstoEsunString --datetime 20191229 --byte 250 --sbyte 120";
            string[] args = inputLine.Split(' ');
            MultiDefinitionParameterException? exceptionDetalle = Assert.Throws<MultiDefinitionParameterException>(() => CommandLine.Parse<Param_Repited_ShortKeyword>(args));
            Assert.That(exceptionDetalle?.Message, Does.Contain("-s"));

            inputLine = $"--string EstoEsunString --datetime 20191229 --byte 250 --sbyte 120";
            args = inputLine.Split(' ');
            MultiDefinitionParameterException? exceptionDetalle2 = Assert.Throws<MultiDefinitionParameterException>(() => CommandLine.Parse<Param_Repited_Keyword>(args));
            Assert.That(exceptionDetalle2?.Message, Does.Contain("byte"));
        }

        
    }
}