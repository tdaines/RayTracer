using System.Collections;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Shapes
{
    public class IntersectData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(5, 0.5f, 0),  new Vector(-1, 0, 0), 4, 6 }; // +x
            yield return new object[] { new Point(-5, 0.5f, 0), new Vector(1, 0, 0),  4, 6 }; // -x
            yield return new object[] { new Point(0.5f, 5, 0),  new Vector(0, -1, 0), 4, 6 }; // +y
            yield return new object[] { new Point(0.5f, -5, 0), new Vector(0, 1, 0),  4, 6 }; // -y
            yield return new object[] { new Point(0.5f, 0, 5),  new Vector(0, 0, -1), 4, 6 }; // +z
            yield return new object[] { new Point(0.5f, 0, -5), new Vector(0, 0, 1),  4, 6 }; // -z
            yield return new object[] { new Point(0, 0.5f, 0),  new Vector(0, 0, 1), -1, 1 }; // inside
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CubeTests
    {
        [Theory]
        [ClassData(typeof(IntersectData))]
        public void Intersect(Point origin, Vector direction, float t1, float t2)
        {
            var cube = new Cube();
            var ray = new Ray(origin, direction);
            var intersections = cube.Intersect(ray);
            
            Assert.Equal(2, intersections.Count);
            Assert.Equal(t1, intersections[0].Time);
            Assert.Equal(t2, intersections[1].Time);
        }
    }
}