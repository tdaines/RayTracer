using System;

namespace RayTracer.Lib.Patterns
{
    public class RingPattern : AlternatingPattern
    {
        public RingPattern(Matrix4x4 transform, params Pattern[] patterns) : base(transform, patterns)
        {
        }

        public RingPattern(params Pattern[] patterns) : this(Matrix4x4.Identity(), patterns)
        {
        }

        public RingPattern(Matrix4x4 transform) : this(transform, SolidPattern.White, SolidPattern.Black)
        {
        }

        public RingPattern() : this(Matrix4x4.Identity(), SolidPattern.White, SolidPattern.Black)
        {
        }

        protected override float Distance(Point patternPoint)
        {
            var xSquared = MathF.Pow(patternPoint.X, 2);
            var zSquared = MathF.Pow(patternPoint.Z, 2);
            return MathF.Sqrt(xSquared + zSquared);
        }
    }
}