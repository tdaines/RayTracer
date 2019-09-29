using RayTracer.Lib.Shapes;

namespace RayTracer.Lib.Patterns
{
    public abstract class Pattern
    {
        public Matrix4x4 Transform { get; }
        private Matrix4x4 InverseTransform { get; }

        protected Pattern(Matrix4x4 transform)
        {
            Transform = transform;
            InverseTransform = Matrix4x4.Inverse(Transform);
        }

        protected Point ConvertToPatternPoint(Shape shape, Point worldPoint)
        {
            var objectPoint = shape.InverseTransform * worldPoint;
            var patternPoint = InverseTransform * objectPoint;

            return patternPoint;
        }

        public virtual Color ColorAt(Shape shape, Point worldPoint)
        {
            var patternPoint = ConvertToPatternPoint(shape, worldPoint);
            return ColorAt(patternPoint);
        }
        
        public abstract Color ColorAt(Point point);
    }
}