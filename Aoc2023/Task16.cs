using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task16
    {

        public static long GetNormal()
        {
            long result = 0;
            bool[,] map = null;

            Dictionary<(int, int), char> nodes = new Dictionary<(int, int), char>();

            using (var reader = new InputReader(16))
            {
                string line;
                int lineNumber = 0;
                int lineLength = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (lineNumber == 0)
                    {
                        lineLength = line.Length;                        
                    }
                    for (int i = 0; i < line.Length; i++)
                    {
                        var c = line[i];
                        if (c != '.')
                        {
                            nodes.Add((lineNumber, i), c);
                        }
                    }
                    lineNumber++;
                }

                map = new bool[lineNumber, lineLength];


                List<Beam> beams = new List<Beam>() { 
                    new Beam()
                    {
                        DirectionX = 1,
                        DirectionY = 0,
                        X = 0,
                        Y = 0,
                    }
                };
                beams[0].InitialPoints.Add((0, 0));
                while (beams.Count > 0)
                {
                    for (int i = 0; i < beams.Count; i++)
                    {
                        var beam = beams[i];
                        TraverseLight(beam, beams, map, nodes);
                    }
                }

                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        Console.Write(map[y, x] ? '#' : '.');
                        if (map[y, x])
                        {                            
                            result++;
                        }
                    }
                    Console.Write(Environment.NewLine);
                }
            }

            return result;
        }

        //brute force
        public static long GetPlatinum()
        {
            long result = 0;
            bool[,] map = null;

            Dictionary<(int, int), char> nodes = new Dictionary<(int, int), char>();

            using (var reader = new InputReader(16))
            {
                string line;
                int lineNumber = 0;
                int lineLength = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (lineNumber == 0)
                    {
                        lineLength = line.Length;
                    }
                    for (int i = 0; i < line.Length; i++)
                    {
                        var c = line[i];
                        if (c != '.')
                        {
                            nodes.Add((lineNumber, i), c);
                        }
                    }
                    lineNumber++;
                }

                map = new bool[lineNumber, lineLength];


                for (int i1 = 0; i1 < lineNumber; i1++)
                {
                    map = new bool[lineNumber, lineLength];
                    long currentResult = 0;
                    List<Beam> beams = new List<Beam>() {
                        new Beam()
                        {
                            DirectionX = 1,
                            DirectionY = 0,
                            X = 0,
                            Y = i1,
                        }
                    };
                    beams[0].InitialPoints.Add((beams[0].Y, beams[0].X));
                    while (beams.Count > 0)
                    {
                        for (int i = 0; i < beams.Count; i++)
                        {
                            var beam = beams[i];
                            TraverseLight(beam, beams, map, nodes);
                        }
                    }

                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            if (map[y, x])
                            {
                                currentResult++;
                            }
                        }
                    }

                    if (currentResult > result)
                    {
                        result = currentResult;
                    }
                }
                for (int i1 = 0; i1 < lineNumber; i1++)
                {
                    map = new bool[lineNumber, lineLength];
                    long currentResult = 0;
                    List<Beam> beams = new List<Beam>() {
                        new Beam()
                        {
                            DirectionX = -1,
                            DirectionY = 0,
                            X = lineLength - 1,
                            Y = i1,
                        }
                    };
                    beams[0].InitialPoints.Add((beams[0].Y, beams[0].X));
                    while (beams.Count > 0)
                    {
                        for (int i = 0; i < beams.Count; i++)
                        {
                            var beam = beams[i];
                            TraverseLight(beam, beams, map, nodes);
                        }
                    }

                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            if (map[y, x])
                            {
                                currentResult++;
                            }
                        }
                    }

                    if (currentResult > result)
                    {
                        result = currentResult;
                    }
                }

                for (int i1 = 0; i1 < lineLength; i1++)
                {
                    map = new bool[lineNumber, lineLength];
                    long currentResult = 0;
                    List<Beam> beams = new List<Beam>() {
                        new Beam()
                        {
                            DirectionX = 0,
                            DirectionY = 1,
                            X = i1,
                            Y = 0,
                        }
                    };
                    beams[0].InitialPoints.Add((beams[0].Y, beams[0].X));
                    while (beams.Count > 0)
                    {
                        for (int i = 0; i < beams.Count; i++)
                        {
                            var beam = beams[i];
                            TraverseLight(beam, beams, map, nodes);
                        }
                    }

                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            if (map[y, x])
                            {
                                currentResult++;
                            }
                        }
                    }

                    if (currentResult > result)
                    {
                        result = currentResult;
                    }
                }

                for (int i1 = 0; i1 < lineLength; i1++)
                {
                    map = new bool[lineNumber, lineLength];
                    long currentResult = 0;
                    List<Beam> beams = new List<Beam>() {
                        new Beam()
                        {
                            DirectionX = 0,
                            DirectionY = -1,
                            X = i1,
                            Y = lineNumber - 1,
                        }
                    };
                    beams[0].InitialPoints.Add((beams[0].Y, beams[0].X));
                    while (beams.Count > 0)
                    {
                        for (int i = 0; i < beams.Count; i++)
                        {
                            var beam = beams[i];
                            TraverseLight(beam, beams, map, nodes);
                        }
                    }

                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            if (map[y, x])
                            {
                                currentResult++;
                            }
                        }
                    }

                    if (currentResult > result)
                    {
                        result = currentResult;
                    }
                }

            }

            return result;
        }

        private static void TraverseLight(Beam beam, List<Beam> beams, bool[,] map, Dictionary<(int, int), char> nodes)
        {
            if (beam.Y >= map.GetLength(0) || beam.Y < 0 || beam.X >= map.GetLength(1) || beam.X < 0)
            {
                beams.Remove(beam);
                return;
            }

            if (nodes.TryGetValue((beam.Y, beam.X), out var c))
            {                
                switch (c)
                {
                    default:
                        throw new NotImplementedException();

                    case '\\':
                        if (beam.DirectionX != 0)
                        {
                            beam.DirectionY += beam.DirectionX;
                            beam.DirectionX = 0;
                        }
                        else
                        {
                            beam.DirectionX += beam.DirectionY;
                            beam.DirectionY = 0;
                        }
                        break;

                    case '/':
                        if (beam.DirectionX != 0)
                        {
                            beam.DirectionY -= beam.DirectionX;
                            beam.DirectionX = 0;
                        }
                        else
                        {
                            beam.DirectionX -= beam.DirectionY;
                            beam.DirectionY = 0;
                        }
                        break;

                    case '|':
                        if (beam.DirectionX != 0)
                        {
                            beam.DirectionX = 0;
                            beam.DirectionY = 1;
                            beam.InitialPoints.Add((beam.Y, beam.X));
                            beams.Add(new Beam()
                            {
                                DirectionX = 0,
                                DirectionY = -1,
                                X = beam.X,
                                Y = beam.Y - 1,
                                InitialPoints = beam.InitialPoints
                            });
                        }
                        break;

                    case '-':
                        if (beam.DirectionY != 0)
                        {
                            beam.DirectionX = 1;
                            beam.DirectionY = 0;
                            beam.InitialPoints.Add((beam.Y, beam.X));
                            beams.Add(new Beam()
                            {
                                DirectionX = -1,
                                DirectionY = 0,
                                X = beam.X - 1,
                                Y = beam.Y,
                                InitialPoints = beam.InitialPoints
                            });
                        }
                        break;
                }
            }

            map[beam.Y, beam.X] = true;

            beam.X += beam.DirectionX;
            beam.Y += beam.DirectionY;    
            
            if (beam.InitialPoints.Contains((beam.Y, beam.X)))
            {
                beams.Remove(beam);
            }
        }

        public class Beam
        {
            public int X { get; set; }
            public int Y { get; set; }
            public short DirectionX { get; set; }
            public short DirectionY { get; set; }

            public HashSet<(int, int)> InitialPoints { get; set; } = new HashSet<(int, int)>();
        }


    }
}
