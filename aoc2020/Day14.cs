using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 7817357407588
    /// Part 2: 4335927555692
    /// </summary>
    public class Day14 : IDay
    {
        private readonly List<string> _code;
        private readonly string _and = "111111111111111111111111111111111111";
        private readonly string _or = "000000000000000000000000000000000000";

        public Day14()
        {
            _code = Inputs.Day14.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private static void GetIndexAndValue(string code, out int index, out long val)
        {
            var match = Regex.Match(code, @"(\d+)....(\d+)");

            index = int.Parse(match.Groups[1].Value);
            val = long.Parse(match.Groups[2].Value);
        }

        public object Part1()
        {
            var mem = new Dictionary<long, long>();
            var orMask = 0L;
            var andMask = 0L;

            foreach (var code in _code)
            {
                if (code.StartsWith("mask ="))
                {
                    var tmpOr = _or.ToArray();
                    var tmpAnd = _and.ToArray();
                    var mask = code[7..];

                    for (int i = 0; i < mask.Length; i++)
                    {
                        var c = mask[i];
                        if (c == '0')
                        {
                            tmpAnd[i] = c;
                        }
                        else if (c == '1')
                        {
                            tmpOr[i] = c;
                        }
                    }

                    orMask = Convert.ToInt64(string.Join(string.Empty, tmpOr), 2);
                    andMask = Convert.ToInt64(string.Join(string.Empty, tmpAnd), 2);
                }
                else
                {
                    GetIndexAndValue(code, out var index, out var val);

                    mem[index] = (val | orMask) & andMask;
                }
            }

            return mem.Values.Sum();
        }

        public object Part2()
        {
            var mem = new Dictionary<long, long>();
            var mask = string.Empty;

            foreach (var code in _code)
            {
                if (code.StartsWith("mask ="))
                {
                    mask = code[7..];
                }
                else
                {
                    GetIndexAndValue(code, out var index, out var val);
                    //
                    var addr = new List<char[]>();
                    var tmp = Convert.ToString(index, 2).PadLeft(mask.Length, '0').ToCharArray();
                    for (int i = 0; i < mask.Length; i++)
                    {
                        tmp[i] = mask[i] switch
                        {
                            '0' => tmp[i],
                            '1' => '1',
                            'X' => 'X',
                            _ => throw new NotImplementedException()
                        };
                    }
                    //
                    addr.Add(tmp);
                    while (addr.Any(x => x.Any(y => y == 'X')))
                    {
                        var i = addr.FindIndex(x => x.Any(y => y == 'X'));
                        var v = addr[i];

                        addr.RemoveAt(i);

                        var ni = Array.IndexOf(v, 'X');
                        var n1 = v.ToArray();
                        var n2 = v.ToArray();
                        n1[ni] = '0';
                        n2[ni] = '1';

                        addr.Add(n1);
                        addr.Add(n2);
                    }
                    //
                    foreach (var a in addr)
                    {
                        var i = Convert.ToInt64(string.Join(string.Empty, a), 2);

                        mem[i] = val;
                    }
                }
            }

            return mem.Values.Sum();
        }
    }
}