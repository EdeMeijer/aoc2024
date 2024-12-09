namespace Aoc2024.Lib.Matrix;

public sealed class RotatedMatrix<T> : AbstractMatrix<T> where T : IEquatable<T>
{
    private readonly IMatrix<T> _inner;

    public override int Height => _inner.Width;
    public override int Width => _inner.Height;

    public RotatedMatrix(IMatrix<T> inner)
    {
        _inner = inner;
    }

    public override T this[int y, int x]
    {
        get => _inner[_inner.Height - x - 1, y];
        set => _inner[_inner.Height - x - 1, y] = value;
    }
}