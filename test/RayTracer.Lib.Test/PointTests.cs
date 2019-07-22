using Xunit;

namespace RayTracer.Lib.Test
{
    public class PointTests
    {
        [Fact]
        public void AddPointAndVector()
        {
            var left = new Point(3, -2, 5);
            var right = new Vector(-2, 3, 1);

            var sum = left + right;
            Assert.Equal(new Point(1, 1, 6), sum);
        }

        [Fact]
        public void SubtractPointFromPoint()
        {
            var left = new Point(3, 2, 1);
            var right = new Point(5, 6, 7);

            var diff = left - right;
            Assert.Equal(new Vector(-2, -4, -6), diff);
        }
        
        [Fact]
        public void SubtractVectorFromPoint()
        {
            var left = new Point(3, 2, 1);
            var right = new Vector(5, 6, 7);

            var diff = left - right;
            Assert.Equal(new Point(-2, -4, -6), diff);
        }
    }
}