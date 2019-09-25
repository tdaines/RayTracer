using RayTracer.Lib.Patterns;
using Xunit;

namespace RayTracer.Lib.Test.Patterns
{
    public class RingPatternTests
    {
        [Fact]
        public void ColorAt()
        {
            var pattern = new RingPattern(SolidPattern.White, SolidPattern.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), Point.Zero));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(1, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(0, 0, 1)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(0.708f, 0, 0.708f)));
        }
    }
}