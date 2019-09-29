using System;
using RayTracer.Lib.Shapes;

namespace RayTracer.Lib.Patterns
{
    public abstract class AlternatingPattern : Pattern
    {
        public Pattern[] Patterns { get; }
        
        protected AlternatingPattern(Matrix4x4 transform, params Pattern[] patterns) : base(transform)
        {
            Patterns = patterns;
        }

        protected abstract float Distance(Point patternPoint);

        public override Color ColorAt(Shape shape, Point worldPoint)
        {
            var patternPoint = ConvertToPatternPoint(shape, worldPoint);
            var distance = (int) MathF.Abs(MathF.Floor(Distance(patternPoint)));
            
            int index = distance % Patterns.Length;
            return Patterns[index].ColorAt(shape, worldPoint);
        }

        public override Color ColorAt(Point point)
        {
            throw new NotImplementedException();
        }
    }
}