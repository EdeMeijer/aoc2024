namespace Aoc2024.Lib.Matrix;

public sealed class Matrix<T> : AbstractMatrix<T> where T : IEquatable<T>
{
    private readonly List<T> _values;

    public override int Height { get; }
    public override int Width { get; }
    public override IEnumerable<T> Values => _values;

    public Matrix(int height, int width, T defaultValue) :
        this(height, width, Enumerable.Repeat(defaultValue, height * width))
    {
    }

    public Matrix(int height, int width, IEnumerable<T> values)
    {
        Height = height;
        Width = width;
        _values = values.ToList();
        if (_values.Count != height * width)
        {
            throw new ArgumentException("Invalid number of values");
        }
    }

    public override T this[int y, int x]
    {
        get => _values[IndexOf(y, x)];
        set => _values[IndexOf(y, x)] = value;
    }

    private int IndexOf(int y, int x)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
        {
            throw new ArgumentException($"Invalid coordinate ({y},{x}) (height={Height},width={Width})");
        }

        return y * Width + x;
    }
}

public static class Matrix
{
    public static IMatrix<char> Parse(string input) => Parse(input, c => c);

    public static IMatrix<T> Parse<T>(string input, Func<char, T> parseElem) where T : IEquatable<T>
    {
        var lines = input.Split('\n');
        var height = lines.Length;
        var width = lines[0].Length;
        var values = lines.SelectMany(line => line.Select(parseElem));
        return new Matrix<T>(height, width, values);
    }
}