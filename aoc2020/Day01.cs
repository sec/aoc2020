using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// 145875
    /// 69596112
    /// </summary>
    public class Day01 : IDay
    {
        private readonly List<int> _list;

        public Day01()
        {
            _list = Inputs.Day01.Split(Environment.NewLine.ToCharArray())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToList();
        }

        public object Part1()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = 0; j < _list.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var (a, b) = (_list[i], _list[j]);

                    if (a + b == 2020)
                    {
                        return a * b;
                    }
                }
            }
            throw new InvalidProgramException();
        }

        public object Part2()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = 0; j < _list.Count; j++)
                {
                    for (int k = 0; k < _list.Count; k++)
                    {
                        if (i == j || j == k)
                        {
                            continue;
                        }

                        var (a, b, c) = (_list[i], _list[j], _list[k]);

                        if (a + b + c == 2020)
                        {
                            return a * b * c;
                        }
                    }
                }
            }

            throw new InvalidProgramException();
        }
    }
}