using System;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Calculator
    {
        public int AddNumbers(string input)
        {
            int result = 0;
            if (String.IsNullOrEmpty(input))
            {
                return 0;
            }
            string[] s = input.Split(',');

            foreach(var num in s)    // Removed 2 number constraint from Step # 1
            {
                result += ParseNumber(num.Trim());
            }

            return result;
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
