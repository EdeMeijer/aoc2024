namespace Aoc2024.Lib.Matrix;

public sealed class MatrixSlice<T> : AbstractMatrix<T>
{
    private readonly IMatrix<T> _inner;
    private readonly int _y;
    private readonly int _x;

    public override int Height { get; }
    public override int Width { get; }

    public MatrixSlice(IMatrix<T> inner, int y, int height, int x, int width)
    {
        _inner = inner;
        _y = y;
        _x = x;
        Height = height;
        Width = width;
    }

    public override T this[int y, int x]
    {
        get => _inner[_y + y, _x + x];
        set => _inner[_y + y, _x + x] = value;
    }
}