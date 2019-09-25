using System;

namespace RayTracer.Lib.Patterns
{
    public class RadialGradientPattern : Pattern
    {
        private Color InnerColor { get; }
        private Color OuterColor { get; }
        
        public RadialGradientPattern(Matrix4x4 transform, Color innerColor, Color outerColor) : base(transform)
        {
            InnerColor = innerColor;
            OuterColor = outerColor;
        }

        public RadialGradientPattern(Color innerColor, Color outerColor) : this(Matrix4x4.Identity(), innerColor, outerColor)
        {
        }

        public RadialGradientPattern(Matrix4x4 transform) : this(transform, Color.White, Color.Black)
        {
        }

        public RadialGradientPattern() : this(Matrix4x4.Identity(), Color.White, Color.Black)
        {
        }

        public override Color ColorAt(Point point)
        {
            var xSquared = MathF.Pow(point.X, 2);
            var zSquared = MathF.Pow(point.Z, 2);
            var magnitude = MathF.Sqrt(xSquared + zSquared);

            var distance = OuterColor - InnerColor;
            var fraction = magnitude - MathF.Floor(magnitude);
            if (magnitude.ApproximatelyEquals(1))
            {
                fraction = 1;
            }

            return InnerColor + distance * fraction;
        }
    }
}