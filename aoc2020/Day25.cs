using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 8740494
    /// Part 2: 8740494
    /// </summary>
    public class Day25 : IDay
    {
        private IEnumerable<long> Transform(long subject = 7)
        {
            var v = 1L;

            while (true)
            {
                v = (v * subject) % 20201227;

                yield return v;
            }
        }

        private int FindLoop(long key)
        {
            var i = 1;

            foreach (var k in Transform())
            {
                if (k == key)
                {
                    return i;
                }
                i++;
            }

            throw new InvalidProgramException();
        }

        const long KEY1 = 9789649;
        const long KEY2 = 3647239;

        public object Part1() => Transform(KEY1).Skip(FindLoop(KEY2) - 1).First();

        public object Part2() => Transform(KEY2).Skip(FindLoop(KEY1) - 1).First();
    }
}