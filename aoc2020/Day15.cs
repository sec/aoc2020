using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    public class Day15 : IDay
    {
        int Say(int howMuch)
        {
            var input = new Stack<int>(new[] { 6, 13, 1, 15, 2, 0 }.Reverse());
            var whenSpoken = new Dictionary<int, List<int>>();
            var turn = 1;
            var last = 0;

            while (howMuch-- > 0)
            {
                if (input.Count > 0)
                {
                    last = SayIt(input.Pop());
                }
                else
                {
                    if (whenSpoken.ContainsKey(last) && whenSpoken[last].Count > 1)
                    {
                        var diff = whenSpoken[last].TakeLast(2);
                        last = SayIt(diff.Last() - diff.First());
                    }
                    else
                    {
                        last = SayIt(0);
                    }
                }

                turn++;
            }
            return last;

            int SayIt(int n)
            {
                if (!whenSpoken.ContainsKey(n))
                {
                    whenSpoken[n] = new List<int>();
                }

                whenSpoken[n].Add(turn);

                return n;
            }
        }

        public object Part1() => Say(2020);

        public object Part2() => Say(30000000);
    }
}