using MartianRobots.Common.Domain;

namespace MartianRobots.Common
{
    public interface IGrid
    {
        void Reset();
        void Init(Instruction instruction);


        bool IsValidPosition(Position position);
        void AddScent(Robot robot, Instruction instruction);
        bool HasScent(Robot robot, Instruction instruction);
    }
}
