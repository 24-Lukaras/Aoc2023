using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task20
    {

        public static long GetNormal()
        {
            using (var reader = new InputReader(20))
            {
                Dictionary<string, Module> modules = new Dictionary<string, Module>();

                Module broadcast = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(" -> ");
                    var targets = arr[1].Split(", ");

                    if (arr[0] == "broadcaster")
                    {
                        broadcast = new Module()
                        {
                            Name = "broadcaster",
                            FlipFlop = false,
                            Targets = targets,
                        };
                    }
                    else
                    {
                        var module = new Module()
                        {
                            Name = arr[0].Substring(1),
                            FlipFlop = arr[0][0] == '%',
                            Targets = targets,
                        };
                        modules.Add(module.Name, module);
                    }
                }

                foreach (var module in modules.Values)
                {
                    if (!module.FlipFlop)
                    {
                        var senders = modules.Values.Where(x => x.Targets.Contains(module.Name)).Select(x => x.Name);
                        module.InitMemory(senders);
                    }
                }

                for (int i = 0; i < 1000; i++)
                {
                    Signal.LowPulses++;
                    List<Signal> signals = new List<Signal>();
                    foreach (var target in broadcast.Targets)
                    {
                        signals.Add(new Signal()
                        {
                            Origin = broadcast.Name,
                            Target = target,
                            High = false
                        });
                    }

                    while (signals.Count > 0)
                    {
                        List<Signal> newSignals = new List<Signal>();

                        foreach (var signal in signals)
                        {
                            if (signal.High)
                            {
                                Signal.HighPulses++;
                            }
                            else
                            {
                                Signal.LowPulses++;
                            }
                            if (modules.TryGetValue(signal.Target, out var module))
                            {
                                module.ProcessSignal(signal, newSignals);
                            }
                        }

                        signals = newSignals;
                    }
                }
            }

            return Signal.LowPulses * Signal.HighPulses;
        }

        public static long GetPlatinum()
        {
            using (var reader = new InputReader(20))
            {
                Dictionary<string, Module> modules = new Dictionary<string, Module>();

                Module broadcast = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(" -> ");
                    var targets = arr[1].Split(", ");

                    if (arr[0] == "broadcaster")
                    {
                        broadcast = new Module()
                        {
                            Name = "broadcaster",
                            FlipFlop = false,
                            Targets = targets,
                        };
                    }
                    else
                    {
                        var module = new Module()
                        {
                            Name = arr[0].Substring(1),
                            FlipFlop = arr[0][0] == '%',
                            Targets = targets,
                        };
                        modules.Add(module.Name, module);
                    }
                }

                foreach (var module in modules.Values)
                {
                    if (!module.FlipFlop)
                    {
                        var senders = modules.Values.Where(x => x.Targets.Contains(module.Name)).Select(x => x.Name);
                        module.InitMemory(senders);
                    }
                }

                int i = 0;
                bool rx = false;
                while (!rx)
                {
                    i++;
                    List<Signal> signals = new List<Signal>();
                    foreach (var target in broadcast.Targets)
                    {
                        signals.Add(new Signal()
                        {
                            Origin = broadcast.Name,
                            Target = target,
                            High = false
                        });
                    }

                    while (signals.Count > 0)
                    {
                        List<Signal> newSignals = new List<Signal>();

                        foreach (var signal in signals)
                        {

                            if (signal.Target == "rx" && !signal.High)
                            {
                                return i;
                            }

                            if (modules.TryGetValue(signal.Target, out var module))
                            {
                                module.ProcessSignal(signal, newSignals);
                            }
                        }

                        signals = newSignals;
                    }
                }
            }

            return 0;
        }


        public class Module
        {
            public string Name { get; init; }
            public bool FlipFlop { get; init; }
            public string[] Targets { get; init; }

            private bool _on;

            private Dictionary<string, bool> conjuctationMemory = null;

            public void InitMemory(IEnumerable<string> signals)
            {
                conjuctationMemory = new Dictionary<string, bool>();
                foreach (var signal in signals)
                {
                    conjuctationMemory[signal] = false;
                }
            }

            public void ProcessSignal(Signal signal, List<Signal> signals)
            {                
                if (FlipFlop)
                {
                    if (signal.High) return;
                    _on = !_on;
                    foreach (var target in Targets)
                    {
                        signals.Add(new Signal()
                        {
                            Origin = Name,
                            Target = target,
                            High = _on
                        });
                    }
                }
                else
                {
                    conjuctationMemory[signal.Origin] = signal.High;

                    foreach (var target in Targets)
                    {
                        signals.Add(new Signal()
                        {
                            Origin = Name,
                            Target = target,
                            High = !conjuctationMemory.All(x => x.Value)
                        });
                    }
                }
            }
            
        }

        public class Signal
        {
            public static long LowPulses = 0;
            public static long HighPulses = 0;

            public string Origin { get; set; }
            public string Target { get; set; }
            public bool High { get; set; }
        }
    }
}
