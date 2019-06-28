using System;

namespace RayTracer.Lib
{
    public struct Vector : IEquatable<Vector>
    {
        public Tuple Value { get; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float W => Value.W;

        public Vector(float x, float y, float z)
        {
            Value = new Tuple(x, y, z, 0);
        }

        public Vector(Tuple tuple) : this(tuple.X, tuple.Y, tuple.Z)
        {
        }
        
        public static Vector operator -(Vector left, Vector right)
        {
            var diff = left.Value - right.Value;

            return new Vector(diff);
        }
        
        public static Vector operator +(Vector left, Vector right)
        {
            var sum = left.Value + right.Value;

            return new Vector(sum);
        }
        
        public static Vector operator -(Vector value)
        {
            var negated = -value.Value;
            return new Vector(negated);
        }

        public float Magnitude()
        {
            var sum = (X * X) +
                      (Y * Y) +
                      (Z * Z) +
                      (W * W);

            return MathF.Sqrt(sum);
        }
        
        public Vector Normalize()
        {
            var magnitude = Magnitude();
            var x = X / magnitude;
            var y = Y / magnitude;
            var z = Z / magnitude;
            
            return new Vector(x, y, z);
        }

        public float Dot(Vector other)
        {
            return (X * other.X) +
                   (Y * other.Y) +
                   (Z * other.Z) +
                   (W * other.W);
        }

        public Vector Cross(Vector other)
        {
            var x = (Y * other.Z) - (Z * other.Y);
            var y = (Z * other.X) - (X * other.Z);
            var z = (X * other.Y) - (Y * other.X);
            
            return new Vector(x, y, z);
        }

        public bool Equals(Vector other)
        {
            return Value.Equals(other.Value);
        }
        
        public override bool Equals(object obj)
        {
            return obj != null && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"Vector({X}, {Y}, {Z}, {W})";
        }
    }
}