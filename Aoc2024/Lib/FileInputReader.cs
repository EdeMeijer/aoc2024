namespace Aoc2024.Lib;

internal static class FileInputReader
{
    internal static string Text(int day) => File.ReadAllText(FileFor(day));

    internal static string[] Lines(int day) => File.ReadAllLines(FileFor(day));

    private static string FileFor(int day)
    {
        var dir = AppDomain.CurrentDomain.BaseDirectory;
        return $"{dir}/../../../input/day{(day < 10 ? "0" : "")}{day}";
    }
}

internal class FileInput(int day) : IInput
{
    public string Text => FileInputReader.Text(day);
    public IReadOnlyCollection<string> Lines => FileInputReader.Lines(day);
}