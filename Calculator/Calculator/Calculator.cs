using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Calculator
{
    public class Calculator
    {
        char alternateDelimiter = '\n';
        bool allowNegativeNum = false;
        int? upperBound = null;

        public Calculator()
        {

        }

        public Calculator(char delimiter, bool negativeNumberAllowed, int? upperLimit)
        {
            alternateDelimiter = delimiter;
            allowNegativeNum = negativeNumberAllowed;
            upperBound = upperLimit;
        } 

        public string AddNumbers(string input)
        {
            return ProcessInput(input, '+');
        }

        public string SubtractNumbers(string input)
        {
            return ProcessInput(input, '-');
        }
        public string MultiplyNumbers(string input)
        {
            return ProcessInput(input, '*');
        }
        public string DivideNumbers(string input)
        {
            return ProcessInput(input, '/');
        }

        // Common function reused among Addition, subtraction, multiplication and division operations
        string ProcessInput(string input, char operation)
        {
            if (String.IsNullOrEmpty(input))   // Empty or null input returns 0
            {
                return "0";
            }

            string interpretedNumbers = null;  //Parsed Numbers
            int? result = null;
            string[] operands = ExtractOperands(input);
            List<string> negativeNumbers = new List<string>();

            bool divisionByZero = false;   //Handling case of Division By Zero
            
            foreach (var num in operands)    // Removed 2 number constraint from Step # 1
            {
                string[] numbers = num.Split(',', alternateDelimiter); // further dividing each custom delmited strings by comma and alternate character
                foreach (var number in numbers)
                {
                    var interpretedNum = ParseNumber(number.Trim(), ref negativeNumbers);
                    interpretedNum = upperBound != null && interpretedNum > upperBound ? 0 : interpretedNum;

                    if (result == null)
                    {
                        result = interpretedNum;
                    }

                    else
                    {
                        switch (operation)
                        {
                            case '+':
                                result += interpretedNum;
                                break;
                            case '-':
                                result -= interpretedNum;
                                break;
                            case '*':
                                result *= interpretedNum;
                                break;
                            case '/':
                                if (interpretedNum == 0)
                                {
                                    divisionByZero = true;
                                }
                                if (!divisionByZero)
                                {
                                    result = result /= interpretedNum;
                                }
                                break;
                        }
                    }
                    interpretedNumbers = interpretedNumbers == null ? interpretedNum.ToString() : string.Format("{0}{1}{2}", interpretedNumbers, operation.ToString(),
                                                                          (interpretedNum < 0 ? string.Format("({0})", interpretedNum.ToString()) : interpretedNum.ToString()));
                }
            }

            if (negativeNumbers.Count > 0 && !allowNegativeNum)
            {
                throw new Exception(string.Format("Negative numbers '{0}' are not allowed", string.Join(",", negativeNumbers)));
            }

            return string.Format("{0} = {1}", interpretedNumbers.Trim(), divisionByZero ? "Cannout divide By Zero" : (result == null ? "0" : result.ToString().Trim()));
        }

        //Extracting Operands by custom delimiters for calculation 
        string[] ExtractOperands(string input)
        {
            var str = input;
            List<string> customDelimiters = new List<string>();

            string delimiterString = Regex.Match(input, @"//(([[].*[]])*|.)\n").Value;  //multiple custom delimiters of any length

            if (!String.IsNullOrEmpty(delimiterString))
            {
                if (!String.IsNullOrEmpty(Regex.Match(delimiterString, @"([[].*[]])+").Value))    // one or more customr delimiters of any length
                {
                    customDelimiters = delimiterString.TrimStart('/').Split('[', ']').Where(x => !String.IsNullOrEmpty(x)).ToList();
                }
                else
                {
                    customDelimiters.Add(delimiterString.Split('\n')[0].TrimStart('/'));  // customr delimiter of single character
                }
                string[] relevantString = Regex.Split(input, @"//[[]?.*[]]?\n");
                str = relevantString[1];
                return str.Split(customDelimiters.ToArray(), StringSplitOptions.None);     // dividing string first by custom delimiter(s) for correctness
            }
            return new string[] { str };     // if no customDelimiters present, sent the input as received
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
            }

            var num = int.Parse(number);
            return num;
        }

        static void Main()
        {
            Calculator calc = new Calculator();
            Console.Write("Calculator Input String: ");
            string input = Console.ReadLine();
            Console.WriteLine("Result: {0}", calc.AddNumbers(input));
            Console.ReadLine();
        }
    }
}
