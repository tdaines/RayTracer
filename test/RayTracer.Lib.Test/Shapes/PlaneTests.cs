using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Shapes
{
    public class PlaneTests
    {
        [Fact]
        public void Normal()
        {
            var plane = new Plane();
            
            Assert.Equal(new Vector(0, 1, 0), plane.Normal(new Point(0, 0, 0)));
            Assert.Equal(new Vector(0, 1, 0), plane.Normal(new Point(10, 0, -10)));
            Assert.Equal(new Vector(0, 1, 0), plane.Normal(new Point(-5, 0, 150)));
        }

        [Fact]
        public void IntersectParallel()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));

            var intersections = plane.Intersect(ray);
            Assert.Empty(intersections);
        }
        
        [Fact]
        public void IntersectCoplanar()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            var intersections = plane.Intersect(ray);
            Assert.Empty(intersections);
        }
        
        [Fact]
        public void IntersectFromAbove()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));

            var intersections = plane.Intersect(ray);
            Assert.Equal(1, intersections.Count);
            Assert.Equal(1, intersections[0].Time);
            Assert.Equal(plane, intersections[0].Shape);
        }
        
        [Fact]
        public void IntersectFromBelow()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));

            var intersections = plane.Intersect(ray);
            Assert.Equal(1, intersections.Count);
            Assert.Equal(1, intersections[0].Time);
            Assert.Equal(plane, intersections[0].Shape);
        }
    }
}