using System;

namespace RayTracer.Lib
{
    public struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        private readonly float[,] matrix;
        public const int NUM_ROWS = 3;
        public const int NUM_COLS = 3;
        
        public Matrix3x3(float m00, float m01, float m02,
                         float m10, float m11, float m12,
                         float m20, float m21, float m22)
        {
            matrix = new float[NUM_ROWS, NUM_COLS];
            
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            matrix[0, 2] = m02;
            
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
            matrix[1, 2] = m12;
            
            matrix[2, 0] = m20;
            matrix[2, 1] = m21;
            matrix[2, 2] = m22;
        }

        public static Matrix3x3 Identity()
        {
            return new Matrix3x3(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1);
        }
        
        public float this[int row, int col] => matrix[row, col];

        public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
        {
            return !left.Equals(right);
        }

        public static Matrix2x2 SubMatrix(Matrix3x3 matrix, int row, int column)
        {
            float[] subMatrix = new float[Matrix2x2.NUM_ROWS * Matrix2x2.NUM_COLS];
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
            
            return new Matrix2x2(
                subMatrix[0], subMatrix[1],
                subMatrix[2], subMatrix[3]);
        }

        public static float Minor(Matrix3x3 matrix, int row, int column)
        {
            return Matrix2x2.Determinate(SubMatrix(matrix, row, column));
        }

        public static float Cofactor(Matrix3x3 matrix, int row, int column)
        {
            bool isOdd = ((row + column) & 0x01) == 1;
            float minor = Minor(matrix, row, column);

            if (isOdd)
            {
                return -minor;
            }
            
            return minor;
        }
        
        public static float Determinate(Matrix3x3 matrix)
        {
            return matrix[0, 0] * Cofactor(matrix, 0, 0)
                 + matrix[0, 1] * Cofactor(matrix, 0, 1)
                 + matrix[0, 2] * Cofactor(matrix, 0, 2);
        }
        
        public bool Equals(Matrix3x3 other)
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
                && (obj is Matrix3x3 other)
                && Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = matrix[0, 0].GetHashCode();
            hash ^= matrix[0, 1].GetHashCode();
            hash ^= matrix[0, 2].GetHashCode();
            
            hash ^= matrix[1, 0].GetHashCode();
            hash ^= matrix[1, 1].GetHashCode();
            hash ^= matrix[1, 2].GetHashCode();
            
            hash ^= matrix[2, 0].GetHashCode();
            hash ^= matrix[2, 1].GetHashCode();
            hash ^= matrix[2, 2].GetHashCode();

            return hash;
        }
    }
}