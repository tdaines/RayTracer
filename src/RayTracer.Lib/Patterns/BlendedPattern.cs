using System;
using RayTracer.Lib.Shapes;

namespace RayTracer.Lib.Patterns
{
    public class BlendedPattern : Pattern
    {
        public Pattern PatternOne { get; }
        public Pattern PatternTwo { get; }
        
        public BlendedPattern(Matrix4x4 transform, Pattern patternOne, Pattern patternTwo) : base(transform)
        {
            PatternOne = patternOne;
            PatternTwo = patternTwo;
        }

        public BlendedPattern(Pattern patternOne, Pattern patternTwo) : this(Matrix4x4.Identity(), patternOne, patternTwo)
        {
        }

        public BlendedPattern(Matrix4x4 transform) : this(transform, SolidPattern.White, SolidPattern.Black)
        {
        }

        public BlendedPattern() : this(Matrix4x4.Identity(), SolidPattern.White, SolidPattern.Black)
        {
        }

        public override Color ColorAt(Shape shape, Point worldPoint)
        {
            var colorOne = PatternOne.ColorAt(shape, worldPoint);
            var colorTwo = PatternTwo.ColorAt(shape, worldPoint);
            
            var red = (colorOne.R + colorTwo.R) / 2.0f;
            var green = (colorOne.G + colorTwo.G) / 2.0f;
            var blue = (colorOne.B + colorTwo.B) / 2.0f;
            
            return new Color(red, green, blue);
        }

        public override Color ColorAt(Point point)
        {
            throw new NotImplementedException();
        }
    }
}