namespace Aoc2024.Lib;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ExampleAttribute(string input, int result) : Attribute
{
    public string Input => input;

    public int Result => result;
};