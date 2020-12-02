using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    public class Day02
    {
        class Password
        {
            private readonly int _min, _max;
            private readonly char _c;
            private readonly string _password;

            public Password(string line)
            {
                var d = line.Trim().Split(' ', 3);
                var n = d[0].Split('-');

                _c = d[1][0];
                _password = d[2];
                _min = int.Parse(n[0]);
                _max = int.Parse(n[1]);
            }

            public bool IsValid()
            {
                var count = _password.Where(x => x == _c).Count();

                return count >= _min && count <= _max;
            }

            public bool IsValidRange()
            {
                var c1 = _password[_min - 1];
                var c2 = _password[_max - 1];

                return (c1 == _c || c2 == _c) && (c1 != c2);
            }

            public override string ToString() => $"{_min}-{_max} {_c}: {_password}";
        }

        private readonly List<Password> _passwd;

        public Day02()
        {
            _passwd = Inputs.Day02
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new Password(x)).ToList();
        }

        internal object Part1() => _passwd.Count(x => x.IsValid());

        internal object Part2() => _passwd.Count(x => x.IsValidRange());
    }
}