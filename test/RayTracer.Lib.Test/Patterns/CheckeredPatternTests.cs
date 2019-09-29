using RayTracer.Lib.Patterns;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Patterns
{
    public class CheckeredPatternTests
    {
        [Fact]
        public void ColorAtRepeatXDirection()
        {
            var pattern = new CheckeredPattern(SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), Point.Zero));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0.99f, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(1.01f, 0, 0)));
        }
        
        [Fact]
        public void ColorAtRepeatYDirection()
        {
            var pattern = new CheckeredPattern(SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), Point.Zero));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0.99f, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(0, 1.01f, 0)));
        }
        
        [Fact]
        public void ColorAtRepeatZDirection()
        {
            var pattern = new CheckeredPattern(SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), Point.Zero));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 0.99f)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(0, 0, 1.01f)));
        }
    }
}