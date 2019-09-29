using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class IntersectionsTests
    {
        [Fact]
        public void Hit()
        {
            var sphere = new Sphere();
            var intersection1 = new Intersection(1, sphere);
            var intersection2 = new Intersection(2, sphere);
            
            var intersections = new Intersections(intersection1, intersection2);
            
            Assert.Equal(intersection1, intersections.Hit());
            
            intersection1 = new Intersection(-1, sphere);
            intersection2 = new Intersection(1, sphere);
            
            intersections = new Intersections(intersection1, intersection2);
            
            Assert.Equal(intersection2, intersections.Hit());
            
            intersection1 = new Intersection(-2, sphere);
            intersection2 = new Intersection(-1, sphere);
            
            intersections = new Intersections(intersection1, intersection2);
            
            Assert.Null(intersections.Hit());
            
            intersection1 = new Intersection(5, sphere);
            intersection2 = new Intersection(7, sphere);
            var intersection3 = new Intersection(-3, sphere);
            var intersection4 = new Intersection(2, sphere);
            
            intersections = new Intersections(intersection1, intersection2, intersection3, intersection4);
            
            Assert.Equal(intersection4, intersections.Hit());
        }
    }
}