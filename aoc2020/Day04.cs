using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2020
{
    /// <summary>
    /// P1 - 210
    /// P2 - 131
    /// </summary>
    public class Day04 : IDay
    {
        class Passport
        {
            private readonly IDictionary<string, string> _data;

            public Passport(IList<string> data)
            {
                _data = new Dictionary<string, string>();

                foreach (var d in data)
                {
                    d.Split(' ')
                        .Select(x => x.Split(':'))
                        .ToList()
                        .ForEach(x => _data.Add(x[0], x[1]));
                }
            }

            public bool IsValid(bool checkValue)
            {
                var f = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

                foreach (var c in f)
                {
                    if (!_data.ContainsKey(c))
                    {
                        return false;
                    }
                    if (checkValue && !ValidField(c))
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool ValidField(string field)
            {
                var v = _data[field];

                return field switch
                {
                    "byr" => InRange(v, 1920, 2002),
                    "iyr" => InRange(v, 2010, 2020),
                    "eyr" => InRange(v, 2020, 2030),
                    "hgt" => ValidHgt(v),
                    "hcl" => Regex.IsMatch(v, @"^\#[0-9a-f]{6}$"),
                    "ecl" => Regex.IsMatch(v, @"^(amb|blu|brn|gry|grn|hzl|oth)$"),
                    "pid" => Regex.IsMatch(v, @"^\d{9}$"),
                    _ => throw new NotImplementedException(),
                };

                static bool ValidHgt(string v)
                {
                    if (v.EndsWith("cm") && InRange(v[0..^2], 150, 193))
                    {
                        return true;

                    }
                    else if (v.EndsWith("in") && InRange(v[0..^2], 59, 76))
                    {
                        return true;
                    }

                    return false;
                }

                static bool InRange(string s, int left, int right) => int.TryParse(s, out var i) && i >= left && i <= right;
            }

            public override string ToString() => string.Join(" ", _data.OrderBy(x => x.Key).Select(x => $"{x.Key}:{x.Value}"));
        }

        private readonly IList<Passport> _passports;

        public Day04()
        {
            _passports = new List<Passport>();

            var tmp = new List<string>();
            foreach (var line in Inputs.Day04.Split(Environment.NewLine))
            {
                if (string.IsNullOrEmpty(line))
                {
                    _passports.Add(new Passport(tmp));
                    tmp.Clear();
                }
                else
                {
                    tmp.Add(line);
                }
            }

            if (tmp.Count > 0)
            {
                _passports.Add(new Passport(tmp));
            }
        }

        public object Part1() => _passports.Count(x => x.IsValid(false));

        public object Part2() => _passports.Count(x => x.IsValid(true));
    }
}