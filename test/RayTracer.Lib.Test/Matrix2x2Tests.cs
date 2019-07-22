using Xunit;

namespace RayTracer.Lib.Test
{
    public class Matrix2x2Tests
    {
        [Fact]
        public void Constructor()
        {
            var matrix = new Matrix2x2(
                -3, 5,
                1, -2);
            
            Assert.Equal(-3, matrix[0, 0]);
            Assert.Equal(5, matrix[0, 1]);
            Assert.Equal(1, matrix[1, 0]);
            Assert.Equal(-2, matrix[1, 1]);
        }
    }
}