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

        [Fact]
        public void MultiplyMatrixByIdentity()
        {
            var matrix = new Matrix4x4(
                0, 1, 2, 4,
                1, 2, 4, 8,
                2, 4, 8, 16,
                4, 8, 16, 32);
            
            Assert.Equal(matrix, matrix * Matrix4x4.Identity());
        }

        [Fact]
        public void MultiplyIdentityMatrixByVector()
        {
            var vector = new Vector(1, 2, 3, 4);
            
            Assert.Equal(vector, vector * Matrix4x4.Identity());
        }

        [Fact]
        public void TransposeMatrix()
        {
            var matrix = new Matrix4x4(
                0, 9, 3, 0,
                9, 8, 0, 8,
                1, 8, 5, 3,
                0, 0, 5, 8);
            
            var expected = new Matrix4x4(
                0, 9, 1, 0,
                9, 8, 8, 0,
                3, 0, 5, 5,
                0, 8, 3, 8);

            var actual = Matrix4x4.Transpose(matrix);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransposeIdentityMatrix()
        {
            Assert.Equal(Matrix4x4.Identity(), Matrix4x4.Transpose(Matrix4x4.Identity()));
        }

        [Fact]
        public void SubMatrix()
        {
            var matrix = new Matrix4x4(
                -6, 1, 1, 6,
                -8, 5, 8, 6,
                -1, 0, 8, 2,
                -7, 1, -1, 1);
            
            var expected = new Matrix3x3(
                -6, 1, 6,
                -8, 8, 6,
                -7, -1, 1);
            
            Assert.Equal(expected, Matrix4x4.SubMatrix(matrix, 2, 1));
        }

        [Fact]
        public void Determinate()
        {
            var matrix = new Matrix4x4(
                -2, -8, 3, 5,
                -3, 1, 7, 3,
                1, 2, -9, 6,
                -6, 7, 7, -9);
            
            Assert.Equal(690, Matrix4x4.Cofactor(matrix, 0, 0));
            Assert.Equal(447, Matrix4x4.Cofactor(matrix, 0, 1));
            Assert.Equal(210, Matrix4x4.Cofactor(matrix, 0, 2));
            Assert.Equal(51, Matrix4x4.Cofactor(matrix, 0, 3));
            Assert.Equal(-4071, Matrix4x4.Determinate(matrix));
        }

        [Fact]
        public void IsInvertible()
        {
            var matrix = new Matrix4x4(
                6, 4, 4, 4,
                5, 5, 7, 6,
                4, -9, 3, -7,
                9, 1, 7, -6);

            Assert.True(Matrix4x4.IsInvertible(matrix));
            
            matrix = new Matrix4x4(
                -4, 2, -2, -3,
                9, 6, 2, 6,
                0, -5, 1, -5,
                0, 0, 0, 0);
            
            Assert.False(Matrix4x4.IsInvertible(matrix));
        }

        [Fact]
        public void Inverse()
        {
            var matrix = new Matrix4x4(
                -5, 2, 6, -8,
                1, -5, 1, 8,
                7, 7, -6, -7,
                1, -3, 7, 4);

            var inverse = Matrix4x4.Inverse(matrix);
            
            Assert.Equal(532, Matrix4x4.Determinate(matrix));
            Assert.Equal(-160, Matrix4x4.Cofactor(matrix, 2, 3));
            Assert.Equal(-160/532f, inverse[3, 2]);
            Assert.Equal(105, Matrix4x4.Cofactor(matrix, 3, 2));
            Assert.Equal(105/532f, inverse[2, 3]);
            
            var expected = new Matrix4x4(
                 0.21805f,  0.45113f,  0.24060f, -0.04511f,
                -0.80827f, -1.45677f, -0.44361f,  0.52068f,
                -0.07895f, -0.22368f, -0.05263f,  0.19737f,
                -0.52256f, -0.81391f, -0.30075f,  0.30639f);
            
            Assert.Equal(expected, inverse);
            
            matrix = new Matrix4x4(
                8, -5, 9, 2,
                7, 5, 6, 1,
                -6, 0, 9, 6,
                -3, 0, -9, -4);
            
            expected = new Matrix4x4(
                -0.15385f, -0.15385f, -0.28205f, -0.53846f,
                -0.07692f,  0.12308f,  0.02564f,  0.03077f,
                 0.35897f,  0.35897f,  0.43590f,  0.92308f,
                -0.69231f, -0.69231f, -0.76923f, -1.92308f);
            
            Assert.Equal(expected, Matrix4x4.Inverse(matrix));
            
            matrix = new Matrix4x4(
                9, 3, 0, 9,
                -5, -2, -6, -3,
                -4, 9, 6, 4,
                -7, 6, 6, 2);
            
            expected = new Matrix4x4(
                -0.04074f, -0.07778f,  0.14444f, -0.22222f,
                -0.07778f,  0.03333f,  0.36667f, -0.33333f,
                -0.02901f, -0.14630f, -0.10926f,  0.12963f,
                 0.17778f,  0.06667f, -0.26667f,  0.33333f);

            inverse = Matrix4x4.Inverse(matrix);
            
            Assert.Equal(expected, Matrix4x4.Inverse(matrix));
        }

        [Fact]
        public void InverseMultiplication()
        {
            var left = new Matrix4x4(
                3, -9, 7, 3,
                3, -8, 2, -9,
                -4, 4, 4, 1,
                -6, 5, -1, 1);
            
            var right = new Matrix4x4(
                8, 2, 2, 2,
                3, -1, 7, 0,
                7, 0, 5, 4,
                6, -2, 0, 5);

            var product = left * right;
            
            Assert.Equal(left, product * Matrix4x4.Inverse(right));
        }
    }
}