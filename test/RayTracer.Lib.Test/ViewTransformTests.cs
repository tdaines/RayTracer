using Xunit;

namespace RayTracer.Lib.Test
{
    public class ViewTransformTests
    {
        [Fact]
        public void CreateDefaultOrientation()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, -1);
            var up = new Vector(0, 1, 0);
            
            Assert.Equal(Matrix4x4.Identity(), ViewTransform.Create(from, to, up));
        }
        
        [Fact]
        public void CreateScaling()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, 1);
            var up = new Vector(0, 1, 0);

            var expected = Matrix4x4.Scaling(-1, 1, -1);
            var actual = ViewTransform.Create(from, to, up);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void CreateTranslation()
        {
            var from = new Point(0, 0, 8);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);

            var expected = Matrix4x4.Translation(0, 0, -8);
            var actual = ViewTransform.Create(from, to, up);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void CreateArbitrary()
        {
            var from = new Point(1, 3, 2);
            var to = new Point(4, -2, 8);
            var up = new Vector(1, 1, 0);

            var expected = new Matrix4x4(
                -0.50709f, 0.50709f, 0.67612f, -2.36643f,
                 0.76772f, 0.60609f, 0.12122f, -2.82843f,
                -0.35857f, 0.59761f, -0.71714f, 0,
                 0,        0,         0,        1);
            var actual = ViewTransform.Create(from, to, up);
            
            Assert.Equal(expected, actual);
        }
    }
}