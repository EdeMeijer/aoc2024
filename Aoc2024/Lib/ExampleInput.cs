namespace Aoc2024.Lib;

public sealed class ExampleInput(string text) : IInput
{
    public string Text => text;
    public IReadOnlyCollection<string> Lines => text.Split(Environment.NewLine);
}