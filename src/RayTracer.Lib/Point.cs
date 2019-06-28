using System;

namespace RayTracer.Lib
{
    public struct Point : IEquatable<Point>
    {
        public Tuple Value { get; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float W => Value.W;

        public Point(float x, float y, float z)
        {
            Value = new Tuple(x, y, z, 1);
        }

        public Point(Tuple tuple) : this(tuple.X, tuple.Y, tuple.Z)
        {
        }
        
        public static Vector operator -(Point left, Point right)
        {
            var diff = left.Value - right.Value;
            
            return new Vector(diff);
        }
        
        public static Point operator -(Point left, Vector right)
        {
            var diff = left.Value - right.Value;
            
            return new Point(diff);
        }

        public static Point operator +(Point left, Vector right)
        {
            var sum = left.Value + right.Value;
            
            return new Point(sum);
        }

        public bool Equals(Point other)
        {
            return Value.Equals(other.Value);
        }
        
        public override bool Equals(object obj)
        {
            return obj != null && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"Point({X}, {Y}, {Z}, {W})";
        }
    }
}