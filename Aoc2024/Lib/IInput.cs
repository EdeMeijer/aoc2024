namespace Aoc2024.Lib;

public interface IInput
{
    string Text { get; }
    
    IList<string> Lines { get; }
}