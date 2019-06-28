using System;

namespace RayTracer.Lib
{
    public struct Color : IEquatable<Color>
    {
        private Tuple Value { get; }
        public float R => Value.X;
        public float G => Value.Y;
        public float B => Value.Z;

        public Color(float red, float green, float blue)
        {
            Value = new Tuple(red, green, blue, 0);
        }

        public Color(Tuple tuple) : this(tuple.X, tuple.Y, tuple.Z)
        {
        }
        
        public static Color operator +(Color left, Color right)
        {
            var sum = left.Value + right.Value;
            
            return new Color(sum);
        }
        
        public static Color operator -(Color left, Color right)
        {
            var diff = left.Value - right.Value;
            
            return new Color(diff);
        }

        public static Color operator *(Color left, Color right)
        {
            var red = left.R * right.R;
            var green = left.G * right.G;
            var blue = left.B * right.B;

            return new Color(red, green, blue);
        }

        public static Color operator *(Color value, float scalar)
        {
            return new Color(value.Value * scalar);
        }
        
        public static Color operator *(float scalar, Color value)
        {
            return value * scalar;
        }
        
        public bool Equals(Color other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals((Color) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"Color({R}, {G}, {B})";
        }
        
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(1, 0, 0);
        public static Color Green => new Color(0, 1, 0);
        public static Color Blue => new Color(0, 0, 1);
        public static Color White => new Color(1, 1, 1);
        public static Color Yellow => new Color(1, 1, 0);
        public static Color Cyan => new Color(0, 1, 1);
        public static Color Purple => new Color(1, 0, 1);
    }
}