using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        [TestCase("1,500", 501)]
        [TestCase("4, 6", 10)]
        [TestCase("20", 20)]
        [TestCase("3,4", 7)]
        [TestCase("5,tyty", 5)]
        [TestCase("6,6ytr7", 6)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase(",", 0)]
        [TestCase("5,6,7", 18)]
        [TestCase(",,",0)]
        [TestCase(",8,",8)]
        [TestCase("1,2,3,4,5,6,7,8,9,10,11,12", 78)]
        [TestCase("5,tyty,yu87i,9,100, 56", 170)]
        [TestCase("\n", 0)]
        [TestCase("\n,\nh", 0)]
        [TestCase("1\n2,3", 6)]
        [TestCase("5\n6", 11)]
        [TestCase("5\n6\nh4,a,3", 14)]
        [TestCase("5\n6\n2,3,10\n5,3", 34)]
        [TestCase(",,", 0)]
        [TestCase("1001", 0)]
        [TestCase(",1002", 0)]
        [TestCase(",1003,", 0)]
        [TestCase("2,1000,6", 1008)]
        [TestCase("2,1001,6", 8)]
        [TestCase("//#\n2#5", 7)]
        [TestCase("//#\n2#5#7,9", 23)]
        [TestCase("//.\n0.5.45,9", 59)]
        [TestCase("//\n\n4\n5\n9,10", 28)]
        [TestCase("//r\n1r3r8,6gghj,10r20", 42)]
        [TestCase("//[***]\n11***22***33", 66)]
        [TestCase("//[v,rt]\n11v,rt22v,rt33,9,\n12", 87)]
        [TestCase("//[aaa]\n0aaa5aaa45aaa9", 59)]
        [TestCase("//[|||||]\n4\n5|||||9|||||10,7\n3|||||8", 46)]
        [TestCase("//[!!][r9r]\n11r9r22\nhh,33!!44", 110)]
        [TestCase("//[*][!!][r9r]\n11r9r22*hh*33!!44", 110)]
        [TestCase("//[w][!!][r9r][abc][****]\n11r9r22****hh!!33\n44w1abc5,8", 124)]
        public void Adding_Two_Numbers(string input, int output)
        {
            Assert.AreEqual(calc.AddNumbers(input), output);
        }

        [TestCase("5,6,-7")]
        [TestCase(",-4,")]
        [TestCase(",-4,6,-7")]
        [TestCase("-1,4,-10,-20,-7")]
        public void Adding_One_Or_More_Negative_Numbers(string input)
        {
            var exception = Assert.Throws<Exception>(() => calc.AddNumbers(input));
            string[] numbers = input.Split(',');
            List<string> negativeNumbers = numbers.Where(x => Regex.IsMatch(x, @"[-]\d+$")).ToList();
            Assert.AreEqual(string.Format("Negative numbers '{0}' are not allowed", string.Join(",", negativeNumbers)).Trim(), exception.Message.Trim());
        }
    }
}