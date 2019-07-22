using Xunit;

namespace RayTracer.Lib.Test
{
    public class Matrix4x4Tests
    {
        [Fact]
        public void Constructor()
        {
            var matrix = new Matrix4x4(1,     2,     3,     4,
                                       5.5f,  6.5f,  7.5f,  8.5f,
                                       9,     10,    11,    12,
                                       13.5f, 14.5f, 15.5f, 16.5f);
            
            Assert.Equal(1, matrix[0, 0]);
            Assert.Equal(4, matrix[0, 3]);
            Assert.Equal(5.5f, matrix[1, 0]);
            Assert.Equal(7.5f, matrix[1, 2]);
            Assert.Equal(11, matrix[2, 2]);
            Assert.Equal(13.5f, matrix[3, 0]);
            Assert.Equal(15.5f, matrix[3, 2]);
        }

        [Fact]
        public void Equality()
        {
            var left = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            var right = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            
            Assert.True(left == right);
        }
        
        [Fact]
        public void Inequality()
        {
            var left = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            
            var right = new Matrix4x4(
                2, 3, 4, 5,
                6, 7, 8, 9,
                8, 7, 6, 5,
                4, 3, 2, 1);
            
            Assert.True(left != right);
        }

        [Fact]
        public void MultiplyMatrixByMatrix()
        {
            var left = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            
            var right = new Matrix4x4(
                -2, 1, 2, 3,
                3, 2, 1, -1,
                4, 3, 6, 5,
                1, 2, 7, 8);
            
            var expected = new Matrix4x4(
                20, 22, 50, 48,
                44, 54, 114, 108,
                40, 58, 110, 102,
                16, 26, 46, 42);
            
            Assert.Equal(expected, left * right);
        }

        [Fact]
        public void MultiplyMatrixByVector()
        {
            var matrix = new Matrix4x4(
                1, 2, 3, 4,
                2, 4, 4, 2,
                8, 6, 4, 1,
                0, 0, 0, 1);
            
            var vector = new Vector(1, 2, 3, 1);
            
            Assert.Equal(new Vector(18, 24, 33, 1), matrix * vector);
        }
    }
}