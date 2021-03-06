﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// 286
    /// 3638606400
    /// </summary>
    public class Day03 : IDay
    {
        private readonly List<string> _map;

        public Day03()
        {
            _map = Inputs.Day03
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToList();
        }

        private ulong Count(int right, int down)
        {
            var cx = 0;
            var ans = 0ul;

            for (int row = down; row < _map.Count; row += down)
            {
                cx = (cx + right) % _map[0].Length;
                if (_map[row][cx] == '#')
                {
                    ans++;
                }
            }

            return ans;
        }

        public object Part1() => Count(3, 1);

        public object Part2() => Count(1, 1) * Count(3, 1) * Count(5, 1) * Count(7, 1) * Count(1, 2);
    }
}