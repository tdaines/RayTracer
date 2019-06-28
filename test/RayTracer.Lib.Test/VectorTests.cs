using System;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class VectorTests
    {
        [Fact]
        public void Magnitude()
        {
            var value = new Vector(1, 0, 0);
            Assert.Equal(1, value.Magnitude());
            
            value = new Vector(0, 1, 0);
            Assert.Equal(1, value.Magnitude());
            
            value = new Vector(0, 0, 1);
            Assert.Equal(1, value.Magnitude());
            
            value = new Vector(1, 2, 3);
            Assert.Equal(MathF.Sqrt(14), value.Magnitude());
            
            value = new Vector(-1, -2, -3);
            Assert.Equal(MathF.Sqrt(14), value.Magnitude());
        }

        [Fact]
        public void Normalize()
        {
            var value = new Vector(4, 0, 0);
            Assert.Equal(new Vector(1, 0, 0), value.Normalize());
            
            value = new Vector(1, 2, 3);
            Assert.Equal(new Vector(0.2672612f, 0.5345225f, 0.8017837f), value.Normalize());
        }

        [Fact]
        public void DotProduct()
        {
            var left = new Vector(1, 2, 3);
            var right = new Vector(2, 3, 4);
            
            Assert.Equal(20, left.Dot(right));
        }

        [Fact]
        public void CrossProduct()
        {
            var left = new Vector(1, 2, 3);
            var right = new Vector(2, 3, 4);
            
            Assert.Equal(new Vector(-1, 2, -1), left.Cross(right));
            Assert.Equal(new Vector(1, -2, 1), right.Cross(left));
        }
    }
}