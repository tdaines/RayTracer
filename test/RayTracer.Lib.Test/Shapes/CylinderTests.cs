using System.Collections;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Lib.Test.Shapes
{
    public class CylinderIntersectMissData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(1, 0, 0),  new Vector(0, 1, 0) };
            yield return new object[] { new Point(0, 0, 0),  new Vector(0, 1, 0) };
            yield return new object[] { new Point(0, 0, -5), new Vector(1, 1, 1) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CylinderIntersectHitData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(1,    0, -5), new Vector(0,    0, 1), 5,         5 };
            yield return new object[] { new Point(0,    0, -5), new Vector(0,    0, 1), 4,         6 };
            yield return new object[] { new Point(0.5f, 0, -5), new Vector(0.1f, 1, 1), 6.808006f, 7.0886984f };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CylinderNormalData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(1, 0, 0),  new Vector(1, 0, 0) };
            yield return new object[] { new Point(0, 5, -1), new Vector(0, 0, -1) };
            yield return new object[] { new Point(0, -2, 1), new Vector(0, 0, 1) };
            yield return new object[] { new Point(-1, 1, 0), new Vector(-1, 0, 0) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CylinderIntersectTruncatedData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0, 1.5f, 0),  new Vector(0.1f, 1, 0), 0 }; // from inside, diagonally out
            yield return new object[] { new Point(0, 3, -5),    new Vector(0, 0, 1),    0 }; // above
            yield return new object[] { new Point(0, 0, -5),    new Vector(0, 0, 1),    0 }; // below
            yield return new object[] { new Point(0, 2, -5),    new Vector(0, 0, 1),    0 }; // just barely above
            yield return new object[] { new Point(0, 1, -5),    new Vector(0, 0, 1),    0 }; // just barely below
            yield return new object[] { new Point(0, 1.5f, -2), new Vector(0, 0, 1),    2 }; // hit
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CylinderIntersectTruncatedAndCappedData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0,  3,  0), new Vector(0, -1, 0), 2 };
            yield return new object[] { new Point(0,  3, -2), new Vector(0, -1, 2), 2 };
            yield return new object[] { new Point(0,  4, -2), new Vector(0, -1, 1), 2 };
            yield return new object[] { new Point(0,  0, -2), new Vector(0,  1, 2), 2 };
            yield return new object[] { new Point(0, -1, -2), new Vector(0,  1, 1), 2 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CappedCylinderNormalData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Point(0,    1, 0),    new Vector(0, -1, 0) };
            yield return new object[] { new Point(0.5f, 1, 0),    new Vector(0, -1, 0) };
            yield return new object[] { new Point(0,    1, 0.5f), new Vector(0, -1, 0) };
            yield return new object[] { new Point(0,    2, 0),    new Vector(0,  1, 0) };
            yield return new object[] { new Point(0.5f, 2, 0),    new Vector(0,  1, 0) };
            yield return new object[] { new Point(0,    2, 0.5f), new Vector(0,  1, 0) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class CylinderTests
    {
        [Theory]
        [ClassData(typeof(CylinderIntersectMissData))]
        public void IntersectMiss(Point origin, Vector direction)
        {
            var cylinder = new Cylinder();
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);

            var intersections = cylinder.Intersect(ray);
            Assert.Equal(0, intersections.Count);            
        }
        
        [Theory]
        [ClassData(typeof(CylinderIntersectHitData))]
        public void IntersectHit(Point origin, Vector direction, float t0, float t1)
        {
            var cylinder = new Cylinder();
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);

            var intersections = cylinder.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(t0, intersections[0].Time);
            Assert.Equal(t1, intersections[1].Time);
        }

        [Theory]
        [ClassData(typeof(CylinderNormalData))]
        public void Normal(Point point, Vector normal)
        {
            var cylinder = new Cylinder();

            var actual = cylinder.Normal(point);
            Assert.Equal(normal, actual);
        }

        [Fact]
        public void Default()
        {
            var cylinder = new Cylinder();
            
            Assert.Equal(float.NegativeInfinity, cylinder.Min);
            Assert.Equal(float.PositiveInfinity, cylinder.Max);
            Assert.False(cylinder.Capped);
        }

        [Theory]
        [ClassData(typeof(CylinderIntersectTruncatedData))]
        public void IntersectTruncated(Point origin, Vector direction, int count)
        {
            var cylinder = new Cylinder { Min = 1, Max = 2 };
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);
            
            var intersections = cylinder.Intersect(ray);
            Assert.Equal(count, intersections.Count);
        }
        
        [Theory]
        [ClassData(typeof(CylinderIntersectTruncatedAndCappedData))]
        public void IntersectTruncatedAndCapped(Point origin, Vector direction, int count)
        {
            var cylinder = new Cylinder { Min = 1, Max = 2, Capped = true };
            direction = Vector.Normalize(direction);
            var ray = new Ray(origin, direction);
            
            var intersections = cylinder.Intersect(ray);
            Assert.Equal(count, intersections.Count);
        }
        
        [Theory]
        [ClassData(typeof(CappedCylinderNormalData))]
        public void NormalCapped(Point point, Vector normal)
        {
            var cylinder = new Cylinder { Min = 1, Max = 2, Capped = true };

            var actual = cylinder.Normal(point);
            Assert.Equal(normal, actual);
        }
    }
}