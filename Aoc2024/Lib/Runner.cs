using System.Diagnostics;
using System.Reflection;

namespace Aoc2024.Lib;

public static class Runner
{
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
                var result = Invoke(day, input, part);
                var expected = example.Result;
                if (result is long && expected is int)
                {
                    expected = (long)(int)expected;
                }
                
                if (Equals(result, expected))
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

            var realResult = Invoke(day, realInput, part);
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

    public static void Profile<TDay>(int part) where TDay : IDay
    {
        var name = typeof(TDay).Name;
        var dayIndex = int.Parse(name[3..]);
        var realInput = new FileInput(dayIndex);
        var day = Activator.CreateInstance<TDay>();

        // Warm up
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < 1000)
        {
            Invoke(day, realInput, part);
        }

        sw.Restart();

        var runs = 0;
        while (sw.ElapsedMilliseconds < 5000)
        {
            Invoke(day, realInput, part);
            runs++;
        }

        var avg = (double)sw.ElapsedMilliseconds / runs;
        Console.WriteLine($"Average run time = {avg} ms");
    }

    private static object Invoke(IDay day, IInput input, int part)
    {
        if (part == 1)
        {
            return day.Part1(input);
        }

        return day.Part2(input);
    }
}