using RayTracer.Lib.Patterns;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Patterns
{
    public class StripePatternTests
    {
        [Fact]
        public void Constructor()
        {
            var pattern = new StripePattern();
            
            Assert.Equal(2, pattern.Patterns.Length);
            Assert.Equal(Color.White, ((SolidPattern)pattern.Patterns[0]).Color);
            Assert.Equal(Color.Black, ((SolidPattern)pattern.Patterns[1]).Color);
        }

        [Fact]
        public void ColorAtYDirection()
        {
            var pattern = new StripePattern();
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 1, 0)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 2, 0)));
        }
        
        [Fact]
        public void ColorAtZDirection()
        {
            var pattern = new StripePattern();
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 1)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 2)));
        }
        
        [Fact]
        public void ColorAtXDirection()
        {
            var pattern = new StripePattern();
            
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(0.9f, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(1, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(-0.1f, 0, 0)));
            Assert.Equal(Color.Black, pattern.ColorAt(new Sphere(), new Point(-1, 0, 0)));
            Assert.Equal(Color.White, pattern.ColorAt(new Sphere(), new Point(-1.1f, 0, 0)));
        }
    }
}