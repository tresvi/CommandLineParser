using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

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

        [TestCase(@"--outputfile C:\Logs\out.txt -o C:\Logs\out2.txt", "outputfile")]
        [TestCase(@"-o C:\Logs\out.txt --outputfile C:\Logs\out2.txt", "outputfile")]
        [TestCase(@"--inputfile C:\Logs\in.txt -i C:\Logs\in2.txt", "inputfile")]
        [TestCase(@"-i C:\Logs\in.txt --inputfile C:\Logs\in2.txt", "inputfile")]
        [TestCase(@"--outputfile C:\Logs\out.txt --outputfile C:\Logs\out2.txt", "outputfile")]
        [TestCase(@"-o C:\Logs\out.txt -o C:\Logs\out2.txt", "outputfile")]
        public void Parse_MixedLongAndShortKeyword_Throws_MultiInvocationParameter(string inputLine, string repeatedParameter)
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

        [Test]
        public void GetHelpText_WithoutVerbs_ContainsAllParameters()
        {
            Parameters targetObject = new Parameters();
            string helpText = CommandLine.GetHelpText(targetObject);

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("Comandos:"));
                Assert.That(helpText, Does.Contain("--inputfile"));
                Assert.That(helpText, Does.Contain("-i"));
                Assert.That(helpText, Does.Contain("Archivo de entrada a ser procesado."));
                Assert.That(helpText, Does.Contain("--outputfile"));
                Assert.That(helpText, Does.Contain("-o"));
                Assert.That(helpText, Does.Contain("Archivo de salida resultante del procesamiento."));
                Assert.That(helpText, Does.Contain("--help | -h | /?"));
            });
        }

        [Test]
        public void GetHelpText_WithFlags_ContainsFlags()
        {
            Parameter_With_Flag targetObject = new Parameter_With_Flag();
            string helpText = CommandLine.GetHelpText(targetObject);

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("--sendmail"));
                Assert.That(helpText, Does.Contain("-s"));
                Assert.That(helpText, Does.Contain("Indica si debe notificar por mail"));
            });
        }

        [Test]
        public void GetHelpText_WithRequiredAndOptional_ShowsAllParameters()
        {
            Param_2_Prop_Req_1_Prop_NoReq targetObject = new Param_2_Prop_Req_1_Prop_NoReq();
            string helpText = CommandLine.GetHelpText(targetObject);

            Assert.Multiple(() =>
            {
                Assert.That(helpText, Does.Contain("--inputfile"));
                Assert.That(helpText, Does.Contain("--outputfile"));
                Assert.That(helpText, Does.Contain("--name"));
            });
        }

        #region High Priority Coverage Tests

        /// <summary>
        /// Test para cubrir la rama donde ShortKeyword está en keywordsAlreadyFound pero Keyword no (línea 51).
        /// Caso: Se usa -o primero (ShortKeyword de outputfile), luego se intenta usar --outputfile.
        /// Debe detectar que -o ya fue usado y lanzar MultiInvocationParameterException.
        /// </summary>
        [TestCase(@"-o file1.txt --outputfile file2.txt")]
        public void Parse_ShortKeywordFirstThenLongKeyword_Throws_MultiInvocationParameter(string inputLine)
        {
            string[] args = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            MultiInvocationParameterException? exception = Assert.Throws<MultiInvocationParameterException>(
                () => CommandLine.Parse<Parameter_With_Flag>(args));

            Assert.That(exception?.Message, Does.Contain("equivalente"));
            Assert.That(exception?.Message, Does.Contain("-o"));
        }

        /// <summary>
        /// Test para cubrir la rama donde matchCounter > 1 en FindMatchKeywordVsAttribute (línea 98-99).
        /// Esto ocurre cuando el mismo keyword se encuentra en múltiples propiedades.
        /// Aunque CheckForDuplicatedKeywordInClass debería detectarlo primero, este test verifica
        /// que la rama esté cubierta.
        /// </summary>
        [Test]
        public void Parse_MultipleProperties_SameKeyword_Throws_MultiDefinitionParameter()
        {
            string inputLine = @"--test value1";
            string[] args = inputLine.Split(' ');
            
            MultiDefinitionParameterException? exception = Assert.Throws<MultiDefinitionParameterException>(
                () => CommandLine.Parse<Param_MultipleAttributes_SameKeyword>(args));

            Assert.That(exception?.Message, Does.Contain("test"));
            Assert.That(exception?.Message, Does.Contain("No se puede mapear la misma palabra en mas de una property"));
        }

        /// <summary>
        /// Test para cubrir GetDefinedParameters<T>() que actualmente tiene 0% coverage.
        /// Verifica que el método retorne correctamente todas las keywords y shortKeywords definidas.
        /// </summary>
        [Test]
        public void GetDefinedParameters_WithOptions_ReturnsAllKeywords()
        {
            List<string> definedParams = CommandLine.GetDefinedParameters<Parameters>();

            Assert.Multiple(() =>
            {
                Assert.That(definedParams, Does.Contain("--inputfile"));
                Assert.That(definedParams, Does.Contain("-i"));
                Assert.That(definedParams, Does.Contain("--outputfile"));
                Assert.That(definedParams, Does.Contain("-o"));
                Assert.That(definedParams.Count, Is.EqualTo(4));
            });
        }

        /// <summary>
        /// Test para GetDefinedParameters con Flags.
        /// </summary>
        [Test]
        public void GetDefinedParameters_WithFlags_ReturnsAllKeywords()
        {
            List<string> definedParams = CommandLine.GetDefinedParameters<Parameter_With_Flag>();

            Assert.Multiple(() =>
            {
                Assert.That(definedParams, Does.Contain("--inputfile"));
                Assert.That(definedParams, Does.Contain("-i"));
                Assert.That(definedParams, Does.Contain("--outputfile"));
                Assert.That(definedParams, Does.Contain("-o"));
                Assert.That(definedParams, Does.Contain("--sendmail"));
                Assert.That(definedParams, Does.Contain("-s"));
                Assert.That(definedParams.Count, Is.EqualTo(6));
            });
        }

        /// <summary>
        /// Test para GetDefinedParameters con clase vacía (sin propiedades con atributos).
        /// </summary>
        [Test]
        public void GetDefinedParameters_EmptyClass_ReturnsEmptyList()
        {
            List<string> definedParams = CommandLine.GetDefinedParameters<Param_Empty>();

            Assert.That(definedParams, Is.Empty);
        }

        /// <summary>
        /// Test para GetDefinedParameters con múltiples tipos de atributos.
        /// </summary>
        [Test]
        public void GetDefinedParameters_MultipleTypes_ReturnsAllKeywords()
        {
            List<string> definedParams = CommandLine.GetDefinedParameters<Param_Multi_Type>();

            Assert.Multiple(() =>
            {
                Assert.That(definedParams, Does.Contain("--string"));
                Assert.That(definedParams, Does.Contain("-x"));
                Assert.That(definedParams, Does.Contain("--datetime"));
                Assert.That(definedParams, Does.Contain("-d"));
                Assert.That(definedParams.Count, Is.GreaterThan(20)); // Múltiples propiedades
            });
        }

        #endregion

        
    }
}