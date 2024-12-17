using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day15 : IDay
{
    private const string TestInput1 =
        """
        ########
        #..O.O.#
        ##@.O..#
        #...O..#
        #.#.O..#
        #...O..#
        #......#
        ########
        
        <^^>>>vv<v>>v<<
        """;
    
    private const string TestInput2 =
        """
        ##########
        #..O..O.O#
        #......O.#
        #.OO..O.O#
        #..O@..O.#
        #O#..O...#
        #O..O..O.#
        #.OO.O.OO#
        #....O...#
        ##########
        
        <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
        vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
        ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
        <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
        ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
        ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
        >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
        <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
        ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
        v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
        """;

    [Example(TestInput1, 2028)]
    [Example(TestInput2, 10092)]
    public object Part1(IInput input)
    {
        var parts = input.Text.Split("\n\n");
        var map = Matrix.Parse(parts[0]);
        var robot = map.Coords.First(c => map[c] == '@');
        
        var moves = parts[1].ReplaceLineEndings("")
            .Select(c => c switch
            {
                '>' => new Vec2D(0, 1),
                'v' => new Vec2D(1, 0),
                '<' => new Vec2D(0, -1),
                '^' => new Vec2D(-1, 0),
                _ => throw new ArgumentException()
            })
            .ToList();

        bool AttemptMove(Vec2D pos, Vec2D dir)
        {
            var targetPos = pos + dir;
            var targetChar = map[targetPos];
            if (targetChar == '#')
            {
                // wall
                return false;
            }

            if (targetChar == 'O')
            {
                // Attempt to move the box
                AttemptMove(targetPos, dir);
            }
            
            if (targetChar == '.')
            {
                // empty space
                map[targetPos] = map[pos];
                map[pos] = '.';
                return true;
            }

            return false;
        }
        
        foreach (var move in moves)
        {
            if (AttemptMove(robot, move))
            {
                // The robot did move
                robot += move;
            }
        }

        return map.Coords.Where(c => map[c] == 'O')
            .Sum(c => c.Y * 100 + c.X);
    }

    [Example(TestInput2, 9021)]
    public object Part2(IInput input)
    {
        var parts = input.Text.Split("\n\n");
        var map = Matrix.Parse(parts[0]);

        map = ResizeMap(map);
        
        var robot = map.Coords.First(c => map[c] == '@');
        
        var moves = parts[1].ReplaceLineEndings("")
            .Select(c => c switch
            {
                '>' => new Vec2D(0, 1),
                'v' => new Vec2D(1, 0),
                '<' => new Vec2D(0, -1),
                '^' => new Vec2D(-1, 0),
                _ => throw new ArgumentException()
            })
            .ToList();

        bool CanMove(Vec2D pos, Vec2D dir)
        {
            var targetPos = pos + dir;
            var targetChar = map[targetPos];
            if (targetChar == '#')
            {
                return false;
            }
                
            if (targetChar == '.')
            {
                return true;
            }
            
            if (dir.Y == 0)
            {
                // Horizontal box moves remain simple like before
                if (targetChar is '[' or ']')
                {
                    return CanMove(targetPos, dir);
                }
            }
            else
            {
                // Vertical box moves are a bit more complex
                if (targetChar == '[')
                {
                    return CanMove(targetPos, dir) && CanMove(targetPos + new Vec2D(0, 1), dir);
                }

                if (targetChar == ']')
                {
                    return CanMove(targetPos, dir) && CanMove(targetPos + new Vec2D(0, -1), dir);
                }
            }

            throw new ArgumentException();
        }

        void ApplyMove(Vec2D pos, Vec2D dir)
        {
            var targetPos = pos + dir;
            var targetChar = map[targetPos];
            
            if (dir.Y == 0)
            {
                // Horizontal moves remain simple like before
                if (targetChar is '[' or ']')
                {
                    ApplyMove(targetPos, dir);
                }
            }
            else
            {
                // Vertical box moves are a bit more complex
                if (targetChar == '[')
                {
                    ApplyMove(targetPos, dir);
                    ApplyMove(targetPos + new Vec2D(0, 1), dir);
                }

                if (targetChar == ']')
                {
                    ApplyMove(targetPos, dir);
                    ApplyMove(targetPos + new Vec2D(0, -1), dir);
                }
            }
            
            targetChar = map[targetPos];
            if (targetChar == '.')
            {
                // empty space
                map[targetPos] = map[pos];
                map[pos] = '.';
                return;
            }
            
            throw new ArgumentException();
        }

        bool AttemptMove(Vec2D pos, Vec2D dir)
        {
            if (CanMove(pos, dir))
            {
                ApplyMove(pos, dir);
                return true;
            }

            return false;
        }
        
        foreach (var move in moves)
        {
            if (AttemptMove(robot, move))
            {
                // The robot did move
                robot += move;
            }
        }

        return map.Coords.Where(c => map[c] == '[')
            .Sum(c => c.Y * 100 + c.X);
    }

    private static IMatrix<char> ResizeMap(IMatrix<char> map)
    {
        var newMap = new Matrix<char>(map.Height, map.Width * 2, '.');
        foreach (var coord in map.Coords)
        {
            var c = map[coord];
            if (c == '@')
            {
                newMap[coord.Y, coord.X * 2] = '@';
            }

            if (c == '#')
            {
                newMap[coord.Y, coord.X * 2] = '#';
                newMap[coord.Y, coord.X * 2 + 1] = '#';
            }

            if (c == 'O')
            {
                newMap[coord.Y, coord.X * 2] = '[';
                newMap[coord.Y, coord.X * 2 + 1] = ']';
            }
        }

        return newMap;
    }
}