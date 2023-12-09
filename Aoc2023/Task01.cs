using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    public static class Task01
    {
        public static int GetNormal()
        {
            int result = 0;

            Regex regex = new Regex(@"\d");

            using (InputReader reader = new InputReader(1))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var digits = regex.Matches(line);
                    int lineValue = Convert.ToInt32($"{digits.First()}{digits.Last()}");
                    result += lineValue;
                }
            }

            return result;
        }

        public static int GetPlatinum()
        {
            int result = 0;

            Regex regex = new Regex(@"(\d|one|two|three|four|five|six|seven|eight|nine)");
            Regex reveseRegex = new Regex(@"(\d|eno|owt|eerht|rouf|evif|xis|neves|thgie|enin)");

            using (InputReader reader = new InputReader(1))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var digits = regex.Matches(line);
                    var reverseLine = new string(line.Reverse().ToArray());
                    var reverseDigits = reveseRegex.Matches(reverseLine);

                    var lastDigit = line.Length - (reverseDigits.First().Index + reverseDigits.First().Value.Length) > digits.Last().Index ? new string(reverseDigits.First().Value.Reverse().ToArray()) : digits.Last().Value;


                    int lineValue = Convert.ToInt32($"{GetDigit(digits.First().Value)}{GetDigit(lastDigit)}");
                    result += lineValue;
                }
            }

            return result;
        }

        private static string GetDigit(string value)
        {
            return value switch
            {
                "one" => "1",
                "two" => "2",
                "three" => "3",
                "four" => "4",
                "five" => "5",
                "six" => "6",
                "seven" => "7",
                "eight" => "8",
                "nine" => "9",
                _ => value
            };
        }
    }
}
