namespace Aoc2024.Lib;

public readonly record struct Vec2D(int Y, int X)
{
    public static Vec2D operator +(Vec2D a, Vec2D b) => new(a.Y + b.Y, a.X + b.X);
    public static Vec2D operator -(Vec2D a, Vec2D b) => new(a.Y - b.Y, a.X - b.X);
    public static Vec2D operator -(Vec2D a) => new(-a.Y, -a.X);

    public Vec2D RotateCw() => new(X, -Y);
}