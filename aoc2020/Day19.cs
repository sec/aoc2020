using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 122
    /// Part 2: 287
    /// </summary>
    public class Day19 : IDay
    {
        private readonly Dictionary<int, string> _rules;
        private readonly List<string> _messages;
        private readonly string[] _start;

        public Day19()
        {
            _rules = new Dictionary<int, string>();
            _messages = new List<string>();

            foreach (var line in Inputs.Day19.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                if (char.IsNumber(line[0]))
                {
                    var d = line.Split(": ");
                    var index = int.Parse(d[0]);

                    _rules[index] = $"({d[1].Trim('"')})";
                }
                else
                {
                    _messages.Add(line);
                }
            }
            _start = _rules[0].Trim('(').Trim(')').Split(' ').ToArray();
        }

        string Expand(string part)
        {
            if (int.TryParse(part, out var index))
            {
                return _rules[index];
            }
            else if (!part.Any(x => char.IsNumber(x)))
            {
                return part;
            }
            else
            {
                var parts = part.Split(' ');

                for (var i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "|")
                    {
                        continue;
                    }

                    var d = Regex.Match(parts[i], @"\d+");
                    if (d.Success)
                    {
                        parts[i] = parts[i].Replace(d.Value, _rules[int.Parse(d.Value)]);
                    }
                }

                return $"({string.Join(" ", parts)})";
            }
        }

        Regex GetRegex(string[] input) => new Regex($@"^{string.Join(string.Empty, input).Replace(" ", string.Empty)}$");

        Regex Build(string[] start, Func<string[], bool> check)
        {
            while (check(start))
            {
                for (int i = 0; i < start.Length; i++)
                {
                    start[i] = Expand(start[i]);
                }
            }

            return GetRegex(start);
        }

        public int Solve()
        {
            var ans = int.MaxValue;

            Build(_start.ToArray(), regex =>
            {
                var r = GetRegex(regex);
                var c = _messages.Count(x => r.IsMatch(x));

                if (c == 0)
                {
                    return true;
                }

                if (c == ans)
                {
                    return false;
                }
                ans = c;

                return true;
            });

            return ans;
        }

        public object Part1() => Solve();

        public object Part2()
        {
            _rules[8] = "(42 | 42 8)";
            _rules[11] = "(42 31 | 42 11 31)";

            return Solve();
        }
    }
}