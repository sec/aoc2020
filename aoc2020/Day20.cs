using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 66020135789767
    /// Part 2: 1537
    /// </summary>
    public class Day20 : IDay
    {
        static readonly Dictionary<byte, Tile> _tileMap = new Dictionary<byte, Tile>();
        static readonly Dictionary<(long, byte), Tile> _tileCache = new Dictionary<(long, byte), Tile>();

        readonly Random _random = new Random();
        readonly int _size;
        List<long> _corners;

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
            public const int SIZE = 10;

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
                if (tile != null)
                {
                    for (int i = 0; i < SIZE; i++)
                    {
                        if (Map[i, SIZE - 1] != tile.Map[i, 0])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            internal bool LeftSideWithRight(Tile tile)
            {
                if (tile != null)
                {
                    for (int i = 0; i < SIZE; i++)
                    {
                        if (Map[i, 0] != tile.Map[i, SIZE - 1])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            internal bool UpSideWithBottom(Tile tile)
            {
                if (tile != null)
                {
                    for (int i = 0; i < SIZE; i++)
                    {
                        if (Map[0, i] != tile.Map[SIZE - 1, i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            internal bool BottomSideWithUp(Tile tile)
            {
                if (tile != null)
                {
                    for (int i = 0; i < SIZE; i++)
                    {
                        if (Map[SIZE - 1, i] != tile.Map[0, i])
                        {
                            return false;
                        }
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
            readonly bool[][] _monster = new[]
            {
                "                  #".Select(x => x == '#').ToArray(),
                "#    ##    ##    ###".Select(x => x == '#').ToArray(),
                " #  #  #  #  #  #".Select(x => x == '#').ToArray()
            };

            readonly Tile[,] _map;
            readonly int _n;

            public Image(int n, Tile first)
            {
                _n = n;
                _map = new Tile[n, n];
                _map[0, 0] = first;
            }

            public bool IsValid()
            {
                for (int y = 0; y < _n; y++)
                {
                    for (int x = 0; x < _n; x++)
                    {
                        if (_map[y, x] == null)
                        {
                            continue;
                        }

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

            internal IEnumerable<long> UsedIds()
            {
                for (int y = 0; y < _n; y++)
                {
                    for (int x = 0; x < _n; x++)
                    {
                        if (_map[y, x] != null)
                        {
                            yield return _map[y, x].Id;
                        }
                    }
                }
            }

            internal bool Solve()
            {
                for (int y = 0; y < _n; y++)
                {
                    for (int x = 0; x < _n; x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }

                        var next = _tileCache.Where(x => !UsedIds().Contains(x.Key.Item1)).ToList();
                        var found = false;
                        foreach (var n in next)
                        {
                            _map[y, x] = n.Value;
                            if (IsValid())
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private bool[,] GetMap()
            {
                var newsize = Tile.SIZE - 2;
                var map = new bool[_n * newsize, _n * newsize];

                for (int y = 0; y < _n; y++)
                {
                    for (int x = 0; x < _n; x++)
                    {
                        var tile = _map[y, x];

                        for (int j = 0; j < newsize; j++)
                        {
                            for (int i = 0; i < newsize; i++)
                            {
                                map[y * newsize + j, x * newsize + i] = tile.Map[j + 1, i + 1];
                            }
                        }
                    }
                }

                return map;
            }

            public int RemoveMonstersAndCountWaters()
            {
                var map = GetMap();
                var monsters = false;
                var newsize = map.GetLength(0);

                for (int y = 0; y < newsize; y++)
                {
                    for (int x = 0; x < newsize; x++)
                    {
                        var found = true;
                        for (int j = 0; j < 3 && found; j++)
                        {
                            for (int i = 0; i < _monster[j].Length && found; i++)
                            {
                                if (y + j >= newsize || x + i >= newsize)
                                {
                                    found = false;
                                }
                                else if (_monster[j][i] && !map[y + j, x + i])
                                {
                                    found = false;
                                }
                            }
                        }

                        if (found)
                        {
                            monsters = true;
                            for (int j = 0; j < 3; j++)
                            {
                                for (int i = 0; i < _monster[j].Length; i++)
                                {
                                    if (_monster[j][i])
                                    {
                                        map[y + j, x + i] = false;
                                    }
                                }
                            }
                        }
                    }
                }

                if (monsters)
                {
                    int count = 0;
                    for (int y = 0; y < newsize; y++)
                    {
                        for (int x = 0; x < newsize; x++)
                        {
                            if (map[y, x])
                            {
                                count++;
                            }
                        }
                    }
                    return count;
                }

                return 0;
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

                        counts[tile.Id] += tile.LeftSideWithRight(next.Value) ? 1 : 0;
                        counts[tile.Id] += tile.RightSideWithLeft(next.Value) ? 1 : 0;
                        counts[tile.Id] += tile.UpSideWithBottom(next.Value) ? 1 : 0;
                        counts[tile.Id] += tile.BottomSideWithUp(next.Value) ? 1 : 0;
                    }
                }
            }

            _corners = counts.Where(x => x.Value == 16).Select(x => x.Key).ToList();

            return _corners.Aggregate(1L, (a, b) => a * b);
        }

        public object Part2()
        {
            int i = 0;

            foreach (var corner in _corners)
            {
                var tiles = _tileCache.Where(x => x.Key.Item1 == corner).Select(x => x.Value);

                foreach (var tile in tiles)
                {
                    var image = new Image(_size, tile);
                    if (image.Solve())
                    {
                        i += image.RemoveMonstersAndCountWaters();
                    }
                }
            }

            return i;
        }
    }
}