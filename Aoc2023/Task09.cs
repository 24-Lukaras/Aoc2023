using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task09
    {

        static Regex digitRegex = new Regex(@"-*\d+");

        public static long GetNormal()
        {
            long result = 0;
            using (var reader = new InputReader(9))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var sequence = digitRegex.Matches(line).Select(x => Convert.ToInt64(x.Value)).ToList();
                    result += GetSequenceNext(sequence);
                }
            }
            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;
            using (var reader = new InputReader(9))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var sequence = digitRegex.Matches(line).Reverse().Select(x => Convert.ToInt64(x.Value)).ToList();
                    result += GetSequenceNext(sequence);
                }
            }
            return result;
        }

        static long GetSequenceNext(List<long> sequence, bool backwards = false)
        {
            if (sequence.All(x => x == 0))
            {
                return 0;
            }

            List<long> newSequence = new List<long>();
            for (int i = 0; i < sequence.Count - 1; i++)
            {
                newSequence.Add(sequence[i + 1] - (backwards ? -sequence[i] : sequence[i]));
            }

            var seqResult = GetSequenceNext(newSequence, backwards);
            var result = sequence[sequence.Count - 1] + (backwards ? -seqResult : seqResult);
            return result;
        }

    }
}
