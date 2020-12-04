using System;

namespace aoc2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Day04();
        }

        static void Day04()
        {
            var d = new Day04();

            Console.WriteLine(d.Part1()); // 210
            Console.WriteLine(d.Part2()); // 131
        }

        static void Day03()
        {
            var d = new Day03();

            Console.WriteLine(d.Part1()); // 286
            Console.WriteLine(d.Part2()); // 3638606400
        }

        static void Day02()
        {
            var d = new Day02();

            Console.WriteLine(d.Part1()); // 474
            Console.WriteLine(d.Part2()); // 745
        }

        static void Day01()
        {
            var d = new Day01();

            Console.WriteLine(d.Part1()); // 145875
            Console.WriteLine(d.Part2()); // 69596112
        }
    }
}