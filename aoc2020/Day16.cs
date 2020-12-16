using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 25961
    /// Part 2: 603409823791
    /// </summary>
    public class Day16 : IDay
    {
        private readonly List<List<int>> _tickets;
        private readonly List<int> _mine;
        private readonly Dictionary<string, List<(int min, int max)>> _rules;
        private readonly Dictionary<string, int> _positions;

        public Day16()
        {
            _tickets = new List<List<int>>();
            _mine = new List<int>();
            _rules = new Dictionary<string, List<(int min, int max)>>();
            _positions = new Dictionary<string, int>();

            var lines = Inputs.Day16.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var state = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("your ticket:"))
                {
                    state = 1;
                }
                else if (line.StartsWith("nearby tickets:"))
                {
                    state = 2;
                }
                else if (state == 0)
                {
                    var data = line.Split(':');
                    var name = data[0];
                    var ranges = data[1].Split(" or ");

                    _rules[name] = new List<(int min, int max)>();
                    _rules[name].Add(ToRange(ranges[0]));
                    _rules[name].Add(ToRange(ranges[1]));
                    _positions.Add(name, -1);
                }
                else
                {
                    var d = line.Split(',').Select(int.Parse).ToList();
                    if (state == 1)
                    {
                        _mine = d;
                    }
                    else
                    {
                        _tickets.Add(d);
                    }
                }
            }
        }

        private (int min, int max) ToRange(string v)
        {
            var x = v.Split('-');

            return (int.Parse(x[0]), int.Parse(x[1]));
        }

        private bool IsValid(int value) => _rules.Any(x => IsValidRule(value, x.Key));

        private bool IsValidRule(int value, string rule) => _rules[rule].Any(v => value >= v.min && value <= v.max);

        public object Part1() => _tickets.SelectMany(x => x).Where(x => !IsValid(x)).Sum();

        public object Part2()
        {
            _tickets.RemoveAll(x => x.Any(y => !IsValid(y)));

            while (_positions.Any(kv => kv.Value == -1))
            {
                foreach (var act in _positions.Where(x => x.Value == -1))
                {
                    var map = new int[_positions.Count];

                    for (int i = 0; i < _positions.Count; i++)
                    {
                        if (!_positions.Any(kv => kv.Value == i))
                        {
                            if (_tickets.All(x => IsValidRule(x[i], act.Key)))
                            {
                                map[i]++;
                            }
                        }
                    }

                    if (map.Count(x => x == 1) == 1)
                    {
                        _positions[act.Key] = Array.IndexOf(map, 1);
                        break;
                    }
                }
            }

            var toSum = _positions.Where(kv => kv.Key.StartsWith("departure")).Select(kv => kv.Value);
            var sum = 1L;

            foreach (var index in toSum)
            {
                sum *= _mine[index];
            }

            return sum;
        }
    }
}