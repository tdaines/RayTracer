using System;

namespace RayTracer.Lib
{
    public struct Vector : IEquatable<Vector>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        public Vector(float x, float y, float z, float w = 0)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector operator -(Vector left, Vector right)
        {
            var x = left.X - right.X;
            var y = left.Y - right.Y;
            var z = left.Z - right.Z;
            var w = left.W - right.W;
            
            return new Vector(x, y, z, w);
        }
        
        public static Vector operator +(Vector left, Vector right)
        {
            var x = left.X + right.X;
            var y = left.Y + right.Y;
            var z = left.Z + right.Z;
            var w = left.W + right.W;
            
            return new Vector(x, y, z, w);
        }
        
        public static Vector operator -(Vector value)
        {
            return new Vector(-value.X, -value.Y, -value.Z, -value.W);
        }
        
        public static Vector operator *(Vector value, float scalar)
        {
            var x = value.X * scalar;
            var y = value.Y * scalar;
            var z = value.Z * scalar;
            var w = value.W * scalar;
            
            return new Vector(x, y, z, w);
        }
        
        public static Vector operator *(float scalar, Vector value)
        {
            return value * scalar;
        }

        public float Length()
        {
            var sum = (X * X) +
                      (Y * Y) +
                      (Z * Z) +
                      (W * W);

            return MathF.Sqrt(sum);
        }

        public static Vector Normalize(Vector vector)
        {
            var length = vector.Length();
            if (length.ApproximatelyEquals(0))
            {
                return new Vector(0, 0, 0);
            }
            
            var x = vector.X / length;
            var y = vector.Y / length;
            var z = vector.Z / length;
            var w = vector.W / length;
            
            return new Vector(x, y, z, w);
        }

        public static float Dot(Vector left, Vector right)
        {
            return (left.X * right.X) +
                   (left.Y * right.Y) +
                   (left.Z * right.Z) +
                   (left.W * right.W);
        }

        public static Vector Cross(Vector left, Vector right)
        {
            var x = (left.Y * right.Z) - (left.Z * right.Y);
            var y = (left.Z * right.X) - (left.X * right.Z);
            var z = (left.X * right.Y) - (left.Y * right.X);
            
            return new Vector(x, y, z);
        }

        public static Vector Reflect(Vector v, Vector normal)
        {
            return v - (normal * 2 * Vector.Dot(v, normal));
        }

        public bool Equals(Vector other)
        {
            return X.ApproximatelyEquals(other.X) 
                && Y.ApproximatelyEquals(other.Y)
                && Z.ApproximatelyEquals(other.Z)
                && W.ApproximatelyEquals(other.W);
        }
        
        public override bool Equals(object obj)
        {
            return (obj != null) 
                && (obj is Vector vector)
                && Equals(vector);
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
            return $"Vector({X}, {Y}, {Z})";
        }
        
        public static Vector UnitX = new Vector(1, 0, 0);
        public static Vector UnitY = new Vector(0, 1, 0);
        public static Vector UnitZ = new Vector(0, 0, 1);
    }
}