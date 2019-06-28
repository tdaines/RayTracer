using System;

namespace RayTracer.Lib
{
    public struct Tuple : IEquatable<Tuple>
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public Tuple(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Tuple operator +(Tuple left, Tuple right)
        {
            var x = left.X + right.X;
            var y = left.Y + right.Y;
            var z = left.Z + right.Z;
            var w = left.W + right.W;
            
            return new Tuple(x, y, z, w);
        }
        
        public static Tuple operator -(Tuple left, Tuple right)
        {
            var x = left.X - right.X;
            var y = left.Y - right.Y;
            var z = left.Z - right.Z;
            var w = left.W - right.W;
            
            return new Tuple(x, y, z, w);
        }
        
        public static Tuple operator -(Tuple value)
        {
            return new Tuple(-value.X, -value.Y, -value.Z, -value.W);
        }

        public static Tuple operator *(Tuple value, float scalar)
        {
            var x = value.X * scalar;
            var y = value.Y * scalar;
            var z = value.Z * scalar;
            var w = value.W * scalar;
            
            return new Tuple(x, y, z, w);
        }
        
        public static Tuple operator *(float scalar, Tuple value)
        {
            return value * scalar;
        }
        
        public static Tuple operator /(Tuple value, float scalar)
        {
            var x = value.X / scalar;
            var y = value.Y / scalar;
            var z = value.Z / scalar;
            var w = value.W / scalar;
            
            return new Tuple(x, y, z, w);
        }

        public bool Equals(Tuple other)
        {
            return X.ApproximatelyEquals(other.X)
                   && Y.ApproximatelyEquals(other.Y)
                   && Z.ApproximatelyEquals(other.Z)
                   && W.ApproximatelyEquals(other.W);
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals((Tuple) obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        public override string ToString()
        {
            return $"Tuple({X}, {Y}, {Z}, {W})";
        }
    }
}