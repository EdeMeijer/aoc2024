namespace Aoc2024.Lib;

public interface IInput
{
    string Text { get; }
    
    IReadOnlyCollection<string> Lines { get; }
}