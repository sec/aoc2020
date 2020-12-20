using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 66020135789767
    /// </summary>
    public class Day20 : IDay
    {
        static readonly Dictionary<byte, Tile> _tileMap = new Dictionary<byte, Tile>();
        static readonly Dictionary<(long, byte), Tile> _tileCache = new Dictionary<(long, byte), Tile>();

        readonly Random _random = new Random();
        readonly int _size;

        public Day20()
        {
            var input = Inputs.Day20
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            for (byte i = 0; i < input.Count / 11; i++)
            {
                var tile = input.Skip(i * 11).Take(11).ToList();
                var id = long.Parse(tile[0][5..^1]);

                var t = new Tile(id, tile.Skip(1).ToList());

                _tileMap[i] = t;

                for (byte v = 0; v < 8; v++)
                {
                    var key = (id, v);
                    var map = t.Get(v);
                    _tileCache[key] = new Tile(t.Id, map, v);
                }
            }

            _size = (int) Math.Sqrt(_tileMap.Count);
        }

        class Tile
        {
            const int SIZE = 10;

            public bool[,] Map = new bool[SIZE, SIZE];
            public long Id;
            public int Variant;

            readonly Dictionary<byte, bool[,]> _cache = new Dictionary<byte, bool[,]>();

            public Tile(long id, IList<string> enumerable)
            {
                Id = id;
                for (int y = 0; y < SIZE; y++)
                {
                    for (int x = 0; x < SIZE; x++)
                    {
                        Map[y, x] = enumerable[y][x] == '#';
                    }
                }
            }

            public Tile(long id, bool[,] map, int variant)
            {
                Id = id;
                Map = map;
                Variant = variant;
            }

            public bool[,] Get(byte n)
            {
                if (!_cache.ContainsKey(n))
                {
                    _cache[n] = n switch
                    {
                        0 => Map,
                        1 => Rotate(Map),
                        2 => Rotate(Get(1)),
                        3 => Rotate(Get(2)),
                        4 => Flip(Map),
                        5 => Rotate(Get(4)),
                        6 => Rotate(Get(5)),
                        7 => Rotate(Get(6)),
                        _ => throw new NotImplementedException(),
                    };
                }
                return _cache[n];
            }

            public override string ToString() => $"{Id} / {Variant}";

            internal bool RightSideWithLeft(Tile tile)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    if (Map[i, SIZE - 1] != tile.Map[i, 0])
                    {
                        return false;
                    }
                }
                return true;
            }

            internal bool LeftSideWithRight(Tile tile)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    if (Map[i, 0] != tile.Map[i, SIZE - 1])
                    {
                        return false;
                    }
                }
                return true;
            }

            internal bool UpSideWithBottom(Tile tile)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    if (Map[0, i] != tile.Map[SIZE - 1, i])
                    {
                        return false;
                    }
                }
                return true;
            }

            internal bool BottomSideWithUp(Tile tile)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    if (Map[SIZE - 1, i] != tile.Map[0, i])
                    {
                        return false;
                    }
                }
                return true;
            }

            static T[,] Rotate<T>(T[,] src)
            {
                var n = src.GetLength(0);
                var tmp = new T[n, n];

                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        tmp[x, n - y - 1] = src[y, x];
                    }
                }

                return tmp;
            }

            static T[,] Flip<T>(T[,] src)
            {
                var n = src.GetLength(0);
                var tmp = new T[n, n];

                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        tmp[y, n - x - 1] = src[y, x];
                    }
                }

                return tmp;
            }
        }

        class Image
        {
            readonly Tile[,] _map;
            readonly int _n;

            public Image(int n, byte[] input)
            {
                _n = n;
                _map = new Tile[n, n];

                int index = 0;

                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        var src = _tileMap[input[index]];
                        var variant = input[index + 1];
                        var key = (src.Id, variant);

                        _map[y, x] = _tileCache[key];

                        index += 2;
                    }
                }
            }

            public bool IsValid()
            {
                for (int y = 0; y < _n; y++)
                {
                    for (int x = 0; x < _n; x++)
                    {
                        if (x + 1 < _n)
                        {
                            if (!_map[y, x].RightSideWithLeft(_map[y, x + 1]))
                            {
                                return false;
                            }
                        }

                        if (y + 1 < _n)
                        {
                            if (!_map[y, x].BottomSideWithUp(_map[y + 1, x]))
                            {
                                return false;
                            }
                        }

                        if (y > 0)
                        {
                            if (!_map[y, x].UpSideWithBottom(_map[y - 1, x]))
                            {
                                return false;
                            }
                        }

                        if (x - 1 >= 0)
                        {
                            if (!_map[y, x].LeftSideWithRight(_map[y, x - 1]))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            public long CRC()
            {
                return _map[0, 0].Id * _map[0, _n - 1].Id * _map[_n - 1, 0].Id * _map[_n - 1, _n - 1].Id;
            }
        }

        public object Part1()
        {
            var counts = new Dictionary<long, int>();
            foreach (var kv in _tileMap)
            {
                counts[kv.Value.Id] = 0;
            }

            foreach (var kv in _tileMap)
            {
                var tiles = _tileCache.Where(x => x.Key.Item1 == kv.Value.Id).Select(x => x.Value).ToList();
                foreach (var tile in tiles)
                {
                    foreach (var next in _tileCache)
                    {
                        if (next.Key.Item1 == tile.Id)
                        {
                            continue;
                        }
                        var i = 0;

                        if (tile.LeftSideWithRight(next.Value)) i++;
                        if (tile.RightSideWithLeft(next.Value)) i++;
                        if (tile.UpSideWithBottom(next.Value)) i++;
                        if (tile.BottomSideWithUp(next.Value)) i++;

                        counts[tile.Id] += i;
                    }
                }
            }

            var min = counts.Values.Min();
            var edge = counts.Where(x => x.Value == min).Select(x => x.Key);
            var ans = edge.Aggregate(1L, (a, b) => a * b);

            return ans;
        }

        public object Part2()
        {
            var corners = new[] { 1951, 1171, 2971, 3079 };

            return 0;
        }

        void Print<T>(T[,] src)
        {
            for (int y = 0; y < src.GetLength(0); y++)
            {
                for (int x = 0; x < src.GetLength(0); x++)
                {
                    Console.Write(src[y, x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}