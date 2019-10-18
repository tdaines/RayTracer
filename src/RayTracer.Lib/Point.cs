using System;

namespace RayTracer.Lib
{
    public struct Point : IEquatable<Point>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        public Point(float x, float y, float z, float w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        public static Vector operator -(Point left, Point right)
        {
            var x = left.X - right.X;
            var y = left.Y - right.Y;
            var z = left.Z - right.Z;
            
            return new Vector(x, y, z);
        }
        
        public static Point operator -(Point left, Vector right)
        {
            var x = left.X - right.X;
            var y = left.Y - right.Y;
            var z = left.Z - right.Z;
            
            return new Point(x, y, z);
        }

        public static Point operator +(Point left, Vector right)
        {
            var x = left.X + right.X;
            var y = left.Y + right.Y;
            var z = left.Z + right.Z;
            var w = left.W + right.W;
            
            return new Point(x, y, z, w);
        }

        public bool Equals(Point other)
        {
            return X.ApproximatelyEquals(other.X) 
                && Y.ApproximatelyEquals(other.Y)
                && Z.ApproximatelyEquals(other.Z)
                && W.ApproximatelyEquals(other.W);
        }
        
        public override bool Equals(object obj)
        {
            return (obj != null) 
                && (obj is Point point)
                && Equals(point);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode()
                 ^ Y.GetHashCode()
                 ^ Z.GetHashCode()
                 ^ W.GetHashCode();
        }

        public override string ToString()
        {
            return $"Point({X}, {Y}, {Z})";
        }
        
        public static Point Zero = new Point(0, 0, 0);
    }
}