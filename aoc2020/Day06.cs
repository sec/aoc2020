using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 6763
    /// Part 2: 3512
    /// </summary>
    public class Day06 : IDay
    {
        class Form
        {
            public int[] Answers;
            public int Number;

            public Form()
            {
                Answers = new int[26];
                Number = 0;
            }
        }

        private readonly List<Form> _data;

        public Day06()
        {
            _data = new List<Form>();

            foreach (var ans in Inputs.Day06.Split(Environment.NewLine))
            {
                if (string.IsNullOrEmpty(ans))
                {
                    AddEmpty();
                }
                else
                {
                    foreach (var c in ans)
                    {
                        _data.Last().Answers['z' - c]++;
                    }
                    _data.Last().Number++;
                }
            }
        }

        void AddEmpty() => _data.Add(new Form());

        public object Part1() => _data.Where(x => x.Number != 0).Sum(x => x.Answers.Where(x => x != 0).Count());

        public object Part2() => _data.Where(x => x.Number != 0).Sum(x => x.Answers.Where(y => y == x.Number).Count());
    }
}