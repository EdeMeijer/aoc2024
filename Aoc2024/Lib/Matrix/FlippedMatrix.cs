namespace Aoc2024.Lib.Matrix;

public sealed class FlippedMatrix<T> : AbstractMatrix<T>
{
    private readonly IMatrix<T> _inner;

    public override int Height => _inner.Height;
    public override int Width => _inner.Width;

    public FlippedMatrix(IMatrix<T> inner)
    {
        _inner = inner;
    }

    public override T this[int y, int x]
    {
        get => _inner[y, _inner.Width - x - 1];
        set => _inner[y, _inner.Width - x - 1] = value;
    }
}