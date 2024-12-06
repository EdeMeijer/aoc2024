namespace Aoc2024.Lib;

internal static class FileInputReader
{
    internal static string Text(int day) => File.ReadAllText(FileFor(day)).TrimEnd();

    private static string FileFor(int day)
    {
        var dir = AppDomain.CurrentDomain.BaseDirectory;
        return $"{dir}/../../../input/day{(day < 10 ? "0" : "")}{day}";
    }
}

internal class FileInput(int day) : IInput
{
    public string Text => FileInputReader.Text(day);
    public IList<string> Lines => Text.Split(Environment.NewLine);
}