using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task11
    {
        const char empty = '.';
        const char galaxy = '#';


        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(11))
            {
                List<string> lines = new List<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                var columns = Enumerable.Range(0, lines[0].Length).ToList();

                foreach (var inputLine in lines)
                {
                    for (int i = columns.Count - 1; i >= 0; i--)
                    {
                        var index = columns[i];

                        if (inputLine[index] == galaxy)
                        {
                            columns.RemoveAt(i);
                        }
                    }
                }
                for (int l = 0; l < lines.Count; l++)
                {
                    for (int i = columns.Count - 1; i >= 0; i--)
                    {
                        lines[l] = lines[l].Insert(columns[i], ".");
                    }
                }
                string emptyLine = string.Concat(Enumerable.Repeat(".", lines[0].Length));
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    if (lines[i].IndexOf(galaxy) == -1)
                    {
                        lines.Insert(i, emptyLine);
                    }
                }

                List<Galaxy> galaxies = new List<Galaxy>();
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == galaxy)
                        {
                            galaxies.Add(new Galaxy(x, y));
                        }
                    }
                }

                for (int i = 0; i < galaxies.Count; i++)
                {
                    var g = galaxies[i];

                    for (int j = i + 1; j < galaxies.Count; j++)
                    {
                        var gComp = galaxies[j];
                        result += (Math.Abs(g.x - gComp.x) + Math.Abs(g.y - gComp.y));
                    }
                }

                ;

            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(11))
            {
                List<string> lines = new List<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                var columns = Enumerable.Range(0, lines[0].Length).ToList();
                var emptyLines = Enumerable.Range(0, lines.Count).ToList();

                foreach (var inputLine in lines)
                {
                    for (int i = columns.Count - 1; i >= 0; i--)
                    {
                        var index = columns[i];

                        if (inputLine[index] == galaxy)
                        {
                            columns.RemoveAt(i);
                        }
                    }
                }
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    if (lines[i].IndexOf(galaxy) != -1)
                    {
                        emptyLines.RemoveAt(i);
                    }
                }

                List<Galaxy> galaxies = new List<Galaxy>();
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == galaxy)
                        {
                            var columnsE = columns.Where(c => c < x).Sum(x => 999999);
                            var linesE = emptyLines.Where(c => c < y).Sum(x => 999999);

                            galaxies.Add(new Galaxy(x + columnsE, y + linesE));
                        }
                    }
                }

                for (int i = 0; i < galaxies.Count; i++)
                {
                    var g = galaxies[i];

                    for (int j = i + 1; j < galaxies.Count; j++)
                    {
                        var gComp = galaxies[j];
                        result += (Math.Abs(g.x - gComp.x) + Math.Abs(g.y - gComp.y));
                    }
                }

                ;

            }

            return result;
        }

        public record Galaxy(int x, int y);
    }
}
