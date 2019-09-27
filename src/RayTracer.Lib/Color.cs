using System;

namespace RayTracer.Lib
{
    public struct Color : IEquatable<Color>
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;

        public Color(float red, float green, float blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        public static Color operator +(Color left, Color right)
        {
            var r = left.R + right.R;
            var g = left.G + right.G;
            var b = left.B + right.B;
            
            return new Color(r, g, b);
        }
        
        public static Color operator -(Color left, Color right)
        {
            var r = left.R - right.R;
            var g = left.G - right.G;
            var b = left.B - right.B;
            
            return new Color(r, g, b);
        }

        public static Color operator *(Color left, Color right)
        {
            var r = left.R * right.R;
            var g = left.G * right.G;
            var b = left.B * right.B;

            return new Color(r, g, b);
        }

        public static Color operator *(Color value, float scalar)
        {
            var r = value.R * scalar;
            var g = value.G * scalar;
            var b = value.B * scalar;

            return new Color(r, g, b);
        }
        
        public static Color operator *(float scalar, Color value)
        {
            return value * scalar;
        }
        
        public bool Equals(Color other)
        {
            return R.ApproximatelyEquals(other.R) 
                && G.ApproximatelyEquals(other.G)
                && B.ApproximatelyEquals(other.B);
        }

        public override bool Equals(object obj)
        {
            return (obj != null) 
                && (obj is Color color)
                && Equals(color);
        }

        public override int GetHashCode()
        {
            return R.GetHashCode()
                 ^ G.GetHashCode()
                 ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return $"Color({R}, {G}, {B})";
        }
        
        public static Color Black = new Color(0, 0, 0);
        public static Color Red = new Color(1, 0, 0);
        public static Color Green = new Color(0, 1, 0);
        public static Color Blue = new Color(0, 0, 1);
        public static Color White = new Color(1, 1, 1);
        public static Color Yellow = new Color(1, 1, 0);
        public static Color Cyan = new Color(0, 1, 1);
        public static Color Purple = new Color(1, 0, 1);
    }
}