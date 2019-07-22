using System;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class TupleTests
    {
        [Fact]
        public void AddTupleAndTuple()
        {
            var left = new Tuple(3, -2, 5, 1);
            var right = new Tuple(-2, 3, 1, 0);

            var sum = left + right;
            Assert.Equal(new Tuple(1, 1, 6, 1), sum);
        }

        [Fact]
        public void NegateTuple()
        {
            var value = new Tuple(1, -2, 3, -4);
            Assert.Equal(new Tuple(-1, 2, -3, 4), -value);
        }

        [Fact]
        public void MultiplyTupleByScalar()
        {
            var value = new Tuple(1, -2, 3, -4);
            var scalar = 3.5f;
            
            Assert.Equal(new Tuple(3.5f, -7f, 10.5f, -14f), value * scalar);
            
            scalar = 0.5f;
            
            Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), scalar * value);
        }
        
        [Fact]
        public void DivideTupleByScalar()
        {
            var value = new Tuple(1, -2, 3, -4);
            var scalar = 2;
            
            Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), value / scalar);
        }
    }
}