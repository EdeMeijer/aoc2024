using System.Reflection;

namespace Aoc2024.Lib;

public static class Runner
{
    static Runner()
    {
        var part1Fn = typeof(IDay).GetMethod(nameof(IDay.Part1));
    }

    public static void Run<TDay>() where TDay : IDay
    {
        Run<TDay>(1);
        Console.WriteLine();
        Run<TDay>(2);
    }

    public static void Run<TDay>(int part) where TDay : IDay
    {
        var name = typeof(TDay).Name;
        var dayIndex = int.Parse(name[3..]);
        var realInput = new FileInput(dayIndex);
        var day = Activator.CreateInstance<TDay>();

        var methodName = $"Part{part}";
        var method = typeof(TDay).GetMethod(methodName)!;

        Console.WriteLine($"Running day {dayIndex} part {part}");

        try
        {
            // Run examples first
            var examplesPassed = true;
            foreach (var (example, i) in method.GetCustomAttributes<ExampleAttribute>().Select((attr, i) => (attr, i)))
            {
                var input = new ExampleInput(example.Input);
                var result = (long)method.Invoke(day, [input])!;
                if (result == example.Result)
                {
                    Console.WriteLine($"Example {i + 1} passed");
                }
                else
                {
                    Console.WriteLine($"Example {i + 1} failed! Expected {example.Result}, got {result}");
                    examplesPassed = false;
                }
            }

            if (!examplesPassed)
            {
                return;
            }

            var realResult = (long)method.Invoke(day, [realInput])!;
            Console.WriteLine($"Result = {realResult}");
        }
        catch (TargetInvocationException ex)
        {
            if (ex.InnerException is NotImplementedException)
            {
                Console.WriteLine("Not implemented yet");
            }
            else
            {
                throw;
            }
        }
    }
}