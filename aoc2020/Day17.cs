using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 263
    /// Part 2: 1680
    /// </summary>
    public class Day17 : IDay
    {
        public Day17()
        {

        }

        void Fill(Action<int, int> callback)
        {
            var data = Inputs.Day17.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < data.Length; y++)
            {
                for (int x = 0; x < data.Length; x++)
                {
                    if (data[y][x] == '#')
                    {
                        callback(x, y);
                    }
                }
            }
        }

        int Cycle(int n)
        {
            var _cubes = new Dictionary<(int x, int y, int z), bool>();
            var _temp = new Dictionary<(int x, int y, int z), bool>();

            Fill((_x, _y) => _cubes.Add((_x, _y, 0), true));

            var ways = new List<(int, int, int)>();
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    for (int c = -1; c < 2; c++)
                    {
                        if (a == 0 & b == 0 & c == 0)
                        {
                            continue;
                        }

                        ways.Add((a, b, c));
                    }
                }
            }

            while (n-- > 0)
            {
                _temp.Clear();

                var MinX = _cubes.Min(x => x.Key.x) - 1;
                var MinY = _cubes.Min(x => x.Key.y) - 1;
                var MinZ = _cubes.Min(x => x.Key.z) - 1;

                var MaxX = _cubes.Max(x => x.Key.x) + 1;
                var MaxY = _cubes.Max(x => x.Key.y) + 1;
                var MaxZ = _cubes.Max(x => x.Key.z) + 1;

                for (var z = MinZ; z <= MaxZ; z++)
                {
                    for (var y = MinY; y <= MaxY; y++)
                    {
                        for (var x = MinX; x <= MaxX; x++)
                        {
                            var active = 0;
                            foreach (var w in ways)
                            {
                                var nx = x + w.Item1;
                                var ny = y + w.Item2;
                                var nz = z + w.Item3;

                                if (_cubes.TryGetValue((nx, ny, nz), out var state) && state)
                                {
                                    active++;
                                }
                            }

                            if (_cubes.TryGetValue((x, y, z), out var s) && s)
                            {
                                _temp[(x, y, z)] = active == 2 || active == 3;
                            }
                            if (active == 3)
                            {
                                _temp[(x, y, z)] = true;
                            }
                        }
                    }
                }

                _cubes.Clear();
                foreach (var kv in _temp)
                {
                    _cubes.Add(kv.Key, kv.Value);
                }
            }

            return _cubes.Where(x => x.Value == true).Count();
        }

        int Cycle2(int n)
        {
            var _cubes = new Dictionary<(int x, int y, int z, int w), bool>();
            var _temp = new Dictionary<(int x, int y, int z, int w), bool>();

            Fill((_x, _y) => _cubes.Add((_x, _y, 0, 0), true));

            var ways = new List<(int, int, int, int)>();
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    for (int c = -1; c < 2; c++)
                    {
                        for (int d = -1; d < 2; d++)
                        {
                            if (a == 0 & b == 0 & c == 0 && d == 0)
                            {
                                continue;
                            }

                            ways.Add((a, b, c, d));
                        }
                    }
                }
            }

            while (n-- > 0)
            {
                _temp.Clear();

                var MinX = _cubes.Min(x => x.Key.x) - 1;
                var MinY = _cubes.Min(x => x.Key.y) - 1;
                var MinZ = _cubes.Min(x => x.Key.z) - 1;
                var MinW = _cubes.Min(x => x.Key.w) - 1;

                var MaxX = _cubes.Max(x => x.Key.x) + 1;
                var MaxY = _cubes.Max(x => x.Key.y) + 1;
                var MaxZ = _cubes.Max(x => x.Key.z) + 1;
                var MaxW = _cubes.Max(x => x.Key.w) + 1;

                for (var z = MinZ; z <= MaxZ; z++)
                {
                    for (var y = MinY; y <= MaxY; y++)
                    {
                        for (var x = MinX; x <= MaxX; x++)
                        {
                            for (var w = MinW; w <= MaxW; w++)
                            {
                                var active = 0;
                                foreach (var mw in ways)
                                {
                                    var nx = x + mw.Item1;
                                    var ny = y + mw.Item2;
                                    var nz = z + mw.Item3;
                                    var nw = w + mw.Item4;

                                    if (_cubes.TryGetValue((nx, ny, nz, nw), out var state) && state)
                                    {
                                        active++;
                                    }
                                }

                                if (_cubes.TryGetValue((x, y, z, w), out var s) && s)
                                {
                                    _temp[(x, y, z, w)] = active == 2 || active == 3;
                                }
                                if (active == 3)
                                {
                                    _temp[(x, y, z, w)] = true;
                                }
                            }
                        }
                    }
                }

                _cubes.Clear();
                foreach (var kv in _temp)
                {
                    _cubes.Add(kv.Key, kv.Value);
                }
            }

            return _cubes.Where(x => x.Value == true).Count();
        }

        public object Part1() => Cycle(6);

        public object Part2() => Cycle2(6);
    }
}