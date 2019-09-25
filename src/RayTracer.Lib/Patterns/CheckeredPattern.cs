using System;

namespace RayTracer.Lib.Patterns
{
    public class CheckeredPattern : AlternatingPattern
    {
        public CheckeredPattern(Matrix4x4 transform, Pattern patternOne, Pattern patternTwo) : base(transform, patternOne, patternTwo)
        {
        }

        public CheckeredPattern(Pattern patternOne, Pattern patternTwo) : this(Matrix4x4.Identity(), patternOne, patternTwo)
        {
        }

        public CheckeredPattern(Matrix4x4 transform) : this(transform, SolidPattern.White, SolidPattern.Black)
        {
        }

        public CheckeredPattern() : this(Matrix4x4.Identity(), SolidPattern.White, SolidPattern.Black)
        {
        }

        protected override float Distance(Point patternPoint)
        {
            return MathF.Floor(patternPoint.X)
                 + MathF.Floor(patternPoint.Y)
                 + MathF.Floor(patternPoint.Z);
        }
    }
}