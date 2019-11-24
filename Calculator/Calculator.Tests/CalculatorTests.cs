using NUnit.Framework;
using System;

namespace Calculator.Tests
{
    [TestFixture]
    public class Tests
    {
        Calculator calc;

        [SetUp]
        public void Setup()
        {
            calc = new Calculator();
        }

        [TestCase("1,5000", 5001)]
        [TestCase("4,-3", 1)]
        [TestCase("20", 20)]
        [TestCase("-3,-4", -7)]
        [TestCase("5,tyty", 5)]
        [TestCase("6,6ytr7", 6)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase(",", 0)]
        public void Adding_Two_Numbers(string input, int output)
        {
            Assert.AreEqual(calc.AddNumbers(input), output);
        }

        [TestCase("5,6,7")]
        [TestCase(",,")]
        [TestCase(",8,")]
        public void Adding_More_Than_Two_Numbers(string input)
        {
            Assert.Throws<Exception>(() => calc.AddNumbers(input));
        }
    }
}