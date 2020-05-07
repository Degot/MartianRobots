using System;
using System.Collections.Generic;
using System.Linq;

namespace MartianRobots.Common.Domain
{
    public class Instruction: IEquatable<Instruction>
    {
        public readonly char Key;
        public readonly IReadOnlyList<string> Arguments;

        public Instruction(char key, IList<string> args = null)
        {
            Key = key;
            Arguments = args == null ? new List<string>() : (IReadOnlyList<string>) args;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Instruction) obj);
        }

        public override int GetHashCode()
        {

            var codes = new List<int>();
            foreach (var item in Arguments)
            {
                codes.Add(item.GetHashCode());
            }
            codes.Sort();
            var hash = 0;
            foreach (int code in codes)
            {
                unchecked
                {
                    hash *= 251;
                    hash += code;
                }
            }
            
            unchecked
            {
                return (Key.GetHashCode() * 397) ^ hash;
            }
        }

        public bool Equals(Instruction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key == other.Key && Arguments.SequenceEqual(other.Arguments);
        }
    }
}
