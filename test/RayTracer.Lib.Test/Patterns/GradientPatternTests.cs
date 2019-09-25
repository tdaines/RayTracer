using RayTracer.Lib.Patterns;
using Xunit;

namespace RayTracer.Lib.Test.Patterns
{
    public class GradientPatternTests
    {
        [Fact]
        public void ColorAt()
        {
            var pattern = new GradientPattern(Color.White, Color.Black);
            
            Assert.Equal(Color.White, pattern.ColorAt(Point.Zero));
            Assert.Equal(new Color(0.75f, 0.75f, 0.75f), pattern.ColorAt(new Point(0.25f, 0, 0)));
            Assert.Equal(new Color(0.5f, 0.5f, 0.5f), pattern.ColorAt(new Point(0.5f, 0, 0)));
            Assert.Equal(new Color(0.25f, 0.25f, 0.25f), pattern.ColorAt(new Point(0.75f, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Point(1, 0, 0)));
        }
    }
}