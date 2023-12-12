using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Aoc2023
{
    internal static class Task12
    {
        public static int task_i = 0;

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(12))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parameters = line.Split(' ');

                    int[] groups = parameters[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                    SpringLine springLine = new SpringLine()
                    {
                        Value = parameters[0]
                    };

                    int val = springLine.TryFit(0, 0, groups);
                    result += val;
                }
            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(12))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    task_i++;
                    string[] parameters = line.Split(' ');

                    string springLine = string.Join('?', new string[] { parameters[0], parameters[0], parameters[0], parameters[0], parameters[0] });
                    int[] groups = string.Join(',', new string[] { parameters[1], parameters[1], parameters[1], parameters[1], parameters[1] }).Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                    Dictionary<(int, int, int), long> resultDictionary = new Dictionary<(int, int, int), long>();

                    result += SpringLineScore(resultDictionary, springLine, groups);
                }
            }

            return result;
        }

        //shamelessly stolen - https://github.com/jonathanpaulson/AdventOfCode/blob/master/2023/12.py
        private static long SpringLineScore(Dictionary<(int, int, int), long> resultDictionary, string springLine, int[] groups, int i = 0, int bi = 0, int current = 0)
        {
            var key = (i, bi, current);

            if (resultDictionary.TryGetValue(key, out long ans))
            {
                return ans;
            }
            if (i == springLine.Length)
            {
                if (bi == groups.Length && current == 0)
                {
                    return 1;
                }
                else if (bi == groups.Length - 1 && groups[bi] == current)
                {
                    return 1;
                }
                return 0;
            }

            long result = 0;

            var chars = ".#";
            foreach (var c in chars)
            {
                if (springLine[i] == c || springLine[i] == '?')
                {
                    if (c == '.' && current == 0)
                    {
                        result += SpringLineScore(resultDictionary, springLine, groups, i + 1, bi);
                    }
                    else if (c == '.' && current > 0 && bi < groups.Length && groups[bi] == current)
                    {
                        result += SpringLineScore(resultDictionary, springLine, groups, i + 1, bi + 1);
                    }
                    else if (c == '#')
                    {
                        result += SpringLineScore(resultDictionary, springLine, groups, i + 1, bi, current + 1);
                    }
                }
            }

            resultDictionary[key] = result;
            return result;
        }
    }

    public class SpringLine
    {
        Regex sliceRegex = new Regex(@"^(\?|#)+$");
        Regex firstMatchRegex = new Regex(@"[#]+");

        public string Value { get; set; }

        public int TryFit(int start, int index, params int[] lengths)
        {
            if (index >= lengths.Length)
            {
                var matches = firstMatchRegex.Matches(Value);
                if (matches.Count != lengths.Length)
                {
                    return 0;
                }

                bool correct = true;
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Value.Length != lengths[i])
                    {
                        correct = false;
                    }
                }
                return correct ? 1 : 0;
            }

            int result = 0;

            int firstDamageIndex = firstMatchRegex.Matches(Value).Skip(index).FirstOrDefault()?.Index ?? int.MaxValue;

            for (int i = start; i + lengths[index] <= Value.Length; i++)
            {
                if (start > firstDamageIndex || start + lengths[index] > Value.Length)
                {
                    break;
                }

                string slice = Value.Substring(i, lengths[index]);
                if ((i + lengths[index] + 1 >= Value.Length || "?.".IndexOf(Value[i + lengths[index]]) != -1) && (i == 0 || Value[i - 1] != '#'))
                {
                    if (sliceRegex.IsMatch(slice))
                    {
                        int l = i + lengths[index] + 1 > Value.Length ? i + lengths[index] : i + lengths[index] + 1;
                        string newString = $"{Value.Remove(i, lengths[index]).Insert(i, string.Join(string.Empty, Enumerable.Repeat('#', lengths[index]))).Substring(0, l).Replace("?", ".")}{Value.Substring(l)}";
                        result += new SpringLine()
                        {
                            Value = newString
                        }.TryFit(i + lengths[index], index + 1, lengths);
                    }
                }

            }
            return result;
        }
    }    
}
