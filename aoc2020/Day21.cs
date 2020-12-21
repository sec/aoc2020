using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 2734
    /// Part 2: kbmlt,mrccxm,lpzgzmk,ppj,stj,jvgnc,gxnr,plrlg
    /// </summary>
    public class Day21 : IDay
    {
        private readonly List<Product> _food;

        class Product
        {
            public List<string> Ingredients;
            public List<string> Allergens;

            public Product(string[] data)
            {
                Ingredients = data[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                Allergens = data[1].Trim(')').Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            public override string ToString() => $"{string.Join(", ", Ingredients)} ({string.Join(", ", Allergens)})";
        }

        public Day21()
        {
            _food = Inputs.Day21
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Product(x.Split(" (contains ")))
                .ToList();
        }

        private List<(string Allergen, List<string> Ingredients)> MapToList()
        {
            var list = new List<(string Allergen, List<string> Ingredients)>();
            foreach (var f in _food)
            {
                foreach (var allergen in f.Allergens)
                {
                    list.Add((allergen, f.Ingredients.ToList()));
                }
            }

            return list;
        }

        public object Part1()
        {
            var removed = new HashSet<string>();
            var list = MapToList();

            foreach (var group in list.GroupBy(x => x.Allergen))
            {
                var ingredients = group.Select(x => x.Ingredients).ToList();

                foreach (var ii in ingredients)
                {
                    foreach (var i in ii)
                    {
                        if (ingredients.All(x => x.Contains(i)))
                        {
                            removed.Add(i);
                        }
                    }
                }
            }

            var all = _food.SelectMany(x => x.Ingredients).Distinct();
            var clean = all.Where(x => !removed.Contains(x)).ToList();
            var count = _food.SelectMany(x => x.Ingredients).Where(x => clean.Contains(x)).Count();

            // Prepare for part #2
            foreach (var p in _food)
            {
                p.Ingredients = p.Ingredients.Where(x => removed.Contains(x)).ToList();
            }

            return count;
        }

        public object Part2()
        {
            var second = MapToList().GroupBy(x => x.Allergen).ToList();
            var map = new Dictionary<string, string>();

            while (second.Count > 0)
            {
                foreach (var group in second)
                {
                    var all = group
                        .SelectMany(x => x.Ingredients)
                        .GroupBy(x => x)
                        .Where(x => !map.ContainsKey(x.Key))
                        .ToDictionary(k => k.Key, v => v.Count());

                    var max = all.Values.Max();

                    if (all.Count(kv => kv.Value == max) == 1)
                    {
                        map.Add(all.Single(kv => kv.Value == max).Key, group.Key);
                        second.Remove(group);
                        break;
                    }
                }
            }

            return string.Join(",", map.OrderBy(x => x.Value).Select(x => x.Key));
        }
    }
}