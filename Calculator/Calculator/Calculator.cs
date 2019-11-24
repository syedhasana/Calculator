using System;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Calculator
    {
        public int AddNumbers(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return 0;
            }
            string[] s = input.Split(',');

            if(s.Length > 2)
            {
                throw new Exception("Calculator only accepts 2 numbers at a time");
            }

            int num1 = ParseNumber(s[0]);      
            int num2 = s.Length == 2 ? ParseNumber(s[1]) : 0;

            return num1 + num2;
        }

        //Returns integer as required for the string
        int ParseNumber(string number)
        {
            if (String.IsNullOrEmpty(number) || !Regex.IsMatch(number, @"^-*\d+$"))
            {
                return 0;
            }
            return int.Parse(number);
        }
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();
            Console.Write("Calculator Input String: ");
            string input = Console.ReadLine();
            Console.WriteLine("Result: {0}", calc.AddNumbers(input));
            Console.ReadLine();
        }
    }
}
