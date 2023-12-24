using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task24
    {

        public static long GetNormal()
        {
            long result = 0;

            long testAreaMin = 200000000000000;
            long testAreaMax = 400000000000000;

            List<Vector> hails = new List<Vector>();

            using (var reader = new InputReader(24))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(" @");
                    var arr1 = arr[0].Split(", ");
                    var arr2 = arr[1].Split(", ");

                    Vector hail = new Vector()
                    {
                        X = Int128.Parse(arr1[0]),
                        Y = Int128.Parse(arr1[1]),
                        Z = Int128.Parse(arr1[2]),

                        Vx = int.Parse(arr2[0]),
                        Vy = int.Parse(arr2[1]),
                        Vz = int.Parse(arr2[2])
                    };
                    hails.Add(hail);
                }
            }

            for (int i = 0; i < hails.Count; i++)
            {
                var hail = hails[i];

                for (int j = i + 1; j < hails.Count; j++)
                {
                    if (hail.TryIntersect2D(hails[j], out var point))
                    {
                        if (point.Item1 >= testAreaMin && point.Item1 <= testAreaMax && point.Item2 >= testAreaMin && point.Item2 <= testAreaMax)
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        public class Vector
        {
            public Int128 X { get; set; }
            public Int128 Y { get; set; }
            public Int128 Z { get; set; }

            public int Vx { get; set; }
            public int Vy { get; set; }
            public int Vz { get; set; }


            public bool TryIntersect2D(Vector other, out (Int128, Int128) point)
            {
                point = LineIntersect(
                    (X, Y),
                    (X + Vx, Y + Vy),
                    (other.X, other.Y),
                    (other.X + other.Vx, other.Y + other.Vy)
                    );

                if (point == default)
                {
                    return false;
                }

                var t1 = (point.Item1 - X) / Vx;
                var t2 = (point.Item1 - other.X) / other.Vx;

                return t1 > 0 && t2 > 0;
            }

            private (Int128, Int128) LineIntersect((Int128, Int128) p0, (Int128, Int128) p1, (Int128, Int128) p2, (Int128, Int128) p3)
            {
                var a1 = p1.Item2 - p0.Item2;
                var b1 = p0.Item1 - p1.Item1;
                var c1 = a1 * p0.Item1 + b1 * p0.Item2;
                var a2 = p3.Item2 - p2.Item2;
                var b2 = p2.Item1 - p3.Item1;
                var c2 = a2 * p2.Item1 + b2 * p2.Item2;
                var denominator = a1 * b2 - a2 * b1;

                if (denominator != 0)
                {
                    return ((b2 * c1 - b1 * c2) / denominator, (a1 * c2 - a2 * c1) / denominator);
                }
                return default;
            }
            
            /*
            public bool TryIntersect(Vector other, out (decimal, decimal) point)
            {
                var factor = -(Convert.ToDecimal(Vx) / Vy);

                var yCalcFactor = other.Vx + (other.Vy * factor);
                var thisFactor = Convert.ToInt64(((X - other.X) - (Y - other.Y))) / yCalcFactor;
                var otherFactor = (Convert.ToInt64(X - other.X) - (other.Vx * thisFactor)) / -Vx;

                point = (X + Convert.ToInt64(Vx * -thisFactor), Y + Convert.ToInt64(Vy * otherFactor));

                return (Vz * thisFactor) - (other.Vz * otherFactor) == -(Z - other.Z);
            }
            */
            
        }

    }
}
