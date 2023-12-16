using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task15
    {

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(15))
            {
                var values = reader.ReadAll().Replace(Environment.NewLine, string.Empty).Split(',');

                foreach (var value in values)
                {
                    int currentValue = 0;
                    foreach (var c in value)
                    {
                        currentValue += c;
                        currentValue *= 17;
                        currentValue %= 256;
                    }
                    result += currentValue;
                }
            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(15))
            {
                var boxes = new Box[256];
                for (int i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = new Box();
                }

                var values = reader.ReadAll().Replace(Environment.NewLine, string.Empty).Split(',');

                foreach (var value in values)
                {
                    if (value.EndsWith('-'))
                    {
                        var arr = value.Split('-');
                        var boxEntity = new BoxEntity(arr[0]);

                        boxes[boxEntity.GetHashValue()].RemoveEntity(boxEntity.Label);
                    }
                    else
                    {
                        var arr = value.Split('=');
                        var boxEntity = new BoxEntity(arr[0], Convert.ToInt32(arr[1]));

                        boxes[boxEntity.GetHashValue()].AddEntity(boxEntity);
                    }
                }

                for (int i = 0; i < boxes.Length; i++)
                {
                    var box = boxes[i];
                    result += box.GetFocusingPower(i);
                }

            }

            return result;
        }

        public class Box
        {
            private List<BoxEntity> _entities = new List<BoxEntity>();

            public HashSet<string> Labels { get; private set; } = new HashSet<string>();

            public bool Contains(string label)
            {
                return Labels.Contains(label);
            }

            public bool AddEntity(BoxEntity entity)
            {
                if (Contains(entity.Label))
                {
                    var existing = _entities.FirstOrDefault(x => x.Label == entity.Label);
                    if (existing != null)
                    {
                        existing.FocalLength = entity.FocalLength;
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }

                _entities.Add(entity);
                Labels.Add(entity.Label);
                return true;
            }

            public bool RemoveEntity(string label)
            {
                var result = _entities.RemoveAll(x => x.Label == label);
                if (result == 1)
                {
                    Labels.Remove(label);
                }
                return result <= 1;
            }

            public long GetFocusingPower(int index)
            {
                long result = 0;

                for (int i = 0; i < _entities.Count; i++)
                {
                    var entity = _entities[i];
                    result += (1 + index) * (1 + i) * entity.FocalLength; 
                }

                return result;
            }
        }

        public class BoxEntity
        {

            public string Label { get; private set; }
            public int FocalLength { get; set; }

            public BoxEntity(string label, int focalLength = 0)
            {
                Label = label;
                FocalLength = focalLength;
            }

            public int GetHashValue()
            {
                int value = 0;
                foreach (var c in Label)
                {
                    value += c;
                    value *= 17;
                    value %= 256;
                }
                return value;
            }
        }

    }
}
