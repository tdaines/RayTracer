using Xunit;

namespace RayTracer.Lib.Test
{
    public class RayTests
    {
        [Fact]
        public void Constructor()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            
            var ray = new Ray(origin, direction);
            
            Assert.Equal(origin, ray.Origin);
            Assert.Equal(direction, ray.Direction);
        }
        
        [Fact]
        public void Position()
        {
            var ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
            
            Assert.Equal(new Point(2, 3, 4), ray.Position(0));
            Assert.Equal(new Point(3, 3, 4), ray.Position(1));
            Assert.Equal(new Point(1, 3, 4), ray.Position(-1));
            Assert.Equal(new Point(4.5f, 3, 4), ray.Position(2.5f));
        }
    }
}