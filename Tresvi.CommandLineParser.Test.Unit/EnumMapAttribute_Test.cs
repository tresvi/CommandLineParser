using System;
using NUnit.Framework;
using Tresvi.CommandParser.Attributes.Validation;

namespace Test_CommandParser
{
    internal enum EnumMapAttributeTestEnum
    {
        East,
        West
    }

    [TestFixture]
    public class EnumMapAttribute_Test
    {
        [Test]
        public void EnumMapAttribute_ctor_null_or_whitespace_input_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new EnumMapAttribute(null!, EnumMapAttributeTestEnum.East));
            Assert.Throws<ArgumentException>(() => new EnumMapAttribute(string.Empty, EnumMapAttributeTestEnum.East));
            Assert.Throws<ArgumentException>(() => new EnumMapAttribute("   ", EnumMapAttributeTestEnum.East));
        }

        [Test]
        public void EnumMapAttribute_ctor_null_enum_value_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EnumMapAttribute("alias", null!));
        }
    }
}
