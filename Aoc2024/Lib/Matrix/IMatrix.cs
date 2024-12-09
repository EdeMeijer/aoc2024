namespace Aoc2024.Lib.Matrix;

public interface IMatrix<T> : IEquatable<IMatrix<T>> where T : IEquatable<T>
{
    int Height { get; }

    int Width { get; }

    IEnumerable<T> Values { get; }

    T this[int y, int x] { get; set; }

    T this[Vec2D coord] { get; set; }

    IEnumerable<T> Row(int y);

    IEnumerable<T> Col(int x);

    IEnumerable<Vec2D> Coords { get; }

    IMatrix<T> Clone();

    IMatrix<T> With(int y, int x, T value);

    bool Contains(int y, int x);
    
    bool Contains(Vec2D coord);

    IMatrix<T> RotateCw();

    IMatrix<T> FlipHorizontal();

    IMatrix<T> Slice(int y, int height, int x, int width);
}