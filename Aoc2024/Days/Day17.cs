using System.Text.RegularExpressions;
using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day17 : IDay
{
    private const string TestInput1 =
        """
        Register A: 729
        Register B: 0
        Register C: 0

        Program: 0,1,5,4,3,0
        """;

    private const string TestInput2 =
        """
        Register A: 2024
        Register B: 0
        Register C: 0

        Program: 0,3,5,4,3,0
        """;

    [Example(TestInput1, "4,6,3,5,6,3,5,2,1,0")]
    public object Part1(IInput input)
    {
        var numPattern = new Regex(@"\d+");
        var nums = numPattern.Matches(input.Text).Select(m => long.Parse(m.Value)).ToArray();

        var program = nums[3..].Select(v => (byte)v).ToArray();
        var registers = nums[..3];
        var outputBuffer = new byte[64];
        Array.Fill(outputBuffer, (byte)8);

        Run(registers, program, outputBuffer);

        return string.Join(',', outputBuffer.Where(v => v < 8));
    }

    [Example(TestInput2, 117440L)]
    public object Part2(IInput input)
    {
        var numPattern = new Regex(@"\d+");
        var nums = numPattern.Matches(input.Text).Select(m => long.Parse(m.Value)).ToArray();

        var program = nums[3..].Select(v => (byte)v).ToArray();
        var registers = new long[3];
        var outputBuffer = new byte[program.Length];

        var result = 0L;
        for (var position = program.Length - 1; position >= 0; position--)
        {
            var incr = 1L << (position * 3);

            for (;;)
            {
                registers[0] = result;
                registers[1] = 0;
                registers[2] = 0;

                Array.Fill(outputBuffer, (byte)8);
                Run(registers, program, outputBuffer);

                if (outputBuffer[position..program.Length].SequenceEqual(program[position..]))
                {
                    break;
                }

                result += incr;
            }
        }

        return result;
    }

    private static void Run(long[] registers, byte[] program, byte[] outputBuffer)
    {
        var nextOutputIndex = 0;
        ushort pointer = 0;

        while (pointer < program.Length)
        {
            var op = program[pointer];
            var operand = program[pointer + 1];
            var combo = operand switch
            {
                < 4 => operand,
                < 7 => registers[operand - 4],
                _ => throw new ArgumentException()
            };

            var didJump = false;
            switch (op)
            {
                case 0:
                    // adv
                    registers[0] /= (1 << (int)combo);
                    break;
                case 1:
                    // bxl
                    registers[1] ^= operand;
                    break;
                case 2:
                    // bst
                    registers[1] = combo % 8;
                    break;
                case 3:
                    // jnz
                    if (registers[0] > 0)
                    {
                        pointer = operand;
                        didJump = true;
                    }

                    break;
                case 4:
                    // bxc
                    registers[1] ^= registers[2];
                    break;
                case 5:
                    // out
                    var output = (byte)(combo % 8);
                    outputBuffer[nextOutputIndex++] = output;
                    break;
                case 6:
                    // bdv
                    registers[1] = registers[0] / (1 << (int)combo);
                    break;
                case 7:
                    // cdv
                    registers[2] = registers[0] / (1 << (int)combo);
                    break;
            }

            if (!didJump)
            {
                pointer += 2;
            }
        }
    }
}