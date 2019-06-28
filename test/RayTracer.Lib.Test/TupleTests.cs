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
        public void AddPointAndVector()
        {
            var left = new Point(3, -2, 5);
            var right = new Vector(-2, 3, 1);

            var sum = left + right;
            Assert.Equal(new Point(1, 1, 6), sum);
        }
        
        [Fact]
        public void AddVectorAndVector()
        {
            var left = new Vector(3, -2, 5);
            var right = new Vector(-2, 3, 1);

            var sum = left + right;
            Assert.Equal(new Vector(1, 1, 6), sum);
        }

        [Fact]
        public void SubtractPointFromPoint()
        {
            var left = new Point(3, 2, 1);
            var right = new Point(5, 6, 7);

            var diff = left - right;
            Assert.Equal(new Vector(-2, -4, -6), diff);
        }
        
        [Fact]
        public void SubtractVectorFromPoint()
        {
            var left = new Point(3, 2, 1);
            var right = new Vector(5, 6, 7);

            var diff = left - right;
            Assert.Equal(new Point(-2, -4, -6), diff);
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