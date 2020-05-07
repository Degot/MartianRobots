using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;
using MartianRobots.Common.Utils;

namespace MartianRobots
{
    public class RobotFactory : IRobotFactory
    {
        public Robot CreateRobot(IReadOnlyList<string> args)
        {
            if (!(args.Count == 3 && args.Take(2).All(p => int.TryParse(p, out var i) && i >= 0) && OrientationManager.Instance.IsValid(args[2])))
            {
                throw new ArgumentException($"Robot must be initiated with positive X, Y and Orientation. Provided: {string.Join(", ", args)}");
            }
            return new Robot() { Position = new Position(int.Parse(args[0]), int.Parse(args[1])), Orientation = args[2], Lost = false };
        }
    }
}