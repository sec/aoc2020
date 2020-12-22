using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 32489
    /// </summary>
    public class Day22 : IDay
    {
        private readonly Queue<int> _player;
        private readonly Queue<int> _crab;

        public Day22()
        {
            _player = new Queue<int>();
            _crab = new Queue<int>();

            var current = _player;

            foreach (var line in Inputs.Day22.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line == "Player 2:")
                {
                    current = _crab;
                }
                else if (int.TryParse(line, out var d))
                {
                    current.Enqueue(d);
                }
            }
        }

        public object Part1()
        {
            while (_player.Count > 0 && _crab.Count > 0)
            {
                var player = _player.Dequeue();
                var crab = _crab.Dequeue();

                if (player > crab)
                {
                    _player.Enqueue(player);
                    _player.Enqueue(crab);
                }
                else
                {
                    _crab.Enqueue(crab);
                    _crab.Enqueue(player);
                }
            }

            var winner = _crab.Count > 0 ? _crab : _player;

            return winner.Select((v, i) => v * (winner.Count - i)).Sum();
        }

        public object Part2()
        {
            return 0;
        }
    }
}