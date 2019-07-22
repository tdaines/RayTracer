using System;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class VectorTests
    {
        [Fact]
        public void AddVectorAndVector()
        {
            var left = new Vector(3, -2, 5);
            var right = new Vector(-2, 3, 1);

            var sum = left + right;
            Assert.Equal(new Vector(1, 1, 6), sum);
        }
        
        [Fact]
        public void SubtractVectorFromVector()
        {
            var left = new Vector(3, 2, 1);
            var right = new Vector(5, 6, 7);

            var diff = left - right;
            Assert.Equal(new Vector(-2, -4, -6), diff);
        }
        
        [Fact]
        public void MultiplyTupleByScalar()
        {
            var value = new Vector(1, -2, 3);
            var scalar = 3.5f;
            
            Assert.Equal(new Vector(3.5f, -7f, 10.5f), value * scalar);
            
            scalar = 0.5f;
            
            Assert.Equal(new Vector(0.5f, -1, 1.5f), scalar * value);
        }
        
        [Fact]
        public void Length()
        {
            var value = new Vector(1, 0, 0);
            Assert.Equal(1, value.Length());
            
            value = new Vector(0, 1, 0);
            Assert.Equal(1, value.Length());
            
            value = new Vector(0, 0, 1);
            Assert.Equal(1, value.Length());
            
            value = new Vector(1, 2, 3);
            Assert.Equal(MathF.Sqrt(14), value.Length());
            
            value = new Vector(-1, -2, -3);
            Assert.Equal(MathF.Sqrt(14), value.Length());
        }

        [Fact]
        public void Normalize()
        {
            var value = new Vector(4, 0, 0);
            Assert.Equal(new Vector(1, 0, 0), Vector.Normalize(value));
            
            value = new Vector(1, 2, 3);
            Assert.Equal(new Vector(0.2672612f, 0.5345225f, 0.8017837f), Vector.Normalize(value));
        }

        [Fact]
        public void DotProduct()
        {
            var left = new Vector(1, 2, 3);
            var right = new Vector(2, 3, 4);
            
            Assert.Equal(20, Vector.Dot(left, right));
        }

        [Fact]
        public void CrossProduct()
        {
            var left = new Vector(1, 2, 3);
            var right = new Vector(2, 3, 4);
            
            Assert.Equal(new Vector(-1, 2, -1), Vector.Cross(left, right));
            Assert.Equal(new Vector(1, -2, 1), Vector.Cross(right, left));
        }
    }
}