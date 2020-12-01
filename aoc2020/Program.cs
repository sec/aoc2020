using System;

namespace aoc2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Day02();
        }


        static void Day02()
        {
            var d = new Day02();

            Console.WriteLine(d.Part1());
            Console.WriteLine(d.Part2());
        }

        static void Day01()
        {
            var d = new Day01();

            Console.WriteLine(d.Part1()); // 145875
            Console.WriteLine(d.Part2()); // 69596112
        }
    }
}