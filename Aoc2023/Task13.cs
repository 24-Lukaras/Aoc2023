using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task13
    {

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(13))
            {
                string line;
                int i = 0;
                List<(int, string)> coll = new List<(int, string)>();
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        result += GetCollValue(coll);

                        coll = new List<(int, string)>();
                        i = 0;
                    }
                    else
                    {
                        coll.Add((i, line));
                        i++;
                    }
                }
                result += GetCollValue(coll);
            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(13))
            {
                string line;
                int i = 0;
                List<(int, string)> coll = new List<(int, string)>();
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        result += GetCollValue_P(coll);

                        coll = new List<(int, string)>();
                        i = 0;
                    }
                    else
                    {
                        coll.Add((i, line));
                        i++;
                    }
                }
                result += GetCollValue_P(coll);
            }

            return result;
        }

        private static int GetCollValue(List<(int, string)> coll)
        {
            var groups = coll.GroupBy(x => x.Item2).Where(x => x.Count() > 1).ToArray();
            bool wasVertical = false;
            if (groups.Any())
            {
                if (TryFindVerticalMirror(groups, out var indexes))
                {
                    foreach (var mirrorIndex in indexes)
                    {
                        wasVertical = true;
                        for (int j = 0; mirrorIndex.Item2 + j < coll.Count && mirrorIndex.Item1 - j >= 0; j++)
                        {
                            if (coll[mirrorIndex.Item1 - j].Item2 != coll[mirrorIndex.Item2 + j].Item2)
                            {
                                wasVertical = false;
                                break;
                            }
                        }
                        if (wasVertical)
                        {
                            return 100 * mirrorIndex.Item2;
                        }
                    }
                }
            }

            if (!wasVertical)
            {
                var tempLine = coll[0];

                var mirrors = FindHorizontalMirror(tempLine.Item2);

                foreach (var mirrorIndex in mirrors)
                {
                    bool isMirrored = true;

                    for (int j = 0; mirrorIndex.Item2 + j < tempLine.Item2.Length && mirrorIndex.Item1 - j >= 0; j++)
                    {
                        foreach (var mapLine in coll)
                        {
                            if (mapLine.Item2[mirrorIndex.Item2 + j] != mapLine.Item2[mirrorIndex.Item1 - j])
                            {
                                isMirrored = false;
                                break;
                            }
                        }

                        if (!isMirrored) break;
                    }

                    if (isMirrored)
                    {
                        return mirrorIndex.Item2;
                    }
                }
            }
            return 0;
        }

        private static int GetCollValue_P(List<(int, string)> coll)
        {
            bool wasVertical = false;

            if (TryFindVerticalMirror_P(coll, out var indexes))
            {
                foreach (var mirrorIndex in indexes)
                {
                    wasVertical = true;
                    bool ignoreZero = mirrorIndex.Error == 1;
                    for (int j = 0; mirrorIndex.Indexes.Item2 + j < coll.Count && mirrorIndex.Indexes.Item1 - j >= 0; j++)
                    {
                        if (!ignoreZero || j != 0)
                        {
                            var lineError = GetLinesErrorScore(coll[mirrorIndex.Indexes.Item1 - j].Item2, coll[mirrorIndex.Indexes.Item2 + j].Item2);
                            mirrorIndex.Error += lineError;
                        }
                        if (mirrorIndex.Error > 1)
                        {
                            wasVertical = false;
                            break;
                        }
                    }
                    if (wasVertical && mirrorIndex.Error == 1)
                    {                       
                        return 100 * mirrorIndex.Indexes.Item2;
                    }
                }
            }


            var flippedColl = new List<(int, string)>();

            for (int i = 0; i < coll[0].Item2.Length; i++)
            {
                flippedColl.Add((i, string.Join(string.Empty, coll.Select(c => c.Item2[i]))));
            }

            if (TryFindVerticalMirror_P(flippedColl, out var indexes_V))
            {
                foreach (var mirrorIndex in indexes_V)
                {
                    wasVertical = true;
                    bool ignoreZero = mirrorIndex.Error == 1;
                    for (int j = 0; mirrorIndex.Indexes.Item2 + j < flippedColl.Count && mirrorIndex.Indexes.Item1 - j >= 0; j++)
                    {
                        if (!ignoreZero || j != 0)
                        {
                            var lineError = GetLinesErrorScore(flippedColl[mirrorIndex.Indexes.Item1 - j].Item2, flippedColl[mirrorIndex.Indexes.Item2 + j].Item2);
                            mirrorIndex.Error += lineError;
                        }
                        if (mirrorIndex.Error > 1)
                        {
                            wasVertical = false;
                            break;
                        }
                    }
                    if (wasVertical && mirrorIndex.Error == 1)
                    {                        
                        return mirrorIndex.Indexes.Item2;
                    }
                }
            }

            /*
            if (!wasVertical)
            {
                var tempLine = coll[0];

                var mirrors = FindHorizontalMirror_P(tempLine.Item2);

                foreach (var mirrorIndex in mirrors)
                {
                    bool isMirrored = true;

                    for (int j = 0; mirrorIndex.Indexes.Item2 + j < tempLine.Item2.Length && mirrorIndex.Indexes.Item1 - j >= 0; j++)
                    {
                        foreach (var mapLine in coll)
                        {
                            if (mapLine.Item2[mirrorIndex.Indexes.Item2 + j] != mapLine.Item2[mirrorIndex.Indexes.Item1 - j])
                            {
                                mirrorIndex.Error++;
                            }
                            if (mirrorIndex.Error > 1)
                            {
                                isMirrored = false;
                                break;
                            }
                        }

                        if (!isMirrored) break;
                    }

                    if (isMirrored && mirrorIndex.Error == 1)
                    {
                        return mirrorIndex.Indexes.Item2;
                    }
                }
            }
            */
            return 0;
        }


        private static bool TryFindVerticalMirror(IGrouping<string, (int, string)>[] groups, out List<(int, int)> indexes)
        {
            bool found = false;
            indexes = new List<(int, int)>();
            foreach (var group in groups)
            {
                foreach (var item in group)
                {
                    if (group.Any(x => x.Item1 == item.Item1 + 1))
                    {
                        indexes.Add((item.Item1, item.Item1 + 1));
                        found = true;
                    }
                }
            }
            return found;
        }

        private static bool TryFindVerticalMirror_P(List<(int, string)> coll, out List<IndexError> indexes)
        {
            indexes = new List<IndexError>();
            for (int i = 0; i < coll.Count - 1; i++)
            {
                int lineError = GetLinesErrorScore(coll[i].Item2, coll[i + 1].Item2);
                if (lineError <= 1)
                {
                    indexes.Add(new IndexError()
                    {
                        Indexes = (i, i + 1),
                        Error = lineError
                    });
                }
            }
            return indexes.Count > 0;
        }

        private static List<(int, int)> FindHorizontalMirror(string line)
        {
            List<(int, int)> result = new List<(int, int)>();

            for (int i = 0; i < line.Length / 2; i++)
            {
                if (line.Substring(0, i + 1) == string.Join(string.Empty, line.Substring(i + 1, i + 1).Reverse()))
                {
                    result.Add((i, i + 1));
                }
                if (line.Substring(line.Length - (i + 1), i + 1) == string.Join(string.Empty, line.Substring(line.Length - ((i + 1) * 2), i + 1).Reverse()))
                {
                    result.Add((line.Length - (i + 2), line.Length - (i + 1)));
                }
            }

            return result;
        }

        private static List<IndexError> FindHorizontalMirror_P(string line)
        {
            List<IndexError> result = new List<IndexError> ();

            for (int i = 0; i < line.Length / 2; i++)
            {
                var error1 = GetLinesErrorScore(line.Substring(0, i + 1), string.Join(string.Empty, line.Substring(i + 1, i + 1).Reverse()));
                if (error1 <= 1)
                {
                    result.Add(new IndexError()
                    {
                        Indexes = (i, i + 1),
                        Error = error1
                    });
                }
                var error2 = GetLinesErrorScore(line.Substring(line.Length - (i + 1), i + 1), string.Join(string.Empty, line.Substring(line.Length - ((i + 1) * 2), i + 1).Reverse()));
                if (error2 <= 1)
                {
                    result.Add(new IndexError()
                    {
                        Indexes = (line.Length - (i + 2), line.Length - (i + 1)),
                        Error = error2
                    });
                }
            }

            return result;
        }

        private static int GetLinesErrorScore(string line1, string line2)
        {
            int error = 0;

            if (line1.Length != line2.Length)
            {
                throw new NotSupportedException();
            }

            for (int i = 0; i < line1.Length; i++)
            {
                if (line1[i] != line2[i])
                {
                    error++;
                }
            }

            return error;
        }
    }

    public class IndexError
    {
        public (int, int) Indexes { get; set; }
        public int Error { get; set; }
    }
}
