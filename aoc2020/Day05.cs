using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// 908
    /// 619
    /// </summary>
    public class Day05 : IDay
    {
        private readonly List<string> _data;
        private readonly int[] _seats;

        const int ROWS = 128;
        const int COLS = 8;

        public Day05()
        {
            _data = Inputs.Day05
                .Split(Environment.NewLine)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            _seats = new int[ROWS * COLS];
        }

        int Decode(string s)
        {
            var row = Binary(s[..7]);
            var col = Binary(s[7..]);
            var id = row * COLS + col;

            return _seats[id] = id;
        }

        int Binary(string s)
        {
            var tab = Enumerable.Range(0, (int) Math.Pow(2, s.Length)).ToList();

            foreach (var c in s)
            {
                if (c == 'F' || c == 'L')
                {
                    // lower half
                    tab = tab.Take(tab.Count / 2).ToList();
                }
                else
                {
                    // upper half
                    tab = tab.Skip(tab.Count / 2).ToList();
                }
            }

            return tab.Single();
        }

        public object Part1() => _data.Max(x => Decode(x));

        public object Part2()
        {
            for (int row = 1; row < ROWS - 1; row++)
            {
                for (int col = 1; col < COLS - 1; col++)
                {
                    var id = row * COLS + col;

                    if (_seats[id] == 0 && _seats[id - 1] != _seats[id + 1])
                    {
                        return id;
                    }
                }
            }

            throw new InvalidProgramException();
        }
    }
}