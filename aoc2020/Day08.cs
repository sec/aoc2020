using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    /// <summary>
    /// Part 1: 1753
    /// Part 2: 733
    /// </summary>
    public class Day08 : IDay
    {
        class Code
        {
            public Instruction I { get; set; }
            public int V { get; set; }
        }

        enum Instruction
        {
            nop,
            acc,
            jmp
        }

        private List<Code> _code;
        private int _accumulator;
        private int _offset = 0;
        private int[] _map;
        private bool _done;

        public Day08()
        {
            Reset();
        }

        private void Reset()
        {
            _code = Inputs.Day08
                .Split(Environment.NewLine)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(' '))
                .Select(x => new Code()
                {
                    I = Enum.Parse<Instruction>(x[0]),
                    V = int.Parse(x[1])
                })
                .ToList();

            _map = new int[_code.Count];
            _done = false;
        }

        private void Step()
        {
            var c = _code[_offset];

            switch (c.I)
            {
                case Instruction.acc:
                    _accumulator += c.V;
                    GoNext(1);
                    break;

                case Instruction.jmp:
                    GoNext(c.V);
                    break;

                case Instruction.nop:
                    GoNext(1);
                    break;
            }
        }

        void GoNext(int offset)
        {
            _offset += offset;

            if (_offset >= _code.Count)
            {
                _done = true;
            }
            else
            {
                _map[_offset]++;
            }
        }

        void Run()
        {
            _accumulator = 0;
            _offset = 0;

            while (!_done && !_map.Any(x => x > 1))
            {
                Step();
            }
        }

        public object Part1()
        {
            Run();

            return _accumulator;
        }

        public object Part2()
        {
            for (int i = 0; i < _code.Count; i++)
            {
                Reset();

                if (_code[i].I == Instruction.nop)
                {
                    _code[i].I = Instruction.jmp;
                }
                else if (_code[i].I == Instruction.jmp)
                {
                    _code[i].I = Instruction.nop;
                }

                Run();

                if (_done)
                {
                    return _accumulator;
                }
            }
            throw new InvalidProgramException();
        }
    }
}