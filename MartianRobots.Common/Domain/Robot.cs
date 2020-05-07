using System;

namespace MartianRobots.Common.Domain
{
    public class Robot : IEquatable<Robot>
    {
        public Position Position;
        public string Orientation;
        public bool Lost;

        public void Apply(Robot robot)
        {
            if (!Lost)
            {
                Position = robot.Position;
                Orientation = robot.Orientation;
            }
        }

        public Robot Clone(Robot robot)
        {
            return new Robot {Position = new Position(robot.Position), Orientation = robot.Orientation, Lost = robot.Lost};
        }

        public bool Equals(Robot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Position, other.Position) && string.Equals(Orientation, other.Orientation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Robot) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Position != null ? Position.GetHashCode() : 0) * 397) ^ (Orientation != null ? Orientation.GetHashCode() : 0);
            }
        }
    }
}
