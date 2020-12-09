using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 133015568
    /// Part 2: 16107959
    /// </summary>
    public class Day09 : IDay
    {
        public const int PREAMBLESIZE = 25;
        public List<long> _preamble, _numbers, _all;
        private long _part1;

        public Day09()
        {
            _part1 = 0;
            _all = Inputs.Day09
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            _preamble = _all.Take(PREAMBLESIZE).ToList();
            _numbers = _all.Skip(PREAMBLESIZE).ToList();
        }

        bool HaveSum(long number)
        {
            for (int i = 0; i < _preamble.Count; i++)
            {
                for (int j = 0; j < _preamble.Count; j++)
                {
                    if (i != j && _preamble[i] + _preamble[j] == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public object Part1()
        {
            foreach (var n in _numbers)
            {
                if (!HaveSum(n))
                {
                    return (_part1 = n);
                }

                _preamble.Add(n);
                _preamble.RemoveAt(0);
            }

            throw new InvalidProgramException();
        }

        public object Part2()
        {
            var xmas = new List<long>();

            for (int i = 0; i < _all.Count; i++)
            {
                xmas.Clear();
                xmas.Add(_all[i]);

                for (int j = i + 1; j < _all.Count; j++)
                {
                    xmas.Add(_all[j]);
                    if (xmas.Sum() > _part1)
                    {
                        break;
                    }
                    else if (xmas.Sum() == _part1)
                    {
                        return xmas.Min() + xmas.Max();
                    }
                }
            }

            throw new InvalidProgramException();
        }
    }
}