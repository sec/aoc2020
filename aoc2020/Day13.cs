using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 1915
    /// Part 2: 294354277694107
    /// </summary>
    public class Day13 : IDay
    {
        private readonly List<int> _times;
        private readonly int _start;

        public Day13()
        {
            var input = Inputs.Day13.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            _start = int.Parse(input[0]);
            _times = input[1].Split(',').Select(x => x.Replace("x", "0")).Select(int.Parse).ToList();
        }

        public object Part1()
        {
            for (var i = _start; true; i++)
            {
                var id = _times.Where(x => x > 0).SingleOrDefault(x => i % x == 0);
                if (id != 0)
                {
                    return id * (i - _start);
                }
            }
            throw new NotImplementedException();
        }

        public object Part2()
        {
            var times = _times.Where(x => x > 0).ToList();
            var adds = _times
                .Select((v, i) => (v, i))
                .Where(x => x.v > 0)
                .Select(x => x.i)
                .ToList();

            var start = 0L;
            var iter = 1L;

            for (int c = 2; c <= times.Count; c++)
            {
                (start, iter) = Find(times, adds, start, c, iter);
            }

            return start;
        }

        private (long nextMatch, long nextIter) Find(List<int> times, List<int> adds, long start, int number, long iter)
        {
            long i = start;

            while (true)
            {
                var found = true;
                for (int j = 0; j < number; j++)
                {
                    if ((i + adds[j]) % times[j] != 0)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return (i, times.Take(number).Aggregate(1L, (x, y) => x * y));
                }
                i += iter;
            }
        }
    }
}