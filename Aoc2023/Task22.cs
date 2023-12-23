using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task22
    {

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(22))
            {
                List<Brick> bricks = new List<Brick>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split('~');
                    var values_0 = arr[0].Split(',');
                    var values_1 = arr[1].Split(',');

                    Brick brick = new Brick()
                    {
                        X_0 = Convert.ToInt32(values_0[0]),
                        Y_0 = Convert.ToInt32(values_0[1]),
                        Z_0 = Convert.ToInt32(values_0[2]),
                        X_1 = Convert.ToInt32(values_1[0]),
                        Y_1 = Convert.ToInt32(values_1[1]),
                        Z_1 = Convert.ToInt32(values_1[2])
                    };
                    bricks.Add(brick);
                }

                List<Brick> settledBricks = new List<Brick>();
                bricks = bricks.OrderBy(x => x.Z_0).ToList();
                var maxZ = 1000;
                foreach (var brick in bricks)
                {
                    int delta = brick.Z_1 - brick.Z_0;
                    brick.Z_0 = maxZ;
                    brick.Z_1 = brick.Z_0 + delta;
                }
                Dictionary<(int, int, int), Brick> occupiedPositions = new Dictionary<(int, int, int), Brick>();

                foreach (var brick in bricks)
                {
                    var original = new Brick()
                    {
                        X_0 = brick.X_0,
                        Y_0 = brick.Y_0,
                        Z_0 = brick.Z_0,
                        X_1 = brick.X_1,
                        Y_1 = brick.Y_1,
                        Z_1 = brick.Z_1
                    };
                    bool settled = false;

                    while (!settled)
                    {
                        if (brick.Z_0 == 1 || brick.Z_1 == 1)
                        {
                            settled = true;
                            var settledPositions = brick.GetOccupiedPositions();
                            foreach (var position in settledPositions)
                            {
                                occupiedPositions.Add(position, brick);
                            }
                        }
                        else
                        {
                            brick.Z_0--;
                            brick.Z_1--;

                            var positions = brick.GetOccupiedPositions();
                            bool occupied = false;
                            foreach (var position in positions)
                            {
                                if (occupiedPositions.TryGetValue(position, out var dependingOn))
                                {
                                    occupied = true;
                                    if (!brick.DependsOn.Contains(dependingOn))
                                    {
                                        brick.DependsOn.Add(dependingOn);
                                    }
                                    if (!dependingOn.DependsOn.Contains(brick))
                                    {
                                        dependingOn.Dependants.Add(brick);
                                    }
                                }
                            }
                            if (occupied)
                            {
                                settled = true;
                                brick.Z_0++;
                                brick.Z_1++;
                                var settledPositions = brick.GetOccupiedPositions();
                                foreach (var sPosition in settledPositions)
                                {
                                    occupiedPositions.Add(sPosition, brick);
                                }
                            }
                        }
                    }
                }


                return bricks.Where(x => x.Dependants.Count == 0 || x.Dependants.All(x => x.DependsOn.Count > 1)).Count();
            }
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(22))
            {
                List<Brick> bricks = new List<Brick>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split('~');
                    var values_0 = arr[0].Split(',');
                    var values_1 = arr[1].Split(',');

                    Brick brick = new Brick()
                    {
                        X_0 = Convert.ToInt32(values_0[0]),
                        Y_0 = Convert.ToInt32(values_0[1]),
                        Z_0 = Convert.ToInt32(values_0[2]),
                        X_1 = Convert.ToInt32(values_1[0]),
                        Y_1 = Convert.ToInt32(values_1[1]),
                        Z_1 = Convert.ToInt32(values_1[2])
                    };
                    bricks.Add(brick);
                }

                List<Brick> settledBricks = new List<Brick>();
                bricks = bricks.OrderBy(x => x.Z_0).ToList();
                var maxZ = 1000;
                foreach (var brick in bricks)
                {
                    int delta = brick.Z_1 - brick.Z_0;
                    brick.Z_0 = maxZ;
                    brick.Z_1 = brick.Z_0 + delta;
                }
                Dictionary<(int, int, int), Brick> occupiedPositions = new Dictionary<(int, int, int), Brick>();

                foreach (var brick in bricks)
                {
                    var original = new Brick()
                    {
                        X_0 = brick.X_0,
                        Y_0 = brick.Y_0,
                        Z_0 = brick.Z_0,
                        X_1 = brick.X_1,
                        Y_1 = brick.Y_1,
                        Z_1 = brick.Z_1
                    };
                    bool settled = false;

                    while (!settled)
                    {
                        if (brick.Z_0 == 1 || brick.Z_1 == 1)
                        {
                            settled = true;
                            var settledPositions = brick.GetOccupiedPositions();
                            foreach (var position in settledPositions)
                            {
                                occupiedPositions.Add(position, brick);
                            }
                        }
                        else
                        {
                            brick.Z_0--;
                            brick.Z_1--;

                            var positions = brick.GetOccupiedPositions();
                            bool occupied = false;
                            foreach (var position in positions)
                            {
                                if (occupiedPositions.TryGetValue(position, out var dependingOn))
                                {
                                    occupied = true;
                                    if (!brick.DependsOn.Contains(dependingOn))
                                    {
                                        brick.DependsOn.Add(dependingOn);
                                    }
                                    if (!dependingOn.DependsOn.Contains(brick))
                                    {
                                        dependingOn.Dependants.Add(brick);
                                    }
                                }
                            }
                            if (occupied)
                            {
                                settled = true;
                                brick.Z_0++;
                                brick.Z_1++;
                                var settledPositions = brick.GetOccupiedPositions();
                                foreach (var sPosition in settledPositions)
                                {
                                    occupiedPositions.Add(sPosition, brick);
                                }
                            }
                        }
                    }
                }


                Dictionary<Brick, long> brickResults = new Dictionary<Brick, long>();
                var orderedBricks = bricks.OrderByDescending(x => x.Z_0).ToList();
                foreach (var brick in orderedBricks)
                {
                    foreach (var b in orderedBricks)
                    {
                        b.Fallen = false;
                    }
                    brick.Fallen = true;

                    long brickResult = 0;

                    List<Brick> dependants = brick.Dependants;

                    while (dependants.Count > 0)
                    {
                        if (dependants.Count == 1 && brickResults.TryGetValue(dependants[0], out long temp))
                        {
                            if (dependants[0].DependsOn.All(x => x.Fallen))
                            {
                                brickResult += 1 + temp;
                            }
                            dependants.Clear();
                        }
                        else
                        {
                            List<Brick> newDependants = new List<Brick>();

                            foreach (var dependant in dependants)
                            {
                                if (dependant.DependsOn.All(x => x.Fallen))
                                {
                                    dependant.Fallen = true;
                                    newDependants.AddRange(dependant.Dependants);
                                    brickResult++;
                                }
                            }

                            dependants = newDependants.Distinct().ToList();
                        }

                    }
                    brickResults.Add(brick, brickResult);
                    result += brickResult;
                }

            }

            return result;
        }
    }    

    public class Brick
    {
        public int X_0 { get; set; }
        public int X_1 { get; set; }
        public int Y_0 { get; set; }
        public int Y_1 { get; set; }
        public int Z_0 { get; set; }
        public int Z_1 { get; set; }

        public List<Brick> DependsOn { get; init; } = new List<Brick>();
        public List<Brick> Dependants { get; init; } = new List<Brick>();

        public bool Fallen { get; set; }

        public IEnumerable<(int, int, int)> GetOccupiedPositions()
        {
            List<(int, int, int)> result = new List<(int, int, int)>();
            for (int x = X_0; x <= X_1; x++)
            {
                for (int y = Y_0; y <= Y_1; y++)
                {
                    for (int z = Z_0; z <= Z_1; z++)
                    {
                        result.Add((x, y, z));
                    }
                }
            }
            return result;
        }
    }
}


