using System.Collections.Generic;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;
using MartianRobots.Common.Utils;

namespace MartianRobots.Implementations.Commands
{
    class TurnLeft : IRobotInstructionHandler
    {
        public char Key => 'L';


        public Instruction ParseArguments(IEnumerator<string[]> tokenIterator)
        {
            tokenIterator.MoveNext();
            return new Instruction(Key);
        }

        public Robot Execute(Robot robot)
        {
            var clone = robot.Clone(robot);
            clone.Orientation = OrientationManager.Instance.TurnLeft(robot.Orientation);
            return clone;
        }
    }
}