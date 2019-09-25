using System;

namespace RayTracer.Lib.Patterns
{
    public class GradientPattern : Pattern
    {
        private Color ColorLeft { get; }
        private Color ColorRight { get; }
        
        public GradientPattern(Matrix4x4 transform, Color colorLeft, Color colorRight) : base(transform)
        {
            ColorLeft = colorLeft;
            ColorRight = colorRight;
        }

        public GradientPattern(Color colorLeft, Color colorRight) : this(Matrix4x4.Identity(), colorLeft, colorRight)
        {
        }

        public GradientPattern(Matrix4x4 transform) : this(transform, Color.White, Color.Black)
        {
        }

        public GradientPattern() : this(Matrix4x4.Identity(), Color.White, Color.Black)
        {
        }

        public override Color ColorAt(Point point)
        {
            var distance = ColorRight - ColorLeft;
            var fraction = point.X - MathF.Floor(point.X);
            if (point.X.ApproximatelyEquals(1))
            {
                fraction = 1;
            }

            return ColorLeft + distance * fraction;
        }
    }
}