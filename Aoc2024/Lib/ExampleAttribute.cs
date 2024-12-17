namespace Aoc2024.Lib;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExampleAttribute(string input, object result) : Attribute
{
    public string Input => input;

    public object Result => result;
};