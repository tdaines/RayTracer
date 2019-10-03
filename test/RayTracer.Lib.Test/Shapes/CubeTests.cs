using System.Collections;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Shapes
{
    public class IntersectHitData : IEnumerable<object[]>
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
    
    public class IntersectMissData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(-2, 0, 0), new Vector(0.2673f, 0.5345f, 0.8018f) };
            yield return new object[] { new Point(0, -2, 0), new Vector(0.8018f, 0.2673f, 0.5345f) };
            yield return new object[] { new Point(0, 0, -2), new Vector(0.5345f, 0.8018f, 0.2673f) };
            yield return new object[] { new Point(2, 0, 2),  new Vector(0, 0, -1) };
            yield return new object[] { new Point(0, 2, 2),  new Vector(0, -1, 0) };
            yield return new object[] { new Point(2, 2, 0),  new Vector(-1, 0, 0) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class NormalData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(1, 0.5f, -0.8f),  new Vector(1, 0, 0) };
            yield return new object[] { new Point(-1, -0.2f, 0.9f), new Vector(-1, 0, 0) };
            yield return new object[] { new Point(-0.4f, 1, -0.1f), new Vector(0, 1, 0) };
            yield return new object[] { new Point(0.3f, -1, -0.7f), new Vector(0, -1, 0) };
            yield return new object[] { new Point(-0.6f, 0.3f, 1),  new Vector(0, 0, 1) };
            yield return new object[] { new Point(0.4f, 0.4f, -1),  new Vector(0, 0, -1) };
            yield return new object[] { new Point(1, 1, 1),         new Vector(1, 0, 0) };
            yield return new object[] { new Point(-1, -1, -1),      new Vector(-1, 0, 0) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CubeTests
    {
        [Theory]
        [ClassData(typeof(IntersectHitData))]
        public void IntersectHit(Point origin, Vector direction, float t1, float t2)
        {
            var cube = new Cube();
            var ray = new Ray(origin, direction);
            var intersections = cube.Intersect(ray);
            
            Assert.Equal(2, intersections.Count);
            Assert.Equal(t1, intersections[0].Time);
            Assert.Equal(t2, intersections[1].Time);
        }
        
        [Theory]
        [ClassData(typeof(IntersectMissData))]
        public void IntersectMiss(Point origin, Vector direction)
        {
            var cube = new Cube();
            var ray = new Ray(origin, direction);
            var intersections = cube.Intersect(ray);
            
            Assert.Equal(0, intersections.Count);
        }
        
        [Theory]
        [ClassData(typeof(NormalData))]
        public void Normal(Point origin, Vector normal)
        {
            var cube = new Cube();
            var actual = cube.Normal(origin);
            
            Assert.Equal(normal, actual);
        }
    }
}