using System;
using System.Diagnostics;

namespace RayTracer.Lib
{
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        private readonly float[,] matrix;
        public const int NUM_ROWS = 4;
        public const int NUM_COLS = 4;
        
        public Matrix4x4(float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33)
        {
            matrix = new float[NUM_ROWS, NUM_COLS];
            
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            matrix[0, 2] = m02;
            matrix[0, 3] = m03;
            
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
            matrix[1, 2] = m12;
            matrix[1, 3] = m13;
            
            matrix[2, 0] = m20;
            matrix[2, 1] = m21;
            matrix[2, 2] = m22;
            matrix[2, 3] = m23;
            
            matrix[3, 0] = m30;
            matrix[3, 1] = m31;
            matrix[3, 2] = m32;
            matrix[3, 3] = m33;
        }

        public static Matrix4x4 Identity()
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);
        }
        
        public float this[int row, int col] => matrix[row, col];

        public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
        {
            return !left.Equals(right);
        }

        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            var result = new float[NUM_ROWS, NUM_COLS];
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    result[row, col] = left[row, 0] * right[0, col] +
                                       left[row, 1] * right[1, col] +
                                       left[row, 2] * right[2, col] +
                                       left[row, 3] * right[3, col];
                }
            }

            return new Matrix4x4(
                result[0, 0], result[0, 1], result[0, 2], result[0, 3],
                result[1, 0], result[1, 1], result[1, 2], result[1, 3],
                result[2, 0], result[2, 1], result[2, 2], result[2, 3],
                result[3, 0], result[3, 1], result[3, 2], result[3, 3]);
        }

        public static Vector operator *(Matrix4x4 matrix, Vector vector)
        {
            var x = matrix[0, 0] * vector.X +
                    matrix[0, 1] * vector.Y +
                    matrix[0, 2] * vector.Z +
                    matrix[0, 3] * vector.W;
            
            var y = matrix[1, 0] * vector.X +
                    matrix[1, 1] * vector.Y +
                    matrix[1, 2] * vector.Z +
                    matrix[1, 3] * vector.W;
            
            var z = matrix[2, 0] * vector.X +
                    matrix[2, 1] * vector.Y +
                    matrix[2, 2] * vector.Z +
                    matrix[2, 3] * vector.W;
            
            var w = matrix[3, 0] * vector.X +
                    matrix[3, 1] * vector.Y +
                    matrix[3, 2] * vector.Z +
                    matrix[3, 3] * vector.W;
            
            return new Vector(x, y, z, w);
        }
        
        public static Vector operator *(Vector vector, Matrix4x4 matrix)
        {
            return matrix * vector;
        }

        public static Matrix4x4 Transpose(Matrix4x4 matrix)
        {
            return new Matrix4x4(
                matrix[0, 0], matrix[1, 0], matrix[2, 0], matrix[3, 0],
                matrix[0, 1], matrix[1, 1], matrix[2, 1], matrix[3, 1],
                matrix[0, 2], matrix[1, 2], matrix[2, 2], matrix[3, 2], 
                matrix[0, 3], matrix[1, 3], matrix[2, 3], matrix[3, 3]);
        }

        public static Matrix3x3 SubMatrix(Matrix4x4 matrix, int row, int column)
        {
            float[] subMatrix = new float[Matrix3x3.NUM_ROWS * Matrix3x3.NUM_COLS];
            int index = 0;
            
            for (int r = 0; r < NUM_ROWS; r++)
            {
                if (r == row) r++;

                if (r >= NUM_ROWS) break;

                for (int c = 0; c < NUM_COLS; c++)
                {
                    if (c == column) c++;

                    if (c >= NUM_COLS) break;

                    subMatrix[index++] = matrix[r, c];
                }
            }

            return new Matrix3x3(
                subMatrix[0], subMatrix[1], subMatrix[2],
                subMatrix[3], subMatrix[4], subMatrix[5],
                subMatrix[6], subMatrix[7], subMatrix[8]);
        }
        
        public static float Minor(Matrix4x4 matrix, int row, int column)
        {
            return Matrix3x3.Determinate(SubMatrix(matrix, row, column));
        }

        public static float Cofactor(Matrix4x4 matrix, int row, int column)
        {
            bool isOdd = ((row + column) & 0x01) == 1;
            float minor = Minor(matrix, row, column);

            if (isOdd)
            {
                return -minor;
            }
            
            return minor;
        }
        
        public static float Determinate(Matrix4x4 matrix)
        {
            return matrix[0, 0] * Cofactor(matrix, 0, 0)
                 + matrix[0, 1] * Cofactor(matrix, 0, 1)
                 + matrix[0, 2] * Cofactor(matrix, 0, 2)
                 + matrix[0, 3] * Cofactor(matrix, 0, 3);
        }

        public static bool IsInvertible(Matrix4x4 matrix)
        {
            float determinate = Determinate(matrix);
            return determinate < 0 || determinate > 0;
        }

        public static Matrix4x4 Inverse(Matrix4x4 matrix)
        {
            float determinate = Determinate(matrix);

            var cofactors = new Matrix4x4(
                Cofactor(matrix, 0, 0), Cofactor(matrix, 0, 1), Cofactor(matrix, 0, 2), Cofactor(matrix, 0, 3),
                Cofactor(matrix, 1, 0), Cofactor(matrix, 1, 1), Cofactor(matrix, 1, 2), Cofactor(matrix, 1, 3),
                Cofactor(matrix, 2, 0), Cofactor(matrix, 2, 1), Cofactor(matrix, 2, 2), Cofactor(matrix, 2, 3),
                Cofactor(matrix, 3, 0), Cofactor(matrix, 3, 1), Cofactor(matrix, 3, 2), Cofactor(matrix, 3, 3));

            var transposed = Transpose(cofactors);
            
            return new Matrix4x4(
                transposed[0, 0] / determinate, transposed[0, 1] / determinate, transposed[0, 2] / determinate, transposed[0, 3] / determinate,
                transposed[1, 0] / determinate, transposed[1, 1] / determinate, transposed[1, 2] / determinate, transposed[1, 3] / determinate,
                transposed[2, 0] / determinate, transposed[2, 1] / determinate, transposed[2, 2] / determinate, transposed[2, 3] / determinate,
                transposed[3, 0] / determinate, transposed[3, 1] / determinate, transposed[3, 2] / determinate, transposed[3, 3] / determinate);
        }
        
        public bool Equals(Matrix4x4 other)
        {
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    if (!matrix[row, col].ApproximatelyEquals(other[row, col]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        public override bool Equals(object obj)
        {
            return (obj != null) 
                && (obj is Matrix4x4 other)
                && Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = matrix[0, 0].GetHashCode();
            hash ^= matrix[0, 1].GetHashCode();
            hash ^= matrix[0, 2].GetHashCode();
            hash ^= matrix[0, 3].GetHashCode();
            
            hash ^= matrix[1, 0].GetHashCode();
            hash ^= matrix[1, 1].GetHashCode();
            hash ^= matrix[1, 2].GetHashCode();
            hash ^= matrix[1, 3].GetHashCode();
            
            hash ^= matrix[2, 0].GetHashCode();
            hash ^= matrix[2, 1].GetHashCode();
            hash ^= matrix[2, 2].GetHashCode();
            hash ^= matrix[2, 3].GetHashCode();
            
            hash ^= matrix[3, 0].GetHashCode();
            hash ^= matrix[3, 1].GetHashCode();
            hash ^= matrix[3, 2].GetHashCode();
            hash ^= matrix[3, 3].GetHashCode();

            return hash;
        }
    }
}