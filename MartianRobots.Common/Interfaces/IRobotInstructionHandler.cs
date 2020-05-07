using System.Collections.Generic;
using MartianRobots.Common.Domain;

namespace MartianRobots.Common.Interfaces
{
    public interface IRobotInstructionHandler
    {
        char Key { get; }

        //Only Instruction knows how to parse its arguments
        Instruction ParseArguments(IEnumerator<string[]> tokenIterator);
        Robot Execute(Robot robot);
    }
}