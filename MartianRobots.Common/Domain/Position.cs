using System;

namespace MartianRobots.Common.Domain
{
    public class Position : IEquatable<Position>
    {
        public readonly int X;
        public readonly int Y;
        
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position(Position position)
        {
            X = position.X;
            Y = position.Y;
        }
        
        public Position Translate(Position delta)
        {
            return new Position(X + delta.X, Y + delta.Y);
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}