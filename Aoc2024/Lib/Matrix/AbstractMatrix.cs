using System.Text;

namespace Aoc2024.Lib.Matrix;

public abstract class AbstractMatrix<T> : IMatrix<T> where T : IEquatable<T>
{
    public abstract int Height { get; }
    public abstract int Width { get; }

    public virtual IEnumerable<T> Values
    {
        get
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return this[y, x];
                }
            }
        }
    }

    public abstract T this[int y, int x] { get; set; }

    public T this[Vec2D coord]
    {
        get => this[coord.Y, coord.X];
        set => this[coord.Y, coord.X] = value;
    }

    public IEnumerable<T> Row(int y)
    {
        for (var x = 0; x < Width; x++)
        {
            yield return this[y, x];
        }
    }

    public IEnumerable<T> Col(int x)
    {
        for (var y = 0; y < Height; y++)
        {
            yield return this[y, x];
        }
    }

    public IEnumerable<Vec2D> Coords
    {
        get
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return new Vec2D(y, x);
                }
            }
        }
    }

    public IMatrix<T> Clone() => new Matrix<T>(Height, Width, Values.ToList());

    public IMatrix<T> With(int y, int x, T value)
    {
        var copy = Clone();
        copy[y, x] = value;
        return copy;
    }

    public bool Contains(int y, int x) => y >= 0 && x >= 0 && y < Height && x < Width;
    
    public bool Contains(Vec2D coord) => Contains(coord.Y, coord.X);

    public IMatrix<T> RotateCw() => new RotatedMatrix<T>(this);

    public IMatrix<T> FlipHorizontal() => new FlippedMatrix<T>(this);

    public IMatrix<T> Slice(int y, int height, int x, int width) => new MatrixSlice<T>(this, y, height, x, width);

    public override string ToString()
    {
        var result = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                result.Append(this[y, x]);
            }

            result.Append('\n');
        }

        return result.ToString();
    }

    public bool Equals(IMatrix<T>? other)
    {
        return other != null &&
               Height == other.Height &&
               Width == other.Width &&
               ValuesEqual(other);
    }

    private bool ValuesEqual(IMatrix<T> other)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (!Equals(this[y, x], other[y, x]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is IMatrix<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Height, Width, Values);
    }
}