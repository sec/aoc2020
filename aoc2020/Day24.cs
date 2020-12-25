using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 263
    /// Part 2: 3649
    /// </summary>
    public class Day24 : IDay
    {
        const int N = 70;

        enum Way
        {
            e = 0,
            w,
            se,
            sw,
            nw,
            ne
        }

        private readonly List<List<Way>> _tiles;
        private readonly Dictionary<(int q, int r), bool> _floor;

        public Day24()
        {
            _floor = new Dictionary<(int q, int r), bool>();
            _tiles = new List<List<Way>>();
            foreach (var line in Inputs.Day24.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                _tiles.Add(ToWay(line).ToList());
            }

            for (int y = -N; y <= N; y++)
            {
                for (int x = -N; x <= N; x++)
                {
                    var p = (x, y);
                    if (!_floor.ContainsKey(p))
                    {
                        _floor[p] = false;
                    }
                }
            }

            Flip();
        }

        static IEnumerable<Way> ToWay(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'e')
                {
                    yield return Way.e;
                }
                else if (input[i] == 'w')
                {
                    yield return Way.w;
                }
                else if (input[i] == 'n')
                {
                    i++;
                    if (input[i] == 'e')
                    {
                        yield return Way.ne;
                    }
                    else
                    {
                        yield return Way.nw;
                    }
                }
                else if (input[i] == 's')
                {
                    i++;
                    if (input[i] == 'e')
                    {
                        yield return Way.se;
                    }
                    else
                    {
                        yield return Way.sw;
                    }
                }
            }
        }

        static (int q, int r) Offset(Way w)
        {
            return w switch
            {
                Way.w => (-1, 0),
                Way.e => (1, 0),

                Way.sw => (-1, 1),
                Way.se => (0, 1),

                Way.nw => (0, -1),
                Way.ne => (1, -1),

                _ => throw new NotImplementedException()
            };
        }

        void Flip()
        {
            foreach (var path in _tiles)
            {
                var q = 0;
                var r = 0;

                foreach (var w in path)
                {
                    var move = Offset(w);

                    q += move.q;
                    r += move.r;
                }

                var pos = (q, r);
                _floor[pos] = !_floor[pos];
            }
        }

        private int CountBlack((int q, int r) pos)
        {
            var cnt = 0;
            foreach (Way e in Enum.GetValues(typeof(Way)))
            {
                var (q, r) = Offset(e);
                var npos = (pos.q + q, pos.r + r);

                if (_floor.TryGetValue(npos, out var v) && v)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        public object Part1() => _floor.Where(kv => kv.Value == true).Count();

        public object Part2()
        {
            for (int i = 0; i < 100; i++)
            {
                var tmp = new List<(int q, int r, bool v)>();
                foreach (var tile in _floor)
                {
                    var blacks = CountBlack(tile.Key);
                    var final = tile.Value;
                    if (final && (blacks == 0 || blacks > 2))
                    {
                        final = false;
                    }
                    else if (blacks == 2)
                    {
                        final = true;
                    }
                    tmp.Add((tile.Key.q, tile.Key.r, final));
                }

                tmp.ForEach(x => _floor[(x.q, x.r)] = x.v);
            }

            return Part1();
        }
    }
}