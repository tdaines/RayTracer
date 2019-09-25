namespace RayTracer.Lib.Patterns
{
    public class StripePattern : AlternatingPattern
    {
        public StripePattern(Matrix4x4 transform, params Pattern[] patterns) : base(transform, patterns)
        {
        }

        public StripePattern(params Pattern[] patterns) : this(Matrix4x4.Identity(), patterns)
        {
        }

        public StripePattern(Matrix4x4 transform) : this(transform, SolidPattern.White, SolidPattern.Black)
        {
        }

        public StripePattern() : this(Matrix4x4.Identity(), SolidPattern.White, SolidPattern.Black)
        {
        }

        protected override float Distance(Point patternPoint)
        {
            return patternPoint.X;
        }
    }
}