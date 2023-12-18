using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task10
    {

        public static int GetNormal()
        {
            List<string> lines = new List<string>();

            Coordinates coordinatesS = null;
            bool tb = false, lr = false, tl = false, tr = false, bl = false, br = false;

            using (var reader = new InputReader(10))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains('S'))
                    {
                        coordinatesS = new Coordinates()
                        {
                            X = line.IndexOf('S'),
                            Y = lines.Count
                        };

                        string prevLine = lines[coordinatesS.Y - 1];

                        lr = "FL-".IndexOf(line[coordinatesS.X - 1]) != -1 && "J7-".IndexOf(line[coordinatesS.X + 1]) != -1;
                        tl = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "FL-".IndexOf(line[coordinatesS.X - 1]) != -1;
                        tr = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "J7-".IndexOf(line[coordinatesS.X + 1]) != -1;

                        lines.Add(line);
                        string currenentLine = line;

                        line = reader.ReadLine();
                        tb = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "JL|".IndexOf(line[coordinatesS.X]) != -1;
                        bl = "JL|".IndexOf(line[coordinatesS.X]) != -1 && "FL-".IndexOf(currenentLine[coordinatesS.X - 1]) != -1;
                        br = "JL|".IndexOf(line[coordinatesS.X]) != -1 && "J7-".IndexOf(currenentLine[coordinatesS.X + 1]) != -1;

                    }
                    lines.Add(line);
                }

                int max = 0;
                if (tb)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y - 1,
                        LastMove = "up"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y + 1,
                        LastMove = "down"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }
                if (lr)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X - 1,
                        Y = coordinatesS.Y,
                        LastMove = "left"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X + 1,
                        Y = coordinatesS.Y,
                        LastMove = "rigth"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }
                if (tl)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y - 1,
                        LastMove = "up"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X - 1,
                        Y = coordinatesS.Y,
                        LastMove = "left"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }
                if (tr)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y - 1,
                        LastMove = "up"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X + 1,
                        Y = coordinatesS.Y,
                        LastMove = "rigth"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }
                if (bl)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y + 1,
                        LastMove = "down"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X - 1,
                        Y = coordinatesS.Y,
                        LastMove = "left"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }
                if (br)
                {
                    Coordinates coords1 = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y + 1,
                        LastMove = "down"
                    };
                    Coordinates coords2 = new Coordinates()
                    {
                        X = coordinatesS.X + 1,
                        Y = coordinatesS.Y,
                        LastMove = "rigth"
                    };
                    int result = NumberOfSteps(coords1, coords2, lines);
                    if (result > max)
                    {
                        max = result;
                    }
                }


                return max;                
            }
        }

        public static int GetPlatinum()
        {
            List<string> lines = new List<string>();

            Coordinates coordinatesS = null;
            bool tb = false, lr = false, tl = false, tr = false, bl = false, br = false;

            using (var reader = new InputReader(10))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains('S'))
                    {
                        coordinatesS = new Coordinates()
                        {
                            X = line.IndexOf('S'),
                            Y = lines.Count
                        };

                        string prevLine = lines[coordinatesS.Y - 1];

                        lr = "FL-".IndexOf(line[coordinatesS.X - 1]) != -1 && "J7-".IndexOf(line[coordinatesS.X + 1]) != -1;
                        tl = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "FL-".IndexOf(line[coordinatesS.X - 1]) != -1;
                        tr = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "J7-".IndexOf(line[coordinatesS.X + 1]) != -1;

                        lines.Add(line);
                        string currenentLine = line;

                        line = reader.ReadLine();
                        tb = "7F|".IndexOf(prevLine[coordinatesS.X]) != -1 && "JL|".IndexOf(line[coordinatesS.X]) != -1;
                        bl = "JL|".IndexOf(line[coordinatesS.X]) != -1 && "FL-".IndexOf(currenentLine[coordinatesS.X - 1]) != -1;
                        br = "JL|".IndexOf(line[coordinatesS.X]) != -1 && "J7-".IndexOf(currenentLine[coordinatesS.X + 1]) != -1;

                    }
                    lines.Add(line);
                }

                if (tb)
                {               
                    List<Coordinates> coordinates = new List<Coordinates>();
                    Coordinates coords = new Coordinates()
                    {
                        X = coordinatesS.X,
                        Y = coordinatesS.Y,
                        LastMove = "down",
                        Inside = InsideDirection.Left,
                    };

                    do
                    {
                        var newCoords = new Coordinates()
                        {
                            X = coords.X,
                            Y = coords.Y,
                        };
                        coords.SwitchPos(coords.GetChar(lines));

                        newCoords.Inside = coords.Inside;

                        coordinates.Add(newCoords);
                    } while (coords.X != coordinatesS.X || coords.Y != coordinatesS.Y);

                    var coordsDic = coordinates.ToDictionary(x => (x.Y, x.X));
                    bool[,] map = new bool[lines.Count, lines[0].Length];
                    bool[,] values = new bool[lines.Count, lines[0].Length];
                    
                    foreach (var coord in coordinates)
                    {
                        map[coord.Y, coord.X] = true;
                    }

                    /*
                     * path visualization
                    for (int y = 0; y < map.GetLength(0); y++)
                    {

                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            Console.Write(map[y, x] ? '1' : '0');
                        }

                        Console.Write(Environment.NewLine);
                    }
                    Console.Write(Environment.NewLine);
                    */

                    foreach (var coord in coordinates)
                    {
                        if (coord.Inside.HasFlag(InsideDirection.Up) && !map[coord.Y - 1, coord.X])
                        {
                            int y = coord.Y - 1;
                            while (!map[y, coord.X])
                            {
                                values[y, coord.X] = true;
                                y--;
                            }
                        }
                        if (coord.Inside.HasFlag(InsideDirection.Down) && !map[coord.Y + 1, coord.X])
                        {
                            int y = coord.Y + 1;
                            while (!map[y, coord.X])
                            {
                                values[y, coord.X] = true;
                                y++;
                            }
                        }
                        if (coord.Inside.HasFlag(InsideDirection.Left) && !map[coord.Y, coord.X - 1])
                        {
                            int x = coord.X - 1;
                            while (!map[coord.Y, x])
                            {
                                values[coord.Y, x] = true;
                                x--;
                            }
                        }
                        if (coord.Inside.HasFlag(InsideDirection.Rigth) && !map[coord.Y, coord.X + 1])
                        {
                            int x = coord.X + 1;
                            while (!map[coord.Y, x])
                            {
                                values[coord.Y, x] = true;
                                x++;
                            }
                        }
                    }


                    int result = 0;
                    for (int y = 1; y < values.GetLength(0) - 1; y++)
                    {
                        for (int x = 1; x < values.GetLength(1) - 1; x++)
                        {
                            if (values[y, x]) result++;
                        }
                    }

                    /*
                     result visualization
                    for (int y = 0; y < values.GetLength(0); y++)
                    {

                        for (int x = 0; x < values.GetLength(1); x++)
                        {
                            Console.Write(values[y, x] ? '1' : '0');
                        }

                        Console.Write(Environment.NewLine);
                    }
                    */
                    return result;                    

                }

                return 0;
            }
        }

        private static int NumberOfSteps(Coordinates coords1, Coordinates coords2, List<string> coll)
        {
            int step = 1;

            while (coords1.X != coords2.X || coords1.Y != coords2.Y)
            {
                coords1.SwitchPos(coords1.GetChar(coll));
                coords2.SwitchPos(coords2.GetChar(coll));

                step++;
            }

            return step;
        }



        public class Coordinates
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string LastMove { get; set; }

            public InsideDirection Inside { get; set; }

            private const string up = "up", down = "down", left = "left", right = "right";

            public void SwitchPos(char c)
            {
                switch (c)
                {
                    default:
                        if (c == 'S')
                        {
                            if (LastMove == up)
                            {
                                Y--;
                            }
                            else if (LastMove == down)
                            {
                                Y++;
                            }
                            else
                            {
                                ;
                            }
                        }
                        break;

                    case '|':
                        if (LastMove == up)
                        {
                            Y--;
                        }
                        else if (LastMove == down)
                        {
                            Y++;
                        }
                        else
                        {
                            ;
                        }
                        Inside = Inside.HasFlag(InsideDirection.Left) ? InsideDirection.Left : InsideDirection.Rigth;
                        break;

                    case '-':
                        if (LastMove == left)
                        {
                            X--;
                        }
                        else if (LastMove == right)
                        {
                            X++;
                        }
                        else
                        {
                            ;
                        }
                        Inside = Inside.HasFlag(InsideDirection.Up) ? InsideDirection.Up : InsideDirection.Down;
                        break;

                    case 'L':
                        if (LastMove == down)
                        {
                            X++;
                            LastMove = right;
                            Inside = Inside.HasFlag(InsideDirection.Left) ? InsideDirection.Left | InsideDirection.Down : InsideDirection.Rigth | InsideDirection.Up;
                        }
                        else if (LastMove == left)
                        {
                            Y--;
                            LastMove = up;
                            Inside = Inside.HasFlag(InsideDirection.Up) ? InsideDirection.Up | InsideDirection.Rigth : InsideDirection.Down | InsideDirection.Left;
                        }
                        else
                        {
                            ;
                        }
                        break;

                    case 'J':
                        if (LastMove == down)
                        {
                            X--;
                            LastMove = left;
                            Inside = Inside.HasFlag(InsideDirection.Left) ? InsideDirection.Left | InsideDirection.Up : InsideDirection.Rigth | InsideDirection.Down;
                        }
                        else if (LastMove == right)
                        {
                            Y--;
                            LastMove = up;
                            Inside = Inside.HasFlag(InsideDirection.Up) ? InsideDirection.Up | InsideDirection.Left : InsideDirection.Down | InsideDirection.Rigth;
                        }
                        else
                        {
                            ;
                        }
                        break;

                    case 'F':
                        if (LastMove == up)
                        {
                            X++;
                            LastMove = right;
                            Inside = Inside.HasFlag(InsideDirection.Left) ? InsideDirection.Left | InsideDirection.Up : InsideDirection.Down | InsideDirection.Rigth;
                        }
                        else if (LastMove == left)
                        {
                            Y++;
                            LastMove = down;
                            Inside = Inside.HasFlag(InsideDirection.Up) ? InsideDirection.Left | InsideDirection.Up : InsideDirection.Rigth | InsideDirection.Down;
                        }
                        else
                        {
                            ;
                        }
                        break;

                    case '7':
                        if (LastMove == up)
                        {
                            X--;
                            LastMove = left;
                            Inside = Inside.HasFlag(InsideDirection.Left) ? InsideDirection.Left | InsideDirection.Down : InsideDirection.Rigth | InsideDirection.Up;
                        }
                        else if (LastMove == right)
                        {
                            Y++;
                            LastMove = down;
                            Inside = Inside.HasFlag(InsideDirection.Up) ? InsideDirection.Up | InsideDirection.Rigth : InsideDirection.Down | InsideDirection.Left;
                        }
                        else
                        {
                            ;
                        }
                        break;

                }
            }

            public char GetChar(List<string> coll)
            {
                return coll[Y][X];
            }
        }

        public enum InsideDirection
        {
            Up = 1 << 0,
            Down = 1 << 1,
            Left = 1 << 2,
            Rigth = 1 << 3
        }

    }
}
