using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 923
    /// Part 2: 24769
    /// </summary>
    public class Day12 : IDay
    {
        enum Dir { E, N, W, S };

        private readonly List<string> _nav;

        public Day12()
        {
            _nav = Inputs.Day12.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        void Process(Action<char, int> callback)
        {
            foreach (var line in _nav)
            {
                var i = line[0];
                var n = int.Parse(line[1..]);

                callback(i, n);
            }
        }

        int Sail1()
        {
            var east = 0;
            var north = 0;
            var dir = Dir.E;

            Process((i, n) =>
            {
                switch (i)
                {
                    case 'N':
                        north += n;
                        break;
                    case 'S':
                        north -= n;
                        break;
                    case 'W':
                        east -= n;
                        break;
                    case 'E':
                        east += n;
                        break;

                    case 'F':
                        switch (dir)
                        {
                            case Dir.N:
                                north += n;
                                break;
                            case Dir.S:
                                north -= n;
                                break;
                            case Dir.W:
                                east -= n;
                                break;
                            case Dir.E:
                                east += n;
                                break;
                        }
                        break;

                    case 'L':
                    case 'R':
                        dir = (Dir) (((int) dir + ((i == 'R' ? 360 - n : n) / 90)) % 4);
                        break;
                }
            });

            return Math.Abs(east) + Math.Abs(north);
        }

        int Sail2()
        {
            var weast = 10;
            var wnorth = 1;

            var east = 0;
            var north = 0;

            Process((i, n) =>
            {
                switch (i)
                {
                    case 'N':
                        wnorth += n;
                        break;
                    case 'S':
                        wnorth -= n;
                        break;
                    case 'W':
                        weast -= n;
                        break;
                    case 'E':
                        weast += n;
                        break;

                    case 'F':
                        north += n * wnorth;
                        east += n * weast;
                        break;

                    case 'L':
                    case 'R':
                        var nn = i == 'L' ? 360 - n : n;
                        for (int j = 0; j < nn / 90; j++)
                        {
                            (wnorth, weast) = (-weast, wnorth);
                        }
                        break;
                }
            });

            return Math.Abs(east) + Math.Abs(north);
        }

        public object Part1() => Sail1();

        public object Part2() => Sail2();
    }
}