using Xunit;

namespace RayTracer.Lib.Test
{
    public class Matrix3x3Tests
    {
        [Fact]
        public void Constructor()
        {
            var matrix = new Matrix3x3(
                -3, 5, 0,
                1, -2, -7, 
                0, 1, 1);
            
            Assert.Equal(-3, matrix[0, 0]);
            Assert.Equal(-2, matrix[1, 1]);
            Assert.Equal(1, matrix[2, 2]);
        }
    }
}