using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task23
    {

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(23))
            {
                (int, int) goal = (0, 0);
                Dictionary<(int, int), Direction?> trailSpaces = new Dictionary<(int, int), Direction?>();
                int lineNum = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (goal.Item1 == 0)
                    {
                        goal = (line.Length - 2, 0);
                    }
                    for (int charNum = 0; charNum < line.Length; charNum++)
                    {
                        if (line[charNum] != '#')
                        {
                            Direction? direction = null;
                            switch (line[charNum])
                            {
                                default:
                                    break;

                                case '>':
                                    direction = Direction.Right;
                                    break;

                                case '<':
                                    direction = Direction.Left;
                                    break;

                                case '^':
                                    direction = Direction.Up;
                                    break;

                                case 'v':
                                    direction = Direction.Down;
                                    break;
                            }
                            trailSpaces.Add((charNum, lineNum), direction);
                        }
                    }
                    lineNum++;
                }
                goal = (goal.Item1, lineNum - 1);

                List<Hike> hikes = new List<Hike>() { new Hike() { X = 1, Y = 0, CurrentSpace = null } };

                while (hikes.Count > 0)
                {
                    List<Hike> newHikes = new List<Hike>();

                    for (int i = hikes.Count - 1; i >= 0; i--)
                    {
                        bool defaultModified = false;

                        if ((hikes[i].X, hikes[i].Y) == goal)
                        {
                            if (hikes[i].Steps > result)
                            {
                                result = hikes[i].Steps;
                            }
                            hikes.RemoveAt(i);
                        }
                        else if (hikes[i].CurrentSpace != null)
                        {
                            if (!hikes[i].TryApplyDirection(hikes[i].CurrentSpace, trailSpaces))
                            {
                                hikes.Remove(hikes[i]);
                            }
                        }
                        else
                        {
                            Hike hike = hikes[i];
                            hikes.Remove(hike);

                            var copy = hike.Copy();
                            if (copy.TryApplyDirection(Direction.Left, trailSpaces))
                            {
                                newHikes.Add(copy);
                            }
                            copy = hike.Copy();
                            if (copy.TryApplyDirection(Direction.Right, trailSpaces))
                            {
                                newHikes.Add(copy);
                            }
                            copy = hike.Copy();
                            if (copy.TryApplyDirection(Direction.Up, trailSpaces))
                            {
                                newHikes.Add(copy);
                            }
                            copy = hike.Copy();
                            if (copy.TryApplyDirection(Direction.Down, trailSpaces))
                            {
                                newHikes.Add(copy);
                            }
                        }
                    }

                    hikes.AddRange(newHikes);
                }
            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(23))
            {
                (int, int) goal = (0, 0);
                Dictionary<(int, int), bool> trailSpaces = new Dictionary<(int, int), bool>();
                int lineNum = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (goal.Item1 == 0)
                    {
                        goal = (line.Length - 2, 0);
                    }
                    for (int charNum = 0; charNum < line.Length; charNum++)
                    {
                        if (line[charNum] != '#')
                        {
                            trailSpaces.Add((charNum, lineNum), false);
                        }
                    }
                    lineNum++;
                }
                goal = (goal.Item1, lineNum - 1);
                
                var startIntersection = new Intersection() { X = 1, Y = 0, Directions = new List<Direction>() { Direction.Down } };
                var goalIntersection = new Intersection() { X = goal.Item1, Y = goal.Item2, Directions = new List<Direction>() { Direction.Up } };                
                List<Intersection> intersections = new List<Intersection>()
                {
                    startIntersection,
                    goalIntersection
                };

                foreach (var coords in trailSpaces.Keys)
                {
                    List<Direction> directions = new List<Direction>();

                    if (trailSpaces.TryGetValue((coords.Item1 - 1, coords.Item2), out _))
                    {
                        directions.Add(Direction.Left);
                    }
                    if (trailSpaces.TryGetValue((coords.Item1 + 1, coords.Item2), out _))
                    {
                        directions.Add(Direction.Right);
                    }
                    if (trailSpaces.TryGetValue((coords.Item1, coords.Item2 + 1), out _))
                    {
                        directions.Add(Direction.Down);
                    }
                    if (trailSpaces.TryGetValue((coords.Item1, coords.Item2 - 1), out _))
                    {
                        directions.Add(Direction.Up);
                    }

                    if (directions.Count > 2)
                    {
                        intersections.Add(new Intersection()
                        {
                            X = coords.Item1,
                            Y = coords.Item2,
                            Directions = directions
                        });
                    }
                }

                var intersectionDic = intersections.ToDictionary(x => (x.X, x.Y), x => x);
                
                List<IntersectionPath> intersectionPaths = new List<IntersectionPath>();
                foreach (var intersection in intersections)
                {
                    foreach (var direction in intersection.Directions)
                    {
                        Intersection destination = null;
                        Hike hike = new Hike() { X = intersection.X, Y = intersection.Y, CurrentSpace = direction };
                        IntersectionPath path = new IntersectionPath()
                        {
                            Intersection1 = intersection,
                            Movement1 = direction,
                        };
                        hike.TryApplyDirection(hike.CurrentSpace, trailSpaces);
                        while (destination == null)
                        {
                            if (intersectionDic.TryGetValue((hike.X, hike.Y), out destination))
                            {
                            }
                            else if (hike.TryApplyDirection(Direction.Left, trailSpaces))
                            {
                                hike.CurrentSpace = Direction.Left;
                            }
                            else if (hike.TryApplyDirection(Direction.Right, trailSpaces))
                            {
                                hike.CurrentSpace = Direction.Right;
                            }
                            else if (hike.TryApplyDirection(Direction.Up, trailSpaces))
                            {
                                hike.CurrentSpace = Direction.Up;
                            }
                            else if (hike.TryApplyDirection(Direction.Down, trailSpaces))
                            {
                                hike.CurrentSpace = Direction.Down;
                            }
                        }
                        path.Intersection2 = destination;
                        if (hike.CurrentSpace == Direction.Left)
                        {
                            path.Movement2 = Direction.Right;                            
                        }
                        else if (hike.CurrentSpace == Direction.Right)
                        {
                            path.Movement2 = Direction.Left;
                        }
                        else if (hike.CurrentSpace == Direction.Up)
                        {

                            path.Movement2 = Direction.Down;
                        }
                        else
                        {
                            path.Movement2 = Direction.Up;
                        }
                        path.Steps = hike.Steps;
                        intersectionPaths.Add(path);
                    }
                }

                var pathDic = intersectionPaths.GroupBy(x => x.Intersection1).ToDictionary(x => x.Key, x => x.ToList());



                /*
                List<IntersectionPathResult> pathResults = new List<IntersectionPathResult>() 
                { 
                    new IntersectionPathResult() {
                        Intersection = startIntersection
                    }
                };

                int complete = 0;
                IntersectionPathResult res = null;
                while (pathResults.Count > 0)
                {
                    List<IntersectionPathResult> newResults = new List<IntersectionPathResult>();

                    for (int i = pathResults.Count - 1; i >= 0; i--)
                    {
                        var pathResult = pathResults[i];
                        pathResults.RemoveAt(i);

                        if (pathResult.Intersection == goalIntersection)
                        {
                            if (pathResult.Steps + 1 > result)
                            {
                                complete++;
                                result = pathResult.Steps + 1;
                                res = pathResult;
                            }
                        }
                        else
                        {
                            var currentIntersectionPaths = pathDic[pathResult.Intersection];

                            foreach (var path in currentIntersectionPaths)
                            {
                                if (!pathResult.UsedPaths.Contains(path.Key) && !pathResult.Intersections.Contains(path.Intersection2))
                                {
                                    var copy = pathResult.Copy();
                                    copy.UsedPaths.Add(path.Key);
                                    copy.Intersections.Add(path.Intersection2);
                                    copy.Intersection = path.Intersection2;
                                    copy.Steps += path.Steps;
                                    newResults.Add(copy);
                                }
                            }
                        }

                    }
                    if (newResults.Count > 400_000)
                    {
                        var temp = newResults.OrderByDescending(x => Convert.ToDouble(x.Steps) / (x.Intersection.X * x.Intersection.Y));

                        newResults = temp.Take(200_000).ToList();
                        newResults.AddRange(temp.Reverse().Take(200_000));
                    }
                    pathResults = newResults;
                }
                */
            }

            return result;
        }
    }


    public class Direction
    {
        public int X { get; init; }
        public int Y { get; init; }

        public static readonly Direction Left = new Direction()
        {
            X = -1,
            Y = 0,
        };

        public static readonly Direction Right = new Direction()
        {
            X = 1,
            Y = 0,
        };

        public static readonly Direction Up = new Direction()
        {
            X = 0,
            Y = -1,
        };

        public static readonly Direction Down = new Direction()
        {
            X = 0,
            Y = 1,
        };
    }

    public class Hike
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction? CurrentSpace { get; set; }
        public HashSet<(int, int)> PreviousPath { get; private set; } = new HashSet<(int, int)>();
        public long Steps { get; private set; }

        public bool TryApplyDirection(Direction direction, Dictionary<(int, int), Direction?> trailSpaces)
        {
            if (trailSpaces.TryGetValue((X + direction.X, Y + direction.Y), out var newDirection))
            {
                PreviousPath.Add((X, Y));
                CurrentSpace = newDirection;
                X += direction.X;
                Y += direction.Y;

                if (PreviousPath.Contains((X, Y)))
                {
                    return false;
                }

                Steps++;
                return true;
            }
            return false;
        }

        public bool TryApplyDirection(Direction direction, Dictionary<(int, int), bool> trailSpaces)
        {
            if (trailSpaces.TryGetValue((X + direction.X, Y + direction.Y), out var newDirection))
            {
                PreviousPath.Add((X, Y));
                X += direction.X;
                Y += direction.Y;

                if (PreviousPath.Contains((X, Y)))
                {
                    X -= direction.X;
                    Y -= direction.Y;
                    return false;
                }

                Steps++;
                return true;
            }
            return false;
        }

        public Hike Copy()
        {
            return new Hike()
            {
                X = X,
                Y = Y,
                CurrentSpace = CurrentSpace,
                PreviousPath = PreviousPath.ToHashSet(),
                Steps = Steps
            };
        }
    }

    public class IntersectionPathResult
    {
        public Intersection Intersection { get; set; }
        public HashSet<string> UsedPaths { get; private set; } = new HashSet<string>();
        public HashSet<Intersection> Intersections { get; private set; } = new HashSet<Intersection>();
        public long Steps { get; set; }

        public bool Complete { get; set; }

        public IntersectionPathResult Copy()
        {
            return new IntersectionPathResult()
            {
                Intersection = Intersection,
                UsedPaths = UsedPaths.ToHashSet(),
                Intersections = Intersections.ToHashSet(),
                Steps = Steps,
                Complete = Complete
            };
        }

    }

    public class IntersectionPath
    {
        public Intersection Intersection1 { get; set; }
        public Direction Movement1 { get; set; }
        public Intersection Intersection2 { get; set; }
        public Direction Movement2 { get; set; }
        public long Steps { get; set; }

        private string _key;
        public string Key
        {
            get
            {
                if (_key != null)
                {
                    return _key;
                }

                Intersection i1;
                Intersection i2;

                if (Intersection1.X < Intersection2.X)
                {
                    i1 = Intersection1;
                    i2 = Intersection2;                    
                }
                else if (Intersection1.X > Intersection2.X)
                {
                    i1 = Intersection2;
                    i2 = Intersection1;
                }
                else
                {
                    if (Intersection1.Y < Intersection2.Y)
                    {
                        i1 = Intersection1;
                        i2 = Intersection2;
                    }
                    else
                    {
                        i1 = Intersection2;
                        i2 = Intersection1;
                    }
                }

                _key = $"{i1.X},{i1.Y};{i2.X},{i2.Y}";
                return _key;
            }
        }
    }

    public class Intersection
    {
        public int X { get; init; }
        public int Y { get; init; }
        public List<Direction> Directions { get; set; } = new List<Direction>();
        public int LowestValue { get; set; } = int.MaxValue;
    }
}
