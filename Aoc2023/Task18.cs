using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aoc2023.Task10;

namespace Aoc2023
{
    internal static class Task18
    {

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(18))
            {
                string line;

                int x = 0;
                int y = 0;
                char prevChar = 'U';
                Dictionary<(int, int), char> path = new Dictionary<(int, int), char>();
                path.Add((x, y), '7');

                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(' ');
                    char direction = arr[0][0];
                    int steps = Convert.ToInt32(arr[1]);

                    switch (prevChar)
                    {
                        default:
                            throw new NotImplementedException();

                        case 'L':
                            if (direction == 'D')
                            {
                                path[(x, y)] = 'F';
                            }
                            else
                            {
                                path[(x, y)] = 'L';
                            }
                            break;

                        case 'R':
                            if (direction == 'D')
                            {
                                path[(x, y)] = '7';
                            }
                            else
                            {
                                path[(x, y)] = 'J';
                            }
                            break;

                        case 'D':
                            if (direction == 'L')
                            {
                                path[(x, y)] = 'J';
                            }
                            else
                            {
                                path[(x, y)] = 'L';
                            }
                            break;

                        case 'U':
                            if (direction == 'L')
                            {
                                path[(x, y)] = '7';
                            }
                            else
                            {
                                path[(x, y)] = 'F';
                            }
                            break;
                    }

                    for (int i = 0; i < steps; i++)
                    {
                        switch (direction)
                        {
                            default:
                                throw new NotImplementedException();

                            case 'L':
                                x--;
                                path.Add((x, y), '-');
                                break;

                            case 'R':
                                x++;
                                path.Add((x, y), '-');
                                break;

                            case 'D':
                                y++;
                                path.Add((x, y), '|');
                                break;

                            case 'U':
                                y--;
                                if (x == 0 && y == 0)
                                {
                                    break;
                                }
                                path.Add((x, y), '|');
                                break;

                        }
                        prevChar = direction;
                    }
                }               

                List<Coordinates> coordinates = new List<Coordinates>();
                Coordinates coords = new Coordinates()
                {
                    X = 0,
                    Y = 0,
                    LastMove = "up",
                    Inside = InsideDirection.Rigth | InsideDirection.Up,
                };

                do
                {
                    var newCoords = new Coordinates()
                    {
                        X = coords.X,
                        Y = coords.Y,
                    };
                    coords.SwitchPos(path[(newCoords.X, newCoords.Y)]);

                    newCoords.Inside = coords.Inside;
                    coordinates.Add(newCoords);
                } while (coords.X != 0 || coords.Y != 0);


                int minX = coordinates.Min(x => x.X);
                int minY = coordinates.Min(x => x.Y);
                int maxX = coordinates.Max(x => x.X);
                int maxY = coordinates.Max(x => x.Y);

                var coordsDic = coordinates.ToDictionary(x => (x.Y, x.X));
                bool[,] map = new bool[maxY - minY + 1, maxX - minX + 1];
                bool[,] values = new bool[maxY - minY + 1, maxX - minX + 1];

                foreach (var coord in coordinates)
                {
                    map[coord.Y - minY, coord.X - minX] = true;
                }

                foreach (var coord in coordinates)
                {
                    if (coord.Inside.HasFlag(InsideDirection.Up) && !map[coord.Y - minY - 1, coord.X - minX])
                    {
                        int tempY = coord.Y - minY - 1;
                        while (!map[tempY, coord.X - minX])
                        {
                            values[tempY, coord.X - minX] = true;
                            tempY--;
                        }
                    }
                    if (coord.Inside.HasFlag(InsideDirection.Down) && !map[coord.Y - minY + 1, coord.X - minX])
                    {
                        int tempY = coord.Y - minY + 1;
                        while (!map[tempY, coord.X - minX])
                        {
                            values[tempY, coord.X - minX] = true;
                            tempY++;
                        }
                    }
                    if (coord.Inside.HasFlag(InsideDirection.Left) && !map[coord.Y - minY, coord.X - minX - 1])
                    {
                        int tempX = coord.X - minX - 1;
                        while (!map[coord.Y - minY, tempX])
                        {
                            values[coord.Y - minY, tempX] = true;
                            tempX--;
                        }
                    }
                    if (coord.Inside.HasFlag(InsideDirection.Rigth) && !map[coord.Y - minY, coord.X - minX + 1])
                    {
                        int tempX = coord.X - minX + 1;
                        while (!map[coord.Y - minY, tempX])
                        {
                            values[coord.Y - minY, tempX] = true;
                            tempX++;
                        }
                    }
                }

                for (int tempY = 0; tempY < values.GetLength(0); tempY++)
                {
                    for (int tempX = 0; tempX < values.GetLength(1); tempX++)
                    {
                        if (values[tempY, tempX] || map[tempY, tempX]) result++;
                    }
                }
            }

            return result;
        }

    }
}
