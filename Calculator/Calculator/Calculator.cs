using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Calculator
    {
        public int AddNumbers(string input)
        {
            int result = 0;
            var str = input;
            char customDelimiter = '\0';

            if (String.IsNullOrEmpty(input))
            {
                return 0;
            }

            string delimiterString = Regex.Match(input, @"//.\n").Value;

            if (!String.IsNullOrEmpty(delimiterString))
            {
                customDelimiter = (char)delimiterString.Split("//")[1][0];
                string[] relevantString = Regex.Split(input, @"//.\n");
                str = relevantString[1];
            }

            string[] s = str.Split(',', '\n', customDelimiter);     // dividing string on both comma and newline character(s)

            List<string> negativeNumbers = new List<string>();

            foreach (var num in s)    // Removed 2 number constraint from Step # 1
            {
                result += ParseNumber(num.Trim(), ref negativeNumbers);
            }

            if(negativeNumbers.Count > 0)
            {
                throw new Exception(string.Format("Negative numbers '{0}' are not allowed", string.Join(",", negativeNumbers)));
            }

            return result;
        }

        //Returns integer as required for the string
        int ParseNumber(string number, ref List<string> negativeNumbers)
        {
            if (String.IsNullOrEmpty(number) || !Regex.IsMatch(number, @"^-*\d+$"))
            {
                return 0;
            }

            if(Regex.IsMatch(number, @"[-]\d+$"))
            {
                negativeNumbers.Add(number);
                return 0;
            }

            var num = int.Parse(number);
            return num > 1000 ? 0 : num;
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
