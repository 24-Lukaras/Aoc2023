using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task03
    {
        static Regex digitRegex = new Regex(@"\d+");

        public static int GetNormal()
        {
            int result = 0;

            using (InputReader reader = new InputReader(3))
            {
                List<int> prevProcessedIndexes = new List<int>();
                List<int> currentProcessedIndexes = new List<int>();
                
                string prevLine = null;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    currentProcessedIndexes.Clear();

                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];

                        if (c != '.' && !char.IsDigit(c))
                        {
                            var lineMatches = digitRegex.Matches(line);
                            var toProcess = lineMatches.Where(x => x.Index + x.Value.Length == i || x.Index == i + 1);
                            foreach (var m in toProcess)
                            {
                                result += Convert.ToInt32(m.Value);
                                currentProcessedIndexes.Add(m.Index);
                            }

                            if (prevLine != null)
                            {
                                result += ProcessDifferentLine(prevLine, i, prevProcessedIndexes);
                            }
                        }
                    }

                    if (prevLine != null)
                    {
                        for (int i = 0; i < prevLine.Length; i++)
                        {
                            char c = prevLine[i];

                            if (c != '.' && !char.IsDigit(c))
                            {
                                result += ProcessDifferentLine(line, i, currentProcessedIndexes);
                            }
                        }
                    }

                    prevLine = line;
                    prevProcessedIndexes = currentProcessedIndexes.ToList();
                }
            }
            return result;
        }

        private static int ProcessDifferentLine(string difLine, int i, List<int> coll)
        {
            int result = 0;
            var lineMatches = digitRegex.Matches(difLine);
            var toProcess = lineMatches.Where(x => x.Index <= i + 1 && x.Index + x.Value.Length >= i);
            foreach (var m in toProcess)
            {
                if (!coll.Contains(m.Index))
                {
                    result += Convert.ToInt32(m.Value);
                }
            }
            return result;
        }

        public static int GetPlatinum()
        {
            int result = 0;

            using (InputReader reader = new InputReader(3))
            {
                string prevLine = null;
                string currentLine = null;
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (currentLine != null)
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (currentLine[i] == '*')
                            {
                                List<int> gearRatios = new List<int>();

                                var lineMatches = digitRegex.Matches(currentLine);
                                var toProcess = lineMatches.Where(x => x.Index + x.Value.Length == i || x.Index == i + 1);
                                gearRatios.AddRange(toProcess.Select(x => Convert.ToInt32(x.Value)));

                                lineMatches = digitRegex.Matches(line);
                                toProcess = lineMatches.Where(x => x.Index <= i + 1 && x.Index + x.Value.Length >= i);
                                gearRatios.AddRange(toProcess.Select(x => Convert.ToInt32(x.Value)));

                                if (prevLine != null)
                                {
                                    lineMatches = digitRegex.Matches(prevLine);
                                    toProcess = lineMatches.Where(x => x.Index <= i + 1 && x.Index + x.Value.Length >= i);
                                    gearRatios.AddRange(toProcess.Select(x => Convert.ToInt32(x.Value)));
                                }

                                if (gearRatios.Count == 2)
                                {
                                    result += gearRatios[0] * gearRatios[1];
                                }
                            }
                        }
                    }


                    prevLine = currentLine;
                    currentLine = line;
                }
            }

            return result;
        }



    }
}
