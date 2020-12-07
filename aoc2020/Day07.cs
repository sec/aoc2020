using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 348
    /// Part 2: 18885
    /// </summary>
    public class Day07 : IDay
    {
        class Bag
        {
            public string Name { get; private set; }
            public List<(int Count, Bag Bag)> Bags { get; private set; }

            public Bag(string name)
            {
                Name = name;
                Bags = new List<(int, Bag)>();
            }

            internal bool IsInsideSomewhere(string lookingFor) => Bags.Any(x => x.Bag.Name == lookingFor || x.Bag.IsInsideSomewhere(lookingFor));

            internal int HowManyInside() => Bags.Sum(x => x.Count + x.Count * x.Bag.HowManyInside());
        }

        private readonly List<Bag> _bags;

        public Day07()
        {
            _bags = new List<Bag>();

            var data = Inputs.Day07
                .Split(Environment.NewLine)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var row in data)
            {
                if (!row.Contains("no other bags"))
                {
                    var source = row[..(row.IndexOf("bags") - 1)];
                    var rest = row[(row.IndexOf("bags contain") + "bags contain".Length + 1)..].Split(", ");

                    foreach (var b in rest)
                    {
                        var info = b.Substring(0, b.LastIndexOf(' '));
                        var parts = info.Split(' ', 2);

                        GetBag(source).Bags.Add((int.Parse(parts[0]), GetBag(parts[1])));
                    }
                }
            }
        }

        Bag GetBag(string name)
        {
            if (!_bags.Any(x => x.Name == name))
            {
                _bags.Add(new Bag(name));
            }
            return _bags.Single(x => x.Name == name);
        }

        public object Part1() => _bags.Count(x => x.IsInsideSomewhere("shiny gold"));

        public object Part2() => GetBag("shiny gold").HowManyInside();
    }
}