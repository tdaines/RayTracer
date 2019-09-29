using RayTracer.Lib.Patterns;

namespace RayTracer.Lib.Test.Patterns
{
    public class TestPattern : Pattern
    {
        public TestPattern() : base(Matrix4x4.Identity())
        {
        }

        public TestPattern(Matrix4x4 transform) : base(transform)
        {
        }

        public override Color ColorAt(Point point)
        {
            return new Color(point.X, point.Y, point.Z);
        }
    }
}