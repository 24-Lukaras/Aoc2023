using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aoc2023
{
    internal static class Task08
    {

        public static int GetNormal()
        {
            using (InputReader reader = new InputReader(8))
            {
                string instructions = reader.ReadLine();

                string line = reader.ReadLine();
                Dictionary<string, Node> nodes = new Dictionary<string, Node>();

                while ((line = reader.ReadLine()) != null)
                {
                    nodes.Add(line.Substring(0, 3), new Node(line.Substring(7,3), line.Substring(12, 3)));
                }

                string start = "AAA";
                string end = "ZZZ";

                string node = start;
                int step = 0;
                while (node != end)
                {
                    int index = step % instructions.Length;
                    step++;

                    var newNode = nodes.GetValueOrDefault(node);
                    if (instructions[index] == 'L')
                    {
                        node = newNode.left;
                    }
                    else
                    {
                        node = newNode.right;
                    }
                    
                }
                return step;
            }
        }

        public static long GetPlatinum()
        {
            using (InputReader reader = new InputReader(8))
            {
                string instructions = reader.ReadLine();

                string line = reader.ReadLine();
                Dictionary<string, Node> nodes = new Dictionary<string, Node>();

                while ((line = reader.ReadLine()) != null)
                {
                    nodes.Add(line.Substring(0, 3), new Node(line.Substring(7, 3), line.Substring(12, 3)));
                }

                List<int> results = new List<int>();

                string[] currentNodes = nodes.Where(x => x.Key[2] == 'A').Select(x => x.Key).ToArray();    
                for (int i = 0; i < currentNodes.Length; i++)
                {
                    string node = currentNodes[i];
                    int step = 0;
                    while (node[2] != 'Z')
                    {
                        int index = step % instructions.Length;
                        step++;

                        var newNode = nodes.GetValueOrDefault(node);
                        if (instructions[index] == 'L')
                        {
                            node = newNode.left;
                        }
                        else
                        {
                            node = newNode.right;
                        }
                    }
                    results.Add(step);
                }
                
                long lcmTemp = results[0];
                for (int i = 1; i < results.Count; i++)
                {
                    lcmTemp = FindLCM(lcmTemp, Convert.ToInt64(results[i]));
                }
                return lcmTemp;
                
                /*
                 * my actuall solution which takes up ~20 min to process
                 * found lcm while searching for a hint
                 * 
                
                long[] temp = results.Select(x => Convert.ToInt64(x)).ToArray();

                while (temp.Distinct().Count() > 1)
                {
                    long min = temp.Min();

                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i] == min)
                        {
                            temp[i] += results[i];
                        }
                    }
                }
                return temp[0];
                */
            }
        }

        private static long FindLCM(long a, long b)
        {
            long num1, num2;

            if (a > b)
            {
                num1 = a;
                num2 = b;
            }
            else
            {
                num1 = b;
                num2 = a;
            }

            for (int i = 1; i <= num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num2;
        }
    }


    public record Node(string left, string right);

    public record Loop(string node, int instructionIndex, int firstHit, int size);
}
