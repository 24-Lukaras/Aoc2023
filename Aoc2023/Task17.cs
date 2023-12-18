
namespace Aoc2023
{
    internal static class Task17
    {

        public static long GetNormal()
        {
            long result = long.MaxValue;

            using (var reader = new InputReader(17))
            {
                string line;
                int maxX = 0;
                int maxY = 0;

                List<List<byte>> values = new List<List<byte>>();

                while ((line = reader.ReadLine()) != null)
                {
                    maxY++;
                    if (maxX == 0)
                    {
                        maxX = line.Length;
                    }

                    List<byte> coll = new List<byte>();
                    foreach (var c in line)
                    {
                        coll.Add(Convert.ToByte(c.ToString()));
                    }
                    values.Add(coll);
                }

                LavaPath path = new LavaPath();

                for (int y = 0; y < maxY - 1; y++)
                {
                    path.Path.Add(new PathPoint() {
                        Y = y + 1,
                        X = y,
                        Value = values[y + 1][y],
                        Direction = PathPoint.down
                    });
                    path.Path.Add(new PathPoint()
                    {
                        Y = y + 1,
                        X = y + 1,
                        Value = values[y + 1][y + 1],
                        Direction = PathPoint.rigth
                    });
                }

                Console.WriteLine(path.Value);
                Console.WriteLine(path.Value);
                

                /*
                List<Path> paths = new List<Path>() { 
                    new Path()
                    {
                        Y = 0,
                        X = 0                        
                    }
                };

                while (paths.Count > 0)
                {
                    List<Path> newPaths = new List<Path>();

                    foreach (var path in paths)
                    {
                        var movedPaths = path.Move(values, maxY - 1, maxX - 1);
                        foreach (var movedPath in movedPaths)
                        {
                            if (!movedPath.Stale)
                            {
                                if (movedPath.X == maxX - 1 && movedPath.Y == maxY - 1 && movedPath.CurrentValue < result)
                                {
                                    result = movedPath.CurrentValue;
                                }
                                else
                                {
                                    newPaths.Add(movedPath);
                                }
                            }
                        }
                    }

                    newPaths = newPaths.GroupBy(x => x.Point).ToDictionary(x => x.Key, x => x.MinBy(x => x.CurrentValue)).Select(x => x.Value).ToList();

                    paths = newPaths;
                }
                */
            }

            return result;
        }

        public class Path
        {
            public const string up = "up";
            public const string down = "down";
            public const string left = "left";
            public const string rigth = "rigth";
            static readonly string[] availableMoves = new string[4] { up, down, left, rigth };

            public int X { get; set; }
            public int Y { get; set; }
            public (int, int) Point => (Y, X);

            public long CurrentValue { get; set; } = 0;

            public string[] LastMoves { get; private set; } = new string[3];
            public HashSet<(int, int)> Previous { get; private set; } = new HashSet<(int, int)>();
            public bool Stale => Previous.Contains((Y, X));

            public List<Path> Move(List<List<byte>> coll, int maxY, int maxX)
            {
                List<Path> result = new List<Path>();

                var moves = GetAvailableMoves(maxY, maxX);

                foreach (var move in moves)
                {
                    var path = new Path()
                    {
                        X = X,
                        Y = Y,
                        LastMoves = new string[3] { LastMoves[0], LastMoves[1], LastMoves[2] },
                        CurrentValue = CurrentValue,
                        Previous = Previous.ToHashSet(),
                    };
                    path.Move(move, coll);
                    result.Add(path);
                }

                return result;
            }

            public void Move(string move, List<List<byte>> coll)
            {
                Previous.Add((Y, X));
                switch (move)
                {
                    default:
                        throw new NotImplementedException();

                    case up:
                        Y -= 1;                        
                        break;

                    case down:
                        Y += 1;
                        break;

                    case left:
                        X -= 1;
                        break;

                    case rigth:
                        X += 1;
                        break;
                }

                CurrentValue += coll[Y][X];
                LastMoves[0] = LastMoves[1];
                LastMoves[1] = LastMoves[2];
                LastMoves[2] = move;
            }

            public List<string> GetAvailableMoves(int maxY, int maxX)
            {
                List<string> result = new List<string>();

                foreach (var move in availableMoves)
                {
                    if (!LastMoves.All(x => x == move))
                    {
                        switch (move)
                        {
                            default:
                                throw new NotImplementedException();

                            case up:
                                if (LastMoves[2] != down && Y > 0)
                                {
                                    result.Add(up);
                                }
                                break;

                            case down:
                                if (LastMoves[2] != up && Y < maxY)
                                {
                                    result.Add(down);
                                }
                                break;

                            case left:
                                if (LastMoves[2] != rigth && X > 0)
                                {
                                    result.Add(left);
                                }
                                break;

                            case rigth:
                                if (LastMoves[2] != left && X < maxX)
                                {
                                    result.Add(rigth);
                                }
                                break;
                        }
                    }
                }

                return result;
            }
        }


        public class LavaPath
        {
            public List<PathPoint> Path { get; private set; } = new List<PathPoint> ();

            public long Value => Path.Sum(x => x.Value);
        }

        public class PathPoint
        {
            public const string up = "up";
            public const string down = "down";
            public const string left = "left";
            public const string rigth = "rigth";

            public int X { get; set; }
            public int Y { get; set; }
            public int Value { get; set; }

            public string Direction { get; set; }
        }
    }
}
