using System;
using System.Collections;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Shapes
{
    public class ConeIntersectHitData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0, 0, -5), new Vector(0,      0, 1), 5,          5 };
            yield return new object[] { new Point(0, 0, -5), new Vector(1,      1, 1), 8.6602545f, 8.6602545f };
            yield return new object[] { new Point(1, 1, -5), new Vector(-0.5f, -1, 1), 4.550057f, 49.44995f };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ConeNormalData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0,   0, 0), new Vector(0,  0,             0) };
            yield return new object[] { new Point(1,   1, 1), new Vector(1, -MathF.Sqrt(2), 1) };
            yield return new object[] { new Point(-1, -1, 0), new Vector(-1, 1,             0) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ConeIntersectCappedData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0, 0, -5),     new Vector(0, 1, 0), 0 };
            yield return new object[] { new Point(0, 0, -0.25f), new Vector(0, 1, 1), 2 };
            yield return new object[] { new Point(0, 0, -0.25f), new Vector(0, 1, 0), 4 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ConeTests
    {
        [Theory]
        [ClassData(typeof(ConeIntersectHitData))]
        public void Intersect(Point origin, Vector direction, float t0, float t1)
        {
            var cone = new Cone();
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);
            
            var intersections = cone.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.True(t0.ApproximatelyEquals(intersections[0].Time));
            Assert.True(t1.ApproximatelyEquals(intersections[1].Time));
        }

        [Fact]
        public void IntersectRayParallelToOneOfConesHalves()
        {
            var cone = new Cone();
            var direction = Vector.Normalize(new Vector(0, 1, 1));
            var ray = new Ray(new Point(0, 0, -1), direction);
            
            var intersections = cone.Intersect(ray);
            Assert.Equal(1, intersections.Count);
            Assert.Equal(0.35355338f, intersections[0].Time);
        }
        
        [Theory]
        [ClassData(typeof(ConeIntersectCappedData))]
        public void IntersectCapped(Point origin, Vector direction, int count)
        {
            var cone = new Cone { Min = -0.5f, Max = 0.5f, Capped = true };
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);
            
            var intersections = cone.Intersect(ray);
            Assert.Equal(count, intersections.Count);
        }
        
        [Theory]
        [ClassData(typeof(ConeNormalData))]
        public void Normal(Point point, Vector normal)
        {
            var cone = new Cone();

            var actual = cone.Normal(point);
            Assert.Equal(Vector.Normalize(normal), actual);
        }
    }
}