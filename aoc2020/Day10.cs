using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 2450
    /// Part 2: 32396521357312
    /// </summary>
    public class Day10 : IDay
    {
        private readonly List<int> _jolts;
        private Dictionary<int, long> _cache;

        public Day10()
        {
            _jolts = Inputs.Day10.Split(Environment.NewLine)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .OrderBy(x => x)
                .ToList();
            _cache = new Dictionary<int, long>();
        }

        public object Part1()
        {
            var diff = new int[4];

            for (int i = 1; i < _jolts.Count; i++)
            {
                diff[_jolts[i] - _jolts[i - 1]]++;
            }
            diff[_jolts.First()]++;
            diff[3]++;

            return diff[1] * diff[3];
        }

        long Count(int index)
        {
            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }

            if (index == _jolts.Count - 1)
            {
                return 1;
            }

            var sum = 0L;
            for (int i = index + 1; i < _jolts.Count; i++)
            {
                if (_jolts[i] - _jolts[index] <= 3)
                {
                    sum += Count(i);
                }
            }

            return (_cache[index] = sum);
        }

        public object Part2()
        {
            _jolts.Insert(0, 0);
            _jolts.Add(_jolts.Max() + 3);

            return Count(0);
        }
    }
}