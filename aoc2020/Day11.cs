using System;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 2249
    /// Part 2: 2023
    /// </summary>
    public class Day11 : IDay
    {
        enum Cell { Floor, Empty, Taken };

        private readonly Cell[] _map, _copy, _init;
        private int _width, _height;

        public Day11()
        {
            var level = Inputs.Day11
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToCharArray())
                .ToList();

            _height = level.Count;
            _width = level.First().Length;

            _map = new Cell[_height * _width];
            _copy = new Cell[_map.Length];
            _init = new Cell[_map.Length];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _map[y * _width + x] = (level[y][x]) switch
                    {
                        'L' => Cell.Empty,
                        '.' => Cell.Floor,
                        '#' => Cell.Taken,
                        _ => throw new NotImplementedException()
                    };
                }
            }
            Array.Copy(_map, _init, _map.Length);
            Array.Copy(_map, _copy, _map.Length);
        }

        int Run(int takenRule, bool isPart2)
        {
            var moves = new[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };

            Array.Copy(_init, _map, _map.Length);

            while (true)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        var index = y * _width + x;

                        var occupied = 0;
                        foreach (var move in moves)
                        {
                            if (LookForSeat(x, y, move.Item1, move.Item2, !isPart2))
                            {
                                occupied++;
                            }
                        }

                        if (_map[index] == Cell.Empty && occupied == 0)
                        {
                            _copy[index] = Cell.Taken;
                        }
                        else if (_map[index] == Cell.Taken && occupied >= takenRule)
                        {
                            _copy[index] = Cell.Empty;
                        }
                        else
                        {
                            _copy[index] = _map[index];
                        }
                    }
                }

                var change = _map.SequenceEqual(_copy);
                Array.Copy(_copy, _map, _copy.Length);
                if (change)
                {
                    break;
                }
            }

            return _map.Count(x => x == Cell.Taken);
        }

        bool LookForSeat(int x, int y, int mx, int my, bool breakFirst)
        {
            while (true)
            {
                x += mx;
                y += my;

                if (x < 0 || x >= _width || y < 0 || y >= _height)
                {
                    return false;
                }

                var index = y * _width + x;

                if (_map[index] == Cell.Taken)
                {
                    return true;
                }
                else if (_map[index] == Cell.Empty || breakFirst)
                {
                    return false;
                }
            }
        }

        public object Part1() => Run(4, false);

        public object Part2() => Run(5, true);
    }
}