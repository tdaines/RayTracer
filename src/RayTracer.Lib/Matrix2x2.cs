using System;

namespace RayTracer.Lib
{
    public struct Matrix2x2 : IEquatable<Matrix2x2>
    {
        private readonly float[,] matrix;
        private const int NUM_ROWS = 2;
        private const int NUM_COLS = 2;
        
        public Matrix2x2(float m00, float m01,
                         float m10, float m11)
        {
            matrix = new float[NUM_ROWS, NUM_COLS];
            
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
        }

        public static Matrix2x2 Identity()
        {
            return new Matrix2x2(1, 0,
                                 0, 1);
        }
        
        public float this[int row, int col] => matrix[row, col];

        public static bool operator ==(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Matrix2x2 left, Matrix2x2 right)
        {
            return !left.Equals(right);
        }
        
        public bool Equals(Matrix2x2 other)
        {
            return matrix[0, 0].ApproximatelyEquals(other[0, 0])
                && matrix[0, 1].ApproximatelyEquals(other[0, 1])
                && matrix[1, 0].ApproximatelyEquals(other[1, 0])
                && matrix[1, 1].ApproximatelyEquals(other[1, 1]);
        }
        
        public override bool Equals(object obj)
        {
            return (obj != null) 
                && (obj is Matrix2x2 other)
                && Equals(other);
        }

        public override int GetHashCode()
        {
            return matrix[0, 0].GetHashCode()
                 ^ matrix[0, 1].GetHashCode()
                 ^ matrix[1, 0].GetHashCode()
                 ^ matrix[1, 1].GetHashCode();
        }
    }
}