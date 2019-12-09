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
        Calculator calc1;
        Calculator calc2;
        Calculator calc3;

        [SetUp]
        public void Setup()
        {
            calc = new Calculator('\n', false, 1000);
            calc1 = new Calculator('g', false, 1000);
            calc2 = new Calculator('\n', true, 1000);
            calc3 = new Calculator('\n', false, null);
        }

        [TestCase("1,500", "1+500 = 501")]
        [TestCase("4,6 ", "4+6 = 10")]
        [TestCase("20", "20 = 20")]
        [TestCase("3,4", "3+4 = 7")]
        [TestCase("5,tyty", "5+0 = 5")]
        [TestCase("6,6ytr7", "6+0 = 6")]
        [TestCase("", "0")]
        [TestCase(null, "0")]
        [TestCase(",", "0+0 = 0")]
        [TestCase("5,6,7", "5+6+7 = 18")]
        [TestCase(",,", "0+0+0 = 0")]
        [TestCase(",8,", "0+8+0 = 8")]
        [TestCase("1,2,3,4,5,6,7,8,9,10,11,12", "1+2+3+4+5+6+7+8+9+10+11+12 = 78")]
        [TestCase("5,tyty,yu87i,9,100, 56", "5+0+0+9+100+56 = 170")]
        [TestCase("\n", "0+0 = 0")]
        [TestCase("\n,\nh", "0+0+0+0 = 0")]
        [TestCase("1\n2,3", "1+2+3 = 6")]
        [TestCase("5\n6", "5+6 = 11")]
        [TestCase("5\n6\nh4,a,3", "5+6+0+0+3 = 14")]
        [TestCase("5\n6\n2,3,10\n5,3", "5+6+2+3+10+5+3 = 34")]
        [TestCase(",,", "0+0+0 = 0")]
        [TestCase("1001", "0 = 0")]
        [TestCase(",1002", "0+0 = 0")]
        [TestCase(",1003,", "0+0+0 = 0")]
        [TestCase("2,1000,6", "2+1000+6 = 1008")]
        [TestCase("2,1001,6", "2+0+6 = 8")]
        [TestCase("//#\n2#5", "2+5 = 7")]
        [TestCase("//#\n2#5#7,9", "2+5+7+9 = 23")]
        [TestCase("//.\n0.5.45,9", "0+5+45+9 = 59")]
        [TestCase("//r\n1r3r8,6gghj,10r20", "1+3+8+0+10+20 = 42")]
        [TestCase("//[***]\n11***22***33", "11+22+33 = 66")]
        [TestCase("//[v,rt]\n11v,rt22v,rt33,9,\n12", "11+22+33+9+0+12 = 87")]
        [TestCase("//[aaa]\n0aaa5aaa45aaa9", "0+5+45+9 = 59")]
        [TestCase("//[|||||]\n4\n5|||||9|||||10,7\n3|||||8", "4+5+9+10+7+3+8 = 46")]
        [TestCase("//[!!][r9r]\n11r9r22\nhh,33!!44", "11+22+0+33+44 = 110")]
        [TestCase("//[*][!!][r9r]\n11r9r22*hh*33!!44", "11+22+0+33+44 = 110")]
        [TestCase("//[w][!!][r9r][abc][****]\n11r9r22****hh!!33\n44w1abc5,8", "11+22+0+33+44+1+5+8 = 124")]
        public void Adding_Two_Numbers(string input, string output)
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

        [TestCase("1,500g6", "1+500+6 = 507")]
        public void Adding_Two_Numbers_With_AlternateDelimiter(string input, string output)
        {
            Assert.AreEqual(calc1.AddNumbers(input), output);
        }

        [TestCase("1,500,-400", "1+500+(-400) = 101")]
        [TestCase("1,-1", "1+(-1) = 0")]
        public void Adding_Two_Numbers_With_NegativeNumbers(string input, string output)
        {
            Assert.AreEqual(calc2.AddNumbers(input), output);
        }

        [TestCase("1,5000", "1+5000 = 5001")]
        [TestCase("1,6000", "1+6000 = 6001")]
        public void Adding_Two_Numbers_With_UpperBound(string input, string output)
        {
            Assert.AreEqual(calc3.AddNumbers(input), output);
        }

        [TestCase("20", "20 = 20")]
        [TestCase("3,4", "3-4 = -1")]
        [TestCase("3,-4", "3-(-4) = 7")]
        [TestCase("5,tyty", "5-0 = 5")]
        [TestCase("5\n6", "5-6 = -1")]
        [TestCase("//#\n2#5", "2-5 = -3")]
        [TestCase("//#\n2#5#7,9", "2-5-7-9 = -19")]
        [TestCase("//[|||||]\n4\n5|||||9|||||10,7\n3|||||8", "4-5-9-10-7-3-8 = -38")]
        public void Subtracting_Two_Numbers(string input, string output)
        {
            Assert.AreEqual(calc2.SubtractNumbers(input), output);
        }

        [TestCase("20", "20 = 20")]
        [TestCase("3,4", "3*4 = 12")]
        [TestCase("3,-4", "3*(-4) = -12")]
        [TestCase("-3,-4", "-3*(-4) = 12")]
        [TestCase("5,tyty", "5*0 = 0")]
        [TestCase("5\n6", "5*6 = 30")]
        [TestCase("//#\n2#5", "2*5 = 10")]
        [TestCase("//#\n2#5#7,9", "2*5*7*9 = 630")]
        [TestCase("//[|||||]\n4\n5|||||9|||||10,7\n3|||||8", "4*5*9*10*7*3*8 = 302400")]
        public void Multiplying_Two_Numbers(string input, string output)
        {
            Assert.AreEqual(calc2.MultiplyNumbers(input), output);
        }

        [TestCase("20", "20 = 20")]
        [TestCase("4,2", "4/2 = 2")]
        [TestCase("12,-4", "12/(-4) = -3")]
        [TestCase("5,tyty", "5/0 = Cannout divide By Zero")]
        [TestCase("6\n3", "6/3 = 2")]
        [TestCase("//#\n10#5", "10/5 = 2")]
        [TestCase("//#\n40#5#4,2", "40/5/4/2 = 1")]
        [TestCase("//[|||||]\n100\n5|||||2|||||5,2", "100/5/2/5/2 = 1")]
        public void  Dividing_Two_Numbers(string input, string output)
        {
            Assert.AreEqual(calc2.DivideNumbers(input), output);
        }
    }
}