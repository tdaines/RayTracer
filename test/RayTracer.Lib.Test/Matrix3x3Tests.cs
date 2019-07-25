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

        [Fact]
        public void SubMatrix()
        {
            var matrix = new Matrix3x3(
                1, 5, 0,
                -3, 2, 7,
                0, 6, -3);
            
            var expected = new Matrix2x2(
                -3, 2,
                0, 6);
            
            Assert.Equal(expected, Matrix3x3.SubMatrix(matrix, 0, 2));
        }

        [Fact]
        public void Minor()
        {
            var matrix = new Matrix3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            var subMatrix = Matrix3x3.SubMatrix(matrix, 1, 0);

            Assert.Equal(25, Matrix2x2.Determinate(subMatrix));
            Assert.Equal(25, Matrix3x3.Minor(matrix, 1, 0));
        }
        
        [Fact]
        public void Cofactor()
        {
            var matrix = new Matrix3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            Assert.Equal(-12, Matrix3x3.Minor(matrix, 0, 0));
            Assert.Equal(-12, Matrix3x3.Cofactor(matrix, 0, 0));
            Assert.Equal(25, Matrix3x3.Minor(matrix, 1, 0));
            Assert.Equal(-25, Matrix3x3.Cofactor(matrix, 1, 0));
        }

        [Fact]
        public void Determinate()
        {
            var matrix = new Matrix3x3(
                1, 2, 6,
                -5, 8, -4,
                2, 6, 4);
            
            Assert.Equal(56, Matrix3x3.Cofactor(matrix, 0, 0));
            Assert.Equal(12, Matrix3x3.Cofactor(matrix, 0, 1));
            Assert.Equal(-46, Matrix3x3.Cofactor(matrix, 0, 2));
            Assert.Equal(-196, Matrix3x3.Determinate(matrix));
        }
    }
}