using RayTracer.Lib.Patterns;
using Xunit;

namespace RayTracer.Lib.Test.Patterns
{
    public class PatternTests
    {
        [Fact]
        public void ColorAtShapeShapeTransformation()
        {
            var sphere = new Sphere(Matrix4x4.Scaling(2, 2, 2));
            var pattern = new StripePattern(SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(sphere, new Point(1.5f, 0, 0)));
        }
        
        [Fact]
        public void ColorAtShapePatternTransformation()
        {
            var sphere = new Sphere();
            var pattern = new StripePattern(Matrix4x4.Scaling(2, 2, 2), SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(sphere, new Point(1.5f, 0, 0)));
        }
        
        [Fact]
        public void ColorAtShapePatternAndShapeTransformation()
        {
            var sphere = new Sphere(Matrix4x4.Scaling(2, 2, 2));
            var pattern = new StripePattern(Matrix4x4.Translation(0.5f, 0, 0), SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(sphere, new Point(2.5f, 0, 0)));
        }
    }
}