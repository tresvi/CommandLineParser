using NUnit.Framework;
using Test_CommandParser.Models;
using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;

namespace Test_CommandParser
{
    [TestFixture]
    public class CommandLine_CoverageGaps_Test
    {
        [Test]
        public void Parse_SameKeyword_OnFlagAndOption_Throws_MultiDefinitionParameterException()
        {
            string[] args = { "--shared" };

            var ex = Assert.Throws<MultiDefinitionParameterException>(() =>
                CommandLine.Parse<Param_DuplicateKeyword_FlagAndOption>(args));

            Assert.That(ex!.Message, Does.Contain("shared"));
        }

        [Test]
        public void Parse_RequiresAttribute_ReferencesMissingProperty_Throws_RequiredParameterNotFoundException()
        {
            string[] args = { "--only", "value" };

            var ex = Assert.Throws<RequiredParameterNotFoundException>(() =>
                CommandLine.Parse<Param_Requires_UnknownProperty>(args));

            Assert.That(ex!.Message, Does.Contain("NoSuchPropertyOnThisClass").Or.Contain("no existe"));
        }
    }
}
