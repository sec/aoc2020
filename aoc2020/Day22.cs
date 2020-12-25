using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 32489
    /// Part 2: 35676
    /// </summary>
    public class Day22 : IDay
    {
        private readonly Queue<int> _deck1;
        private readonly Queue<int> _deck2;

        public Day22()
        {
            _deck1 = new Queue<int>();
            _deck2 = new Queue<int>();
        }

        void Reset()
        {
            _deck1.Clear();
            _deck2.Clear();

            var current = _deck1;

            foreach (var line in Inputs.Day22.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line == "Player 2:")
                {
                    current = _deck2;
                }
                else if (int.TryParse(line, out var d))
                {
                    current.Enqueue(d);
                }
            }
        }

        long Combat(bool partTwo)
        {
            Reset();

            var playerWins = Play(partTwo ? 1 : 0, _deck1, _deck2);
            var winner = playerWins ? _deck1 : _deck2;

            return winner.Select((v, i) => v * (winner.Count - i)).Sum();
        }

        bool Play(int game, Queue<int> player, Queue<int> crab)
        {
            var check = new HashSet<string>();

            while (player.Count > 0 && crab.Count > 0)
            {
                if (game > 0)
                {
                    foreach (var deck in new[] { player, crab })
                    {
                        var order = string.Join(",", deck);
                        if (check.Contains(order))
                        {
                            return true;
                        }
                        check.Add(order);
                    }
                }

                var playerCard = player.Dequeue();
                var crabCard = crab.Dequeue();

                var playerWins = playerCard > crabCard;

                if (game > 0)
                {
                    if (player.Count >= playerCard && crab.Count >= crabCard)
                    {
                        var newPlayer = new Queue<int>(player.Take(playerCard));
                        var newCrab = new Queue<int>(crab.Take(crabCard));

                        playerWins = Play(game + 1, newPlayer, newCrab);
                    }
                }

                if (playerWins)
                {
                    player.Enqueue(playerCard);
                    player.Enqueue(crabCard);
                }
                else
                {
                    crab.Enqueue(crabCard);
                    crab.Enqueue(playerCard);
                }
            }

            return player.Count > 0;
        }

        public object Part1() => Combat(false);

        public object Part2() => Combat(true);
    }
}