using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task19
    {

        private static readonly Regex xReg = new Regex(@"(?<=x=)\d+(?=,)");
        private static readonly Regex mReg = new Regex(@"(?<=m=)\d+(?=,)");
        private static readonly Regex aReg = new Regex(@"(?<=a=)\d+(?=,)");
        private static readonly Regex sReg = new Regex(@"(?<=s=)\d+(?=})");
        private static readonly Regex conditionsBody = new Regex(@"(?<={).+(?=})");

        public static long GetNormal()
        {
            long result = 0;

            bool conditions = true;
            using (var reader = new InputReader(19))
            {
                List<Part> parts = new List<Part>();
                Dictionary<string, PartWorkflow> workflows = new Dictionary<string, PartWorkflow>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        conditions = false;
                        line = reader.ReadLine();
                    }

                    if (conditions)
                    {
                        string name = line.Substring(0, line.IndexOf('{'));
                        var cBody = conditionsBody.Match(line).Value;
                        var arr = cBody.Split(',');
                        string defaultCondition = arr.Last();
                        List<WorkflowCondition> wConditions = new List<WorkflowCondition>();

                        for (int i = 0; i < arr.Length - 1; i++)
                        {
                            var arr1 = arr[i].Split(':');
                            WorkflowCondition condition = new WorkflowCondition(arr1[0][0], arr1[0][1] == '<', Convert.ToInt32(arr1[0].Substring(2)), arr1[1]);
                            wConditions.Add(condition);
                        }

                        PartWorkflow workflow = new PartWorkflow(name, defaultCondition, wConditions);
                        workflows.Add(workflow.name, workflow);
                    }
                    else
                    {
                        int x = Convert.ToInt32(xReg.Match(line).Value);
                        int m = Convert.ToInt32(mReg.Match(line).Value);
                        int a = Convert.ToInt32(aReg.Match(line).Value);
                        int s = Convert.ToInt32(sReg.Match(line).Value);

                        var part = new Part(x, m, a, s);
                        parts.Add(part);
                    }
                }

                foreach (var part in parts)
                {
                    result += workflows["in"].MatchPart(part, workflows);
                }
            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            bool conditions = true;
            using (var reader = new InputReader(19))
            {
                Dictionary<string, PartWorkflow> workflows = new Dictionary<string, PartWorkflow>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        conditions = false;
                        line = reader.ReadLine();
                    }

                    if (conditions)
                    {
                        string name = line.Substring(0, line.IndexOf('{'));
                        var cBody = conditionsBody.Match(line).Value;
                        var arr = cBody.Split(',');
                        string defaultCondition = arr.Last();
                        List<WorkflowCondition> wConditions = new List<WorkflowCondition>();

                        for (int i = 0; i < arr.Length - 1; i++)
                        {
                            var arr1 = arr[i].Split(':');
                            WorkflowCondition condition = new WorkflowCondition(arr1[0][0], arr1[0][1] == '<', Convert.ToInt32(arr1[0].Substring(2)), arr1[1]);
                            wConditions.Add(condition);
                        }

                        PartWorkflow workflow = new PartWorkflow(name, defaultCondition, wConditions);
                        workflows.Add(workflow.name, workflow);
                    }
                }

                List<QuantumPart> parts = new List<QuantumPart>() { new QuantumPart() };

                while (parts.Count > 0)
                {
                    var part = parts[0];
                    if (part.Workflow == "A")
                    {
                        long value = part.Value();
                        result += value;
                        parts.Remove(part);
                    }
                    else if (part.Workflow == "R")
                    {
                        parts.Remove(part);
                    }
                    else
                    {
                        part.MatchWorkflow(workflows[part.Workflow], parts);
                    }
                }

            }

            return result;
        }

    }

    public record PartWorkflow(string name, string defaultCondition, List<WorkflowCondition> conditions)
    {
        public long MatchPart(Part part, Dictionary<string, PartWorkflow> workflows)
        {
            string wResult = string.Empty;

            foreach (var condition in conditions)
            {
                if (condition.MatchPart(part))
                {
                    wResult = condition.result;
                    break;
                }
            }

            if (string.IsNullOrEmpty(wResult))
            {
                wResult = defaultCondition;
            }

            if (wResult == "R")
            {
                return 0;
            }
            else if (wResult == "A")
            {
                return part.Value;
            }
            else
            {
                return workflows[wResult].MatchPart(part, workflows);
            }
        }
    };

    public record WorkflowCondition(char property, bool less, int value, string result)
    {
        public bool MatchPart(Part part)
        {
            int val;
            switch (property)
            {
                default: return false;

                case 'x':
                    val = part.x;
                    break;

                case 'm':
                    val = part.m;
                    break;

                case 'a':
                    val = part.a;
                    break;

                case 's':
                    val = part.s;
                    break;
            }

            return (less && val < value) || (!less && val > value);
        }
    }

    public record Part(int x, int m, int a, int s)
    {
        public long Value => x + m + a + s;
    }

    public class QuantumPart
    {
        public string Workflow { get; set; } = "in";
        public int MinX { get; set; } = 1;
        public int MaxX { get; set; } = 4000;
        public int MinM { get; set; } = 1;
        public int MaxM { get; set; } = 4000;
        public int MinA { get; set; } = 1;
        public int MaxA { get; set; } = 4000;
        public int MinS { get; set; } = 1;
        public int MaxS { get; set; } = 4000;

        public long Value() => (long)(MaxX - MinX + 1) * (long)(MaxM - MinM + 1) * (long)(MaxA - MinA + 1) * (long)(MaxS - MinS + 1);

        public static QuantumPart CopyPart(QuantumPart original)
        {
            return new QuantumPart()
            {
                Workflow = original.Workflow,
                MinX = original.MinX,
                MinA = original.MinA,
                MinM = original.MinM,
                MinS = original.MinS,
                MaxX = original.MaxX,
                MaxA = original.MaxA,
                MaxM = original.MaxM,
                MaxS = original.MaxS,
            };
        }

        public void MatchWorkflow(PartWorkflow workflow, List<QuantumPart> parts)
        {
            foreach (var condition in workflow.conditions)
            {
                int value = condition.value;
                var copy = CopyPart(this);
                copy.Workflow = condition.result;

                switch (condition.property)
                {
                    default:
                        throw new NotImplementedException();

                    case 'x':
                        if (condition.less)
                        {
                            MinX = value;
                            copy.MaxX = value - 1;
                        }
                        else
                        {
                            MaxX = value;
                            copy.MinX = value + 1;
                        }
                        break;

                    case 'm':
                        if (condition.less)
                        {
                            MinM = value;
                            copy.MaxM = value - 1;
                        }
                        else
                        {
                            MaxM = value;
                            copy.MinM = value + 1;
                        }
                        break;

                    case 'a':
                        if (condition.less)
                        {
                            MinA = value;
                            copy.MaxA = value - 1;
                        }
                        else
                        {
                            MaxA = value;
                            copy.MinA = value + 1;
                        }
                        break;

                    case 's':
                        if (condition.less)
                        {
                            MinS = value;
                            copy.MaxS = value - 1;
                        }
                        else
                        {
                            MaxS = value;
                            copy.MinS = value + 1;
                        }
                        break;
                }

                parts.Add(copy);
            }

            Workflow = workflow.defaultCondition;
        }
    }
}
