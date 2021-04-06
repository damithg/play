using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BritGroup.Application
{
    public class ParseNumbers : IParseNumbers
    {
        private readonly string _input;

        // Quite unnecessary as we could either pass the whole request here or on the public method.
        // Just adding in ctor as per the test spec
        public ParseNumbers(string input)
        {
            _input = input;
        }

        public double Parse(string request)
        {
            var operators = Regex.Matches(request, @"(apply)")
                .OfType<Match>();

            return !operators.Any() ? throw new ArgumentException($"Invalid request") : CalculateLine(request);
        }

        private double CalculateLine(string section)
        {
            var digits = Regex.Split(section, @"\D+")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            digits.Insert(0, _input);

            var operators = Regex.Matches(section, @"(addition|multiplication|subtraction|division)")
                .OfType<Match>()
                .Select(m => m.Groups[0].Value).ToList();

            var result = 0.0;

            operators.ForEach(o => {
                result = Calculate((Instruction)Enum.Parse(typeof(Instruction), o, true), double.Parse(digits[0]), double.Parse(digits[1]));
                digits.RemoveRange(0, 2);
                digits.Insert(0, result.ToString());
            });

            return result;
        }

        private double Calculate(Instruction operation, double value1, double value2 = 0)
        {
            switch (operation)
            {
                case Instruction.Addition:
                    return value1 + value2;
                case Instruction.Subtraction:
                    return value1 - value2;
                case Instruction.Multiplication:
                    return value1 * value2;
                case Instruction.Division:
                    return value1 / value2;
                default:
                    throw new ArgumentException($"Operation {operation} not supported");
            }
        }
    }
}