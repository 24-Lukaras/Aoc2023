using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task14
    {

        private static int s = 0;

        public static long GetNormal()
        {
            long result = 0;

            using (var reader = new InputReader(14))
            {
                List<Block>[] blocks = null;
                List<Rock>[] rocks = null;

                int lineNumber = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    if (blocks == null)
                    {
                        blocks = new List<Block>[line.Length];
                        rocks = new List<Rock>[line.Length];

                        for (int i = 0; i < line.Length; i++)
                        {
                            blocks[i] = new List<Block>();
                            rocks[i] = new List<Rock>();
                        }
                    }

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'O')
                        {
                            rocks[i].Add(new Rock()
                            {
                                Column = i,
                                Line = lineNumber
                            });
                        }
                        else if (line[i] == '#')
                        {
                            blocks[i].Add(new Block()
                            {
                                Column = i,
                                Line = lineNumber
                            });
                        }
                    }
                    lineNumber++;
                }


                for (int i = 0; i < rocks.Length; i++)
                {
                    Block currentBlock = new Block()
                    {
                        Column = i,
                        Line = -1
                    };
                    Block nextBlock = blocks[i].FirstOrDefault();
                    int nextBlockIndex = 0;

                    foreach (var rock in rocks[i])
                    {
                        if (nextBlock == null || rock.Line < nextBlock.Line)
                        {
                            currentBlock = new Block()
                            {
                                Column = i,
                                Line = currentBlock.Line + 1
                            };
                            int val = lineNumber - currentBlock.Line;
                            result += val;
                        }
                        else if (rock.Line > nextBlock.Line)
                        {
                            while (nextBlock != null && rock.Line > nextBlock.Line)
                            {
                                currentBlock = nextBlock;
                                nextBlockIndex++;
                                nextBlock = nextBlockIndex >= blocks[i].Count ? null : blocks[i][nextBlockIndex];
                            }
                            currentBlock = new Block()
                            {
                                Column = i,
                                Line = currentBlock.Line + 1
                            };
                            int val = lineNumber - currentBlock.Line;
                            result += val;                            
                        }
                    }

                }

            }

            return result;
        }

        public static long GetPlatinum()
        {
            long result = 0;

            using (var reader = new InputReader(14))
            {
                List<Rock> rocks = new List<Rock>();
                List<List<Block>> blocksC = new List<List<Block>>();
                List<List<Block>> blocksV = new List<List<Block>>();

                List<Block> currentBlocksV = null;

                int lineNumber = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    if (!blocksC.Any())
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            blocksC.Add(new List<Block>());
                        }
                    }

                    currentBlocksV = new List<Block>();

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'O')
                        {
                            rocks.Add(new Rock()
                            {
                                Column = i,
                                Line = lineNumber
                            });
                        }
                        else if (line[i] == '#')
                        {
                            var block = new Block()
                            {
                                Column = i,
                                Line = lineNumber
                            };
                            blocksC[i].Add(block);
                            currentBlocksV.Add(block);
                        }
                    }

                    blocksV.Add(currentBlocksV);

                    lineNumber++;
                }                

                Visualize(rocks, blocksV);

                //var tempRocks = rocks.Select(x => new Rock() { Column = x.Column, Line = x.Line }).ToList();
                var tempRocks = rocks.ToList();
                var upBlocks = blocksC.ToList();
                var leftBlocks = blocksV.ToList();
                var downBlocks = blocksC.ToList();
                var rightBlocks = blocksV.ToList();
                int changes = -1;

                List<Rock>[] orderedRocks;

                for (int i = 0; i < 100; i++)
                {
                    orderedRocks = tempRocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, upBlocks, false, false, -1);
                    orderedRocks = tempRocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, leftBlocks, true, false, -1);
                    orderedRocks = tempRocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, downBlocks, false, true, lineNumber);
                    orderedRocks = tempRocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, rightBlocks, true, true, blocksC.Count);
                }

                
                while (tempRocks.Count > 0 && changes != 0)
                {
                    s++;

                    if (s == 25)
                    {
                        ;
                    }

                    orderedRocks = tempRocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, upBlocks, false, false, -1);
                    orderedRocks = tempRocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, leftBlocks, true, false, -1);
                    orderedRocks = tempRocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, downBlocks, false, true, lineNumber);
                    orderedRocks = tempRocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, rightBlocks, true, true, blocksC.Count);

                    changes = 0;
                    var newUpBlocks = new List<Block>();
                    var newLeftBlocks = new List<Block>();
                    var newDownBlocks = new List<Block>();
                    var newRightBlocks = new List<Block>();
                    

                    for (int i = tempRocks.Count - 1; i >= 0; i--)
                    {
                        var rock = tempRocks[i];
                        var tempRock = new Rock()
                        {
                            Line = rock.Line,
                            Column = rock.Column
                        };

                        (int, int) up = (upBlocks[tempRock.Column].Where(x => x.Line < tempRock.Line).LastOrDefault()?.Line ?? 0, rock.Column);
                        tempRock.Line = up.Item1 + 1;
                        (int, int) left = (tempRock.Line, leftBlocks[tempRock.Line].Where(x => x.Column < tempRock.Column).LastOrDefault()?.Column ?? 0);
                        tempRock.Column = left.Item2 + 1;
                        (int, int) down = (downBlocks[tempRock.Column].Where(x => x.Line > tempRock.Line).FirstOrDefault()?.Line ?? 99, rock.Column);
                        tempRock.Line = down.Item1 - 1;
                        (int, int) right = (tempRock.Line, rightBlocks[tempRock.Line].Where(x => x.Column > tempRock.Column).FirstOrDefault()?.Column ?? 99);
                        tempRock.Column = right.Item2 - 1;

                        if (tempRock.Line == rock.Line && tempRock.Column == rock.Column)
                        {
                            changes++;
                            newUpBlocks.Add(new Block()
                            {
                                Line = up.Item1,
                                Column = up.Item2
                            });
                            newLeftBlocks.Add(new Block()
                            {
                                Line = left.Item1,
                                Column = left.Item2
                            });
                            newDownBlocks.Add(new Block()
                            {
                                Line = down.Item1,
                                Column = down.Item2
                            });
                            newRightBlocks.Add(new Block()
                            {
                                Line = right.Item1,
                                Column = right.Item2
                            });
                            tempRocks.Remove(rock);
                        }
                    }

                    foreach (var tempUp in newUpBlocks)
                    {
                        for (int i = 0; i <= upBlocks[tempUp.Column].Count; i++)
                        {
                            if (i == upBlocks[tempUp.Column].Count - 1 || upBlocks[tempUp.Column][i + 1].Column > tempUp.Column)
                            {
                                upBlocks[tempUp.Column].Insert(i, tempUp);
                                break;
                            }
                        }
                    }
                    foreach (var tempLeft in newLeftBlocks)
                    {
                        for (int i = 0; i <= leftBlocks[tempLeft.Line].Count; i++)
                        {
                            if (i == leftBlocks[tempLeft.Line].Count - 1 || leftBlocks[tempLeft.Line][i + 1].Column > tempLeft.Column)
                            {
                                leftBlocks[tempLeft.Line].Insert(i, tempLeft);
                                break;
                            }
                        }
                    }
                    foreach (var tempDown in newDownBlocks)
                    {
                        for (int i = 0; i <= downBlocks[tempDown.Column].Count; i++)
                        {
                            if (i == downBlocks[tempDown.Column].Count - 1 || downBlocks[tempDown.Column][i + 1].Column > tempDown.Column)
                            {
                                downBlocks[tempDown.Column].Insert(i, tempDown);
                                break;
                            }
                        }
                    }
                    foreach (var tempRight in newRightBlocks)
                    {
                        for (int i = 0; i <= rightBlocks[tempRight.Line].Count; i++)
                        {
                            if (i == rightBlocks[tempRight.Line].Count - 1 || rightBlocks[tempRight.Line][i + 1].Column > tempRight.Column)
                            {
                                rightBlocks[tempRight.Line].Insert(i, tempRight);
                                break;
                            }
                        }
                    }
                }

                foreach (var rock in rocks.OrderBy(x => x.Line))
                {
                    int val = lineNumber - rock.Line;
                    result += val;
                }

                /*
                Dictionary<(int, int)[], int> map = new Dictionary<(int, int)[], int>();
                map.Add(RocksToKey(rocks), 0);
                bool found = false;

                Visualize(rocks, blocksV);

                int j = 1;
                while (!found)
                {
                    var orderedRocks = rocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, blocksC, false, false, -1);

                    //Visualize(rocks, blocksV);                    

                    orderedRocks = rocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderBy(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, blocksV, true, false, -1);

                    //Visualize(rocks, blocksV);

                    orderedRocks = rocks.GroupBy(x => x.Column).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Line).ToList()).ToArray();
                    Move(orderedRocks, blocksC, false, true, lineNumber);

                    //Visualize(rocks, blocksV);

                    orderedRocks = rocks.GroupBy(x => x.Line).OrderBy(x => x.Key).Select(x => x.OrderByDescending(y => y.Column).ToList()).ToArray();
                    Move(orderedRocks, blocksV, true, true, blocksC.Count);

                    Visualize(rocks, blocksV);

                    var key = RocksToKey(rocks);

                    if (map.TryGetValue(key, out int first))
                    {
                        found = true;
                    }

                    map.Add(key, j);
                    j++;
                }
                */

            }

            return result;
        }

        private static void Move(List<Rock>[] rocks, List<List<Block>> blocks, bool useColumn, bool fromEnd, int startValue)
        {
            int direction = fromEnd ? -1 : 1;
            for (int i = (fromEnd ? rocks.Length - 1 : 0); (fromEnd ? i >= 0 : i < rocks.Length); i += direction)
            {
                Block currentBlock = new Block();
                if (useColumn)
                {
                    currentBlock.Column = startValue;
                    currentBlock.Line = i;
                }
                else
                {
                    currentBlock.Column = i;
                    currentBlock.Line = startValue;
                }
                Block nextBlock = fromEnd ? blocks[i].LastOrDefault() : blocks[i].FirstOrDefault();
                int nextBlockIndex = fromEnd ? blocks[i].Count - 1 : 0;

                foreach (var rock in rocks[i])
                {
                    if (useColumn)
                    {
                        if (nextBlock == null || (fromEnd ? rock.Column > nextBlock.Column : rock.Column < nextBlock.Column))
                        {
                            currentBlock = new Block()
                            {
                                Column = currentBlock.Column + direction,
                                Line = i
                            };
                            rock.Column = currentBlock.Column;
                            rock.Line = currentBlock.Line;
                        }
                        else if ((fromEnd ? rock.Column < nextBlock.Column : rock.Column > nextBlock.Column))
                        {
                            while (nextBlock != null && (fromEnd ? rock.Column < nextBlock.Column : rock.Column > nextBlock.Column))
                            {
                                currentBlock = nextBlock;
                                nextBlockIndex += direction;
                                nextBlock = (fromEnd ? nextBlockIndex < 0 : nextBlockIndex >= blocks[i].Count) ? null : blocks[i][nextBlockIndex];
                            }
                            currentBlock = new Block()
                            {
                                Column = currentBlock.Column + direction,
                                Line = i
                            };
                            rock.Column = currentBlock.Column;
                            rock.Line = currentBlock.Line;
                        }
                    }
                    else
                    {
                        if (nextBlock == null || (fromEnd ? rock.Line > nextBlock.Line : rock.Line < nextBlock.Line))
                        {
                            currentBlock = new Block()
                            {
                                Column = i,
                                Line = currentBlock.Line + direction
                            };
                            rock.Column = currentBlock.Column;
                            rock.Line = currentBlock.Line;
                        }
                        else if ((fromEnd ? rock.Line < nextBlock.Line : rock.Line > nextBlock.Line))
                        {
                            while (nextBlock != null && (fromEnd ? rock.Line < nextBlock.Line : rock.Line > nextBlock.Line))
                            {
                                currentBlock = nextBlock;
                                nextBlockIndex += direction;
                                nextBlock = (fromEnd ? nextBlockIndex < 0 : nextBlockIndex >= blocks[i].Count) ? null : blocks[i][nextBlockIndex];
                            }
                            currentBlock = new Block()
                            {
                                Column = i,
                                Line = currentBlock.Line + direction
                            };
                            rock.Column = currentBlock.Column;
                            rock.Line = currentBlock.Line;
                        }
                    }                      
                }

            }
        }

        private static void Visualize(List<Rock> rocks, List<List<Block>> blocksV)
        {
            var r = RocksToKey(rocks);
            int ri = 0;

            for (int y = 0; y < 100; y++)
            {
                int bi = 0;
                for (int x = 0; x < 100; x++)
                {
                    if (ri < r.Length && r[ri] == (y, x))
                    {
                        Console.Write('O');
                        ri++;
                    }
                    else if (bi < blocksV[y].Count && blocksV[y][bi].Column == x)
                    {
                        Console.Write("#");
                        bi++;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
        }

        private static (int, int)[] RocksToKey(List<Rock> rocks)
        {
            return rocks.OrderBy(x => x.Line).ThenBy(x => x.Column).Select(x => (x.Line, x.Column)).ToArray();
        }

    }

    public class Rock
    {
        public int Column { get; set; }
        public int Line { get; set; }
    }

    public class Block
    {
        public int Column { get; set; }
        public int Line { get; set; }
    }
}
