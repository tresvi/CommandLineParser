using NUnit.Framework;
using System;
using Tresvi.CommandParser;
using Tresvi.CommandLineParser.Test.Unit.Models;
using System.Linq;

namespace Tresvi.CommandLineParser.Test.Unit
{
    [TestFixture]
    public class RepeatableOption_Test
    {
        [Test]
        public void Parse_RepeatableDictionary_Accumulates()
        {
            var r = CommandLine.Parse<Params_Repeatable_Dictionary>(new[]
            {
                "--define", "a=1",
                "-d", "b=2",
                "--define", "c=3"
            });

            Assert.That(r.Puertos.Count, Is.EqualTo(3));
            Assert.That(r.Puertos["a"], Is.EqualTo(1));
            Assert.That(r.Puertos["b"], Is.EqualTo(2));
            Assert.That(r.Puertos["c"], Is.EqualTo(3));
        }

        [Test]
        public void Parse_RepeatableDictionary_LastWinsSameKey()
        {
            var r = CommandLine.Parse<Params_Repeatable_Dictionary>(new[]
            {
                "-d", "a=1",
                "-d", "a=99"
            });

            Assert.That(r.Puertos.Count, Is.EqualTo(1));
            Assert.That(r.Puertos["a"], Is.EqualTo(99));
        }

        [Test]
        public void Parse_RepeatableList_Accumulates()
        {
            var r = CommandLine.Parse<Params_Repeatable_Sequence>(new[]
            {
                "-n", "10",
                "--numero", "20",
                "-n", "30",
                "-t", "uno",
                "--tag", "dos"
            });

            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, r.Numeros);
            Assert.That(r.Tags.ToList(), Is.EqualTo(new[] { "uno", "dos" }).AsCollection);
        }

        [Test]
        public void Parse_RepeatableIntArray_Accumulates()
        {
            var r = CommandLine.Parse<Params_Repeatable_IntArray>(new[]
            {
                "-i", "1",
                "-i", "2",
                "-i", "3"
            });

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, r.Ids);
        }

        [Test]
        public void Parse_AllowMultiple_OnScalar_Throws()
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommandLine.Parse<Params_Repeatable_InvalidScalar>(new[] { "-x", "1" }));
        }
    }
}
