using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task02
    {

        const int RED_MAX = 12;
        const int GREEN_MAX = 13;
        const int BLUE_MAX = 14;

        static Regex colorRegex = new Regex(@"(red|green|blue)");
        static Regex digitRegex = new Regex(@"\d+");
        const char RESULT_SEPARATOR = ':';
        const char SET_SEPARATOR = ';';

        public static int GetNormal()
        {
            int result = 0;

            using (var reader = new InputReader(2))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result += LineToGameId(line);
                }
            }

            return result;
        }

        public static int GetPlatinum()
        {
            int result = 0;

            using (var reader = new InputReader(2))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result += FindLinePower(line);
                }
            }

            return result;
        }

        private static int LineToGameId(string line)
        {
            var arr = line.Split(RESULT_SEPARATOR);

            var id = Convert.ToInt32(digitRegex.Match(arr[0]).Value);

            string[] sets = arr[1].Split(SET_SEPARATOR);
            foreach (string set in sets)
            {
                int red = 0;
                int green = 0;
                int blue = 0;
                var values = digitRegex.Matches(set);
                var colors = colorRegex.Matches(set);

                for (int i = 0; i < values.Count; i++)
                {
                    int value = Convert.ToInt32(values[i].Value);
                    string color = colors[i].Value;

                    switch (color)
                    {
                        default:
                            throw new ArgumentException();

                        case "red":
                            red += value;
                            break;

                        case "green":
                            green += value;
                            break;

                        case "blue":
                            blue += value;
                            break;
                    }
                }

                if (red > RED_MAX || green > GREEN_MAX || blue > BLUE_MAX)
                {
                    return 0;
                }
            }

            return id;
        }

        private static int FindLinePower(string line)
        {
            var arr = line.Split(RESULT_SEPARATOR);

            int redMin = 0;
            int greenMin = 0;
            int blueMin = 0;
            string[] sets = arr[1].Split(SET_SEPARATOR);
            foreach (string set in sets)
            {
                int red = 0;
                int green = 0;
                int blue = 0;
                var values = digitRegex.Matches(set);
                var colors = colorRegex.Matches(set);

                for (int i = 0; i < values.Count; i++)
                {
                    int value = Convert.ToInt32(values[i].Value);
                    string color = colors[i].Value;

                    switch (color)
                    {
                        default:
                            throw new ArgumentException();

                        case "red":
                            red += value;
                            break;

                        case "green":
                            green += value;
                            break;

                        case "blue":
                            blue += value;
                            break;
                    }
                }

                if (red > redMin) redMin = red;
                if (green > greenMin) greenMin = green;
                if (blue > blueMin) blueMin = blue;
            }

            return redMin * greenMin * blueMin;
        }
    }
}
