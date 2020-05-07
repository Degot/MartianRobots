using System.Collections.Generic;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;
using MartianRobots.Common.Utils;

namespace MartianRobots.Implementations.Commands
{
    class TurnRight : IRobotInstructionHandler
    {
        public char Key => 'R';


        public Instruction ParseArguments(IEnumerator<string[]> tokenIterator)
        {
            tokenIterator.MoveNext();
            return new Instruction(Key);
        }

        public Robot Execute(Robot robot)
        {
            var clone = robot.Clone(robot);
            clone.Orientation = OrientationManager.Instance.TurnRight(robot.Orientation);
            return clone;
        }
    }
}
