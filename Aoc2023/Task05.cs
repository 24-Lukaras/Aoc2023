using System;
using System.Text.RegularExpressions;

namespace Aoc2023
{
    internal static class Task05
    {

        static Regex digitRegex = new Regex(@"\d+");
        static int layer = 0;

        public static long GetNormal()
        {
            long result = 0;
            using (InputReader reader = new InputReader(5))
            {
                string line = reader.ReadLine();
                long[] seeds = digitRegex.Matches(line).Select(x => Convert.ToInt64(x.Value)).ToArray();

                result = GetLowestForSeeds(reader, seeds);
            }
            return result;
        }

        public static long GetPlatinum()
        {
            long result = long.MaxValue;
            int layer = 0;
            using (InputReader reader = new InputReader(5))
            {
                List<(long, long)> originalSeeds = new List<(long, long)>();
                string line = reader.ReadLine();
                var matches = digitRegex.Matches(line);
                for (int i = 0; i < matches.Count; i += 2)
                {
                    long sMin = Convert.ToInt64(matches[i].Value);
                    originalSeeds.Add((sMin, sMin + Convert.ToInt64(matches[i + 1].Value) - 1));
                }
                List<(long, long)> seedsCopy = originalSeeds.ToList();

                List<SourceDestinationMap> almanachMaps = new List<SourceDestinationMap>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        if (almanachMaps.Count > 0)
                        {
                            ProcessAlmanachMap(almanachMaps, ref seedsCopy);
                        }
                        reader.ReadLine();
                    }
                    else
                    {
                        var m = digitRegex.Matches(line);
                        long sourceMin = Convert.ToInt64(m[1].Value);
                        SourceDestinationMap map = new SourceDestinationMap()
                        {
                            SourceMin = sourceMin,
                            SourceMax = sourceMin + Convert.ToInt64(m[2].Value),
                            Shift = Convert.ToInt64(m[0].Value) - sourceMin
                        };
                        almanachMaps.Add(map);
                    }
                }

                ProcessAlmanachMap(almanachMaps, ref seedsCopy);

                var temp = seedsCopy.OrderBy(x => x.Item1).ToArray();
                return temp.First().Item1;

            }
            return result;
        }

        private static long GetLowestForSeeds(InputReader reader, IEnumerable<long> seeds)
        {
            List<List<SourceDestinationMap>> almanachMaps = new List<List<SourceDestinationMap>>();
            List<SourceDestinationMap> currentMapColl = null;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    currentMapColl = new List<SourceDestinationMap>();
                    almanachMaps.Add(currentMapColl);
                    reader.ReadLine();
                }
                else
                {
                    var matches = digitRegex.Matches(line);
                    long sourceMin = Convert.ToInt64(matches[1].Value);
                    SourceDestinationMap map = new SourceDestinationMap()
                    {
                        SourceMin = sourceMin,
                        SourceMax = sourceMin + Convert.ToInt64(matches[2].Value),
                        Shift = Convert.ToInt64(matches[0].Value) - sourceMin
                    };
                    currentMapColl.Add(map);
                }
            }

            long lowest = long.MaxValue;
            foreach (var seed in seeds)
            {
                long current = seed;

                foreach (var mapColl in almanachMaps)
                {
                    var map = mapColl.Where(x => x.SourceMin <= current && x.SourceMax >= current);
                    if (map.Count() > 1)
                    {
                        ;
                    }
                    else if (map.Count() == 1)
                    {
                        current += map.First().Shift;
                    }
                }

                if (current < lowest)
                {
                    lowest = current;
                }
            }
            return lowest;
        }

        private static void ProcessAlmanachMap(List<SourceDestinationMap> almanachMaps, ref List<(long, long)> seedsCopy)
        {
            layer++;
            List<(long, long)> newEntries = new List<(long, long)>();
            for (int i = 0; i < seedsCopy.Count; i++)
            {
                var seeds = seedsCopy[i];
                TryProcessUnprocessed(seeds, almanachMaps, ref newEntries);
            }
            seedsCopy = newEntries.OrderBy(x => x.Item1).ToList();

            for (int i = 0; i < seedsCopy.Count; i++)
            {
                var seed = seedsCopy[i];
                CustomRange<long> range = new CustomRange<long>(seed.Item1, seed.Item2);

                var overlapping1 = seedsCopy.Skip(i + 1).Where(x => (x.Item1 <= seed.Item1 && x.Item2 >= seed.Item1) || (x.Item2 >= seed.Item2 && x.Item1 <= seed.Item2) || (x.Item1 >= seed.Item1 && x.Item2 <= seed.Item2)).ToArray();
                var overlapping = seedsCopy.Skip(i + 1).Where(x => new CustomRange<long>(x.Item1, x.Item2).IsOverlapped(range)).ToArray();

                long min = seed.Item1;
                long max = seed.Item2;

                foreach (var o in overlapping)
                {
                    if (o.Item1 < min)
                    {
                        min = o.Item1;
                    }
                    if (o.Item2 > max)
                    {
                        max = o.Item2;
                    }
                    seedsCopy.Remove(o);
                }

                seedsCopy[i] = (min, max);
            }
            almanachMaps.Clear();
        }

        static void TryProcessUnprocessed((long, long) seeds, List<SourceDestinationMap> almanachMaps, ref List<(long, long)> newEntries)
        {
            CustomRange<long> range = new CustomRange<long>(seeds.Item1, seeds.Item2);
            var maps = almanachMaps.Where(x => new CustomRange<long>(x.SourceMin, x.SourceMax).IsOverlapped(range)).OrderBy(x => x.SourceMin);

            if (maps.Any())
            {
                if (maps.First().SourceMin > seeds.Item1)
                {
                    newEntries.AddCustom((seeds.Item1, maps.First().SourceMin - 1));
                }
                foreach (var map in maps)
                {
                    long min = ((map.SourceMin < seeds.Item1) ? seeds.Item1 : map.SourceMin) + map.Shift;
                    long max = ((map.SourceMax > seeds.Item2) ? seeds.Item2 : map.SourceMax) + map.Shift;
                    newEntries.AddCustom((min, max));

                    if (seeds.Item1 < map.SourceMin)
                    {
                        TryProcessUnprocessed((seeds.Item1, map.SourceMin - 1), almanachMaps, ref newEntries);
                    }
                    if (seeds.Item2 > map.SourceMax)
                    {
                        TryProcessUnprocessed((map.SourceMax + 1, seeds.Item2), almanachMaps, ref newEntries);
                    }
                }
                if (maps.Last().SourceMin > seeds.Item2)
                {
                    newEntries.AddCustom((maps.Last().SourceMax + 1, seeds.Item2));
                }
            }
            else
            {
                newEntries.AddCustom(seeds);
            }
        }

        static void AddCustom(this List<(long, long)> coll, (long, long) item)
        {
            coll.Add(item);
        }

        public class SourceDestinationMap
        {
            public long SourceMin { get; init; }
            public long SourceMax { get; init; }
            public long Shift { get; init; }
        }

        public class CustomRange<T> where T : IComparable
        {
            readonly T min;
            readonly T max;

            public CustomRange(T min, T max)
            {
                this.min = min;
                this.max = max;
            }

            public bool IsOverlapped(CustomRange<T> other)
            {
                return Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;
            }

            public T Min { get { return min; } }
            public T Max { get { return max; } }
        }
    }
}
