using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 43769582
    /// Part 2: 264692662390
    /// </summary>
    public class Day23 : IDay
    {
        const string INPUT = "467528193";

        public object Part1()
        {
            var cups = new LinkedList<int>(INPUT.ToCharArray().Select(x => int.Parse(x.ToString())));

            Move(cups, 100);

            var start = cups.Find(1);
            var sb = new StringBuilder();

            for (int i = 1; i < cups.Count; i++)
            {
                start = start.Next ?? cups.First;
                sb.Append(start.Value);
            }

            return sb.ToString();
        }

        public object Part2()
        {
            var cups = new LinkedList<int>(INPUT.ToCharArray().Select(x => int.Parse(x.ToString())));
            var next = cups.Max();

            while (cups.Count != 1_000_000)
            {
                cups.AddLast(++next);
            }

            Move(cups, 10_000_000);

            var start = cups.Find(1);

            var one = start.Next.Value;
            var two = start.Next.Next.Value;

            return (long) one * two;
        }

        void Move(LinkedList<int> cups, int moves)
        {
            var maxCup = cups.Max();

            var map = new Dictionary<int, LinkedListNode<int>>();
            for (var start = cups.First; start != null; start = start.Next)
            {
                map[start.Value] = start;
            }

            var current = cups.First;
            for (int i = 0; i < moves; i++)
            {
                var pickup = new List<LinkedListNode<int>>();
                var next = current.Next ?? current.List.First;
                for (int j = 0; j < 3; j++)
                {
                    pickup.Add(next);
                    next = next.Next ?? next.List.First;
                }

                foreach (var p in pickup)
                {
                    cups.Remove(p);
                }

                var destination = current.Value - 1;
                while (destination <= 0 || pickup.Any(x => x.Value == destination))
                {
                    destination--;
                    if (destination <= 0)
                    {
                        destination = maxCup;
                    }
                }

                var where = map[destination];
                for (int j = 2; j >= 0; j--)
                {
                    cups.AddAfter(where, pickup[j]);
                }

                current = current.Next ?? cups.First;
            }
        }
    }
}