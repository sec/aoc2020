using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 25190263477788
    /// Part 2: 297139939002972
    /// </summary>
    public class Day18 : IDay
    {
        private readonly List<string> _input;

        long SimpleCalc(string input)
        {
            if (!input.Contains('+') && !input.Contains('*'))
            {
                return long.Parse(input.Trim('(').Trim(')'));
            }

            var left = new StringBuilder();
            var right = new StringBuilder();
            bool flag = true;
            bool add = true;
            long sum;

            foreach (var c in input)
            {
                if (char.IsNumber(c))
                {
                    if (flag)
                    {
                        left.Append(c);
                    }
                    else
                    {
                        right.Append(c);
                    }
                }
                else if (c == '+' || c == '*')
                {
                    if (flag == false)
                    {
                        Action();
                    }
                    add = c == '+';
                    flag = false;
                }
            }

            Action();

            void Action()
            {
                var n1 = long.Parse(left.ToString());
                var n2 = long.Parse(right.ToString());
                sum = add ? n1 + n2 : n1 * n2;

                left.Clear();
                right.Clear();

                left.Append(sum.ToString());
            }

            return sum;
        }

        long MakeItSimple(string s, Func<string, long> calc)
        {
            while (s.IndexOf('(') > -1)
            {
                var left = s.LastIndexOf('(');
                var right = left;

                while (s[right++] != ')') ;

                var action = s[left..right];
                var result = calc(action);

                s = s.Replace(action, result.ToString());
            }

            return calc(s);
        }

        long AlsoSimpleCalc(string input)
        {
            while (input.IndexOf('+') > -1)
            {
                var r = Regex.Match(input, @"\d+ \+ \d+");

                input = input.Replace(r.Value, SimpleCalc(r.Value).ToString());
            }

            return SimpleCalc(input);
        }

        public Day18()
        {
            _input = Inputs.Day18.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public object Part1() => _input.Select(x => MakeItSimple(x, SimpleCalc)).Sum();

        public object Part2() => _input.Select(x => MakeItSimple(x, AlsoSimpleCalc)).Sum();
    }
}