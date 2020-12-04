using System;
using System.Linq;
using System.Reflection;

namespace aoc2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = DateTime.Now.Day;
            if (args.Length == 1)
            {
                day = int.Parse(args[0]);
            }

            var t = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Name == $"Day{day:d2}")
                .Single();

            var c = (IDay) Activator.CreateInstance(t);

            Console.WriteLine($"Running Day{day:d2}...");

            Console.WriteLine($"Part 1: {c.Part1()}");
            Console.WriteLine($"Part 2: {c.Part2()}");
        }

        static void Day01()
        {
            var d = new Day01();

            Console.WriteLine(d.Part1()); // 
            Console.WriteLine(d.Part2()); // 
        }
    }
}