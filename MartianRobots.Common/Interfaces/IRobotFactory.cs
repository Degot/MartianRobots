using System.Collections.Generic;
using MartianRobots.Common.Domain;

namespace MartianRobots.Common.Interfaces
{
    public interface IRobotFactory
    {
        Robot CreateRobot(IReadOnlyList<string> args);
    }
}
