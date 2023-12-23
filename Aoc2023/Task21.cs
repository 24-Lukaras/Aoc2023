using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task21
    {

        public static int GetNormal()
        {
            using (var reader = new InputReader(21))
            {
                List<(int, int)> currentPositions = new List<(int, int)>();
                HashSet<(int, int)> rocks = new HashSet<(int, int)>();
                string line;
                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        var c = line[x];
                        switch (c)
                        {
                            default:
                                throw new NotImplementedException();

                            case '.':
                                break;

                            case 'S':
                                currentPositions.Add((x, y));
                                break;

                            case '#':
                                rocks.Add((x, y));
                                break;
                        }
                    }

                    y++;
                }

                for (int i = 0; i < 64; i++)
                {
                    List<(int, int)> newPositions = new List<(int, int)>();

                    foreach (var position in currentPositions)
                    {
                        if (!rocks.Contains((position.Item1 - 1, position.Item2)))
                        {
                            newPositions.Add((position.Item1 - 1, position.Item2));
                        }
                        if (!rocks.Contains((position.Item1 + 1, position.Item2)))
                        {
                            newPositions.Add((position.Item1 + 1, position.Item2));
                        }
                        if (!rocks.Contains((position.Item1, position.Item2 - 1)))
                        {
                            newPositions.Add((position.Item1, position.Item2 - 1));
                        }
                        if (!rocks.Contains((position.Item1, position.Item2 + 1)))
                        {
                            newPositions.Add((position.Item1, position.Item2 + 1));
                        }
                    }

                    currentPositions = newPositions.Distinct().ToList();
                }

                return currentPositions.Count();
            }
        }

        public static int GetPlatinum()
        {
            using (var reader = new InputReader(21))
            {
                List<(int, int)> currentPositions = new List<(int, int)>();
                HashSet<(int, int)> rocks = new HashSet<(int, int)>();
                string line;
                int y = 0;
                int maxX = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (maxX == 0)
                    {
                        maxX = line.Length - 1;
                    }
                    for (int x = 0; x < line.Length; x++)
                    {
                        var c = line[x];
                        switch (c)
                        {
                            default:
                                throw new NotImplementedException();

                            case '.':
                                break;

                            case 'S':
                                currentPositions.Add((x, y));
                                break;

                            case '#':
                                rocks.Add((x, y));
                                break;
                        }
                    }

                    y++;
                }

                for (int i = 0; i < 50; i++)
                {
                    List<(int, int)> newPositions = new List<(int, int)>();

                    foreach (var position in currentPositions)
                    {

                        //if (position.Item1 <= 0 || position.Item1 >= 32 || position.Item2 <= 0 || position.Item2 >= 32)
                        if (position.Item1 <= -31)
                        {
                            ;
                        }

                        int x = position.Item1 - 1 % maxX;
                        if (x < 0)
                        {
                            x = maxX + 1 + x;
                        }
                        if (!rocks.Contains((x, position.Item2)))
                        {
                            newPositions.Add((position.Item1 - 1, position.Item2));
                        }
                        x = (position.Item1 + 1) % (maxX + 1);
                        if (!rocks.Contains((x, position.Item2)))
                        {
                            newPositions.Add((position.Item1 + 1, position.Item2));
                        }
                        int _y = (position.Item2 - 1) % (y - 1);
                        if (_y < 0)
                        {
                            _y = y + _y;
                        }
                        if (!rocks.Contains((position.Item1, _y)))
                        {
                            newPositions.Add((position.Item1, position.Item2 - 1));
                        }
                        _y = (position.Item2 + 1) % y;
                        if (!rocks.Contains((position.Item1, _y)))
                        {
                            newPositions.Add((position.Item1, position.Item2 + 1));
                        }
                    }

                    currentPositions = newPositions.Distinct().ToList();
                }

                return currentPositions.Count();
            }
        }

    }
}
