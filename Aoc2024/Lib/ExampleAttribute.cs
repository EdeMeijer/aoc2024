namespace Aoc2024.Lib;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ExampleAttribute(string input, int result) : Attribute
{
    public string Input => input;

    public int Result => result;
};