using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day24 : IDay
{
    private const string TestInput1 =
        """
        x00: 1
        x01: 1
        x02: 1
        y00: 0
        y01: 1
        y02: 0

        x00 AND y00 -> z00
        x01 XOR y01 -> z01
        x02 OR y02 -> z02
        """;

    private const string TestInput2 =
        """
        x00: 1
        x01: 0
        x02: 1
        x03: 1
        x04: 0
        y00: 1
        y01: 1
        y02: 1
        y03: 1
        y04: 1

        ntg XOR fgs -> mjb
        y02 OR x01 -> tnw
        kwq OR kpj -> z05
        x00 OR x03 -> fst
        tgd XOR rvg -> z01
        vdt OR tnw -> bfw
        bfw AND frj -> z10
        ffh OR nrd -> bqk
        y00 AND y03 -> djm
        y03 OR y00 -> psh
        bqk OR frj -> z08
        tnw OR fst -> frj
        gnj AND tgd -> z11
        bfw XOR mjb -> z00
        x03 OR x00 -> vdt
        gnj AND wpb -> z02
        x04 AND y00 -> kjc
        djm OR pbm -> qhw
        nrd AND vdt -> hwm
        kjc AND fst -> rvg
        y04 OR y02 -> fgs
        y01 AND x02 -> pbm
        ntg OR kjc -> kwq
        psh XOR fgs -> tgd
        qhw XOR tgd -> z09
        pbm OR djm -> kpj
        x03 XOR y03 -> ffh
        x00 XOR y04 -> ntg
        bfw OR bqk -> z06
        nrd XOR fgs -> wpb
        frj XOR qhw -> z04
        bqk OR frj -> z07
        y03 OR x01 -> nrd
        hwm AND bqk -> z03
        tgd XOR rvg -> z12
        tnw OR pbm -> gnj
        """;

    [Example(TestInput1, 4)]
    [Example(TestInput2, 2024)]
    public object Part1(IInput input)
    {
        var parts = input.Text.Split("\n\n");

        var states = parts[0].Split("\n").ToDictionary(line => line[..3], line => line[5] == '1');
        var nodes = parts[1].Split("\n").Select(line => line.Split(' ')).ToDictionary(p => p[4], p => p[..3]);

        bool GetState(string n)
        {
            if (states.TryGetValue(n, out var state))
            {
                return state;
            }

            var node = nodes[n];
            var a = GetState(node[0]);
            var b = GetState(node[2]);
            return node[1] switch
            {
                "AND" => a && b,
                "OR" => a || b,
                "XOR" => a != b,
            };
        }

        var result = 0UL;

        foreach (var n in nodes.Keys.Where(n => n.StartsWith("z")))
        {
            var b = GetState(n);

            if (b)
            {
                var i = int.Parse(n[1..]);
                result += 1UL << i;
            }
        }

        return result;
    }

    public object Part2(IInput input)
    {
        var parts = input.Text.Split("\n\n");
        var nodes = parts[1].Split("\n").Select(line => line.Split(' ')).ToDictionary(p => p[4], p => p[..3]);

        void DoSwap(string a, string b)
        {
            (nodes[a], nodes[b]) = (nodes[b], nodes[a]);
        }
        
        // TODO manually collect the best swaps
        DoSwap("mvb", "z08");
        DoSwap("jss", "rds");
        DoSwap("wss", "z18");
        DoSwap("bmn", "z23");
        
        // bmn,jss,mvb,rds,wss,z08,z18,z23

        var states = new Dictionary<string, bool?>();

        int Test(string swapA, string swapB)
        {
            for (var offset = 0; offset <= 44; offset++)
            {
                // Test 0.. + 1.. = 1..
                if (!PartialTest(0UL << offset, 1UL << offset, swapA, swapB))
                {
                    return offset;
                }
                
                // Test 1.. + 1.. = 10..
                if (!PartialTest(1UL << offset, 1UL << offset, swapA, swapB))
                {
                    return offset;
                }
                    
                // Test 11.. + 01.. = 100..
                if (!PartialTest(11UL << offset, 1UL << offset, swapA, swapB))
                {
                    return offset;
                }
                
                // Test 11.. + 11.. = 110..
                if (!PartialTest(11UL << offset, 11UL << offset, swapA, swapB))
                {
                    return offset;
                }
            }

            return -1;
        }
        
        bool PartialTest(ulong x, ulong y, string swapA, string swapB)
        {
            x &= 0b11111111111111111111111111111111111111111111;
            y &= 0b11111111111111111111111111111111111111111111;
            
            foreach (var key in nodes.Keys)
            {
                states[key] = null;
            }
            
            var mask = 1UL;
            for (var b = 0; b <= 44; b++)
            {
                states[GetNode('x', b)] = (x & mask) > 0;
                states[GetNode('y', b)] = (y & mask) > 0;
                mask <<= 1;
            }

            var z = x + y;
            
            mask = 1UL;
            for (var b = 0; b <= 45; b++)
            {
                try
                {
                    var s = GetState(GetNode('z', b), swapA, swapB, 0);
                    var expected = (z & mask) > 0;
                    if (s != expected)
                    {
                        return false;
                    }
                }
                catch (ApplicationException ex)
                {
                    return false;
                }

                mask <<= 1;
            }

            return true;
        }

        var firstErrorBitNo = Test( "", "");

        if (firstErrorBitNo == -1)
        {
            return 0;
        }
        
        // Try to find a pair of nodes to increase the first bit where things error out
        var nodeKeys = nodes.Keys.OrderBy(k => k).ToList();
        for (var a = 0; a < nodeKeys.Count; a++)
        {
            var nodeA = nodeKeys[a];
            for (var b = a + 1; b < nodeKeys.Count; b++)
            {
                var nodeB = nodeKeys[b];
                try
                {
                    var r = Test(nodeA, nodeB);
                    if (r == -1 || r > firstErrorBitNo)
                    {
                        Console.WriteLine($"Swap {nodeA} / {nodeB} -> {r}");
                    }
                }
                catch (ApplicationException)
                {
                    // Overflow
                }
            }
        }

        return 0;
        
        bool GetState(string n, string swapA, string swapB, int depth = 0)
        {
            if (depth > 1000)
            {
                throw new ApplicationException("Overflow");
            }
            if (n == swapA)
            {
                n = swapB;
            }
            else if (n == swapB)
            {
                n = swapA;
            }

            var s = states[n];
            if (s != null)
            {
                return s.Value;
            }

            var node = nodes[n];
            var a = GetState(node[0], swapA, swapB, depth + 1);
            var b = GetState(node[2], swapA, swapB, depth + 1);
            s = node[1] switch
            {
                "AND" => a && b,
                "OR" => a || b,
                "XOR" => a != b,
            };
            states[n] = s;
            return s.Value;
        }

        static string GetNode(char prefix, int n) => $"{prefix}{n.ToString().PadLeft(2, '0')}";
    }
}