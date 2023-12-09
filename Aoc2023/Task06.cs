using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task06
    {

        static Regex digitRegex = new Regex(@"\d+");

        public static int GetNormal()
        {
            int[] times, distances;

            using (var reader = new InputReader(6))
            {
                var line = reader.ReadLine();
                times = digitRegex.Matches(line).Select(x => Convert.ToInt32(x.Value)).ToArray();
                line = reader.ReadLine();
                distances = digitRegex.Matches(line).Select(x => Convert.ToInt32(x.Value)).ToArray();
            }

            int[] results = new int[times.Length];

            for (int i = 0; i < times.Length; i++)
            {
                var time = times[i];
                var distance = distances[i];
                for (int j = 0; j <= time; j++)
                {
                    if (j * (time - j) > distance)
                    {
                        results[i]++;
                    }
                }
            }

            int result = results[0];
            for (int i = 1; i < results.Length; i++)
            {
                result *= results[i];
            }
            return result;
        }

        public static int GetPlatinum()
        {
            long time, distance;

            using (var reader = new InputReader(6))
            {
                var line = reader.ReadLine();
                time = Convert.ToInt64(string.Join(string.Empty, digitRegex.Matches(line).Select(x => x.Value)));

                line = reader.ReadLine();
                distance = Convert.ToInt64(string.Join(string.Empty, digitRegex.Matches(line).Select(x => x.Value)));
            }

            int result = 0;
            for (int j = 0; j <= time; j++)
            {
                if (j * (time - j) > distance)
                {
                    result++;
                }
            }
            return result;
        }

    }
}
