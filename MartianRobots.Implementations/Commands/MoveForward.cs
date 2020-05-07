using System.Collections.Generic;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;
using MartianRobots.Common.Utils;

namespace MartianRobots.Implementations.Commands
{
    class MoveForward : IRobotInstructionHandler
    {
        public char Key => 'F';


        public Instruction ParseArguments(IEnumerator<string[]> tokenIterator)
        {
            tokenIterator.MoveNext();
            return new Instruction(Key);
        }

        public Robot Execute(Robot robot)
        {
            var clone = robot.Clone(robot);
            var delta = OrientationManager.Instance.GetPositionDelta(robot.Orientation);
            clone.Position = clone.Position.Translate(delta);
            return clone;
        }
    }
}