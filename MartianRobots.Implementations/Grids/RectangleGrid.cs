using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Common;
using MartianRobots.Common.Domain;

namespace MartianRobots.Implementations
{
    public class RectangleGrid : IGrid
    {
        private int _width;
        private int _height;
        private Dictionary<Robot, HashSet<Instruction>> _graveyardNotes;

        public RectangleGrid()
        {
            Reset();
        }
        public void Reset()
        {
            _width = -1;
            _height = -1;
            _graveyardNotes = new Dictionary<Robot, HashSet<Instruction>>();
        }

        public void Init(Instruction instruction)
        {
            var args = instruction.Arguments;

            if (!(args.Count == 2 && args.All(p => int.TryParse(p, out var i) && i >= 0)))
            {
                throw new ArgumentOutOfRangeException(nameof(instruction), $"Grid can be initiated with two positive integers: Width, Height. Provided: {string.Join(", ", args)}");
            }

            _width = int.Parse(args[0]) + 1;
            _height = int.Parse(args[1]) + 1;

        }

        public bool IsValidPosition(Position position)
        {
            return ((position.X >= 0 && position.X < _width) && (position.Y >= 0 && position.Y < _height));
        }

        public void AddScent(Robot robot, Instruction instruction)
        {
            if (!_graveyardNotes.ContainsKey(robot))
            {
                _graveyardNotes.Add(robot, new HashSet<Instruction>());
            }

            if (!_graveyardNotes[robot].Contains(instruction))
            {
                _graveyardNotes[robot].Add(instruction);
            }
        }

        public bool HasScent(Robot robot, Instruction instruction)
        {
            if (_graveyardNotes.ContainsKey(robot))
            {
                if (_graveyardNotes[robot].Contains(instruction))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
