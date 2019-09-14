using Xunit;

namespace RayTracer.Lib.Test
{
    public class IntersectionInfoTests
    {
        [Fact]
        public void Constructor()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();
            var intersection = new Intersection(4, sphere);
            
            var info = new IntersectionInfo(intersection, ray);
            Assert.Equal(4, info.Intersection.Time);
            Assert.Equal(sphere, info.Intersection.Object);
            Assert.Equal(new Point(0, 0, -1), info.Point);
            Assert.Equal(new Vector(0, 0, -1), info.EyeVector);
            Assert.Equal(new Vector(0, 0, -1), info.Normal);
            Assert.False(info.Inside);
        }
        
        [Fact]
        public void ConstructorHitInsideObject()
        {
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var sphere = new Sphere();
            var intersection = new Intersection(1, sphere);
            
            var info = new IntersectionInfo(intersection, ray);
            Assert.Equal(1, info.Intersection.Time);
            Assert.Equal(sphere, info.Intersection.Object);
            Assert.Equal(new Point(0, 0, 1), info.Point);
            Assert.Equal(new Vector(0, 0, -1), info.EyeVector);
            Assert.Equal(new Vector(0, 0, -1), info.Normal);
            Assert.True(info.Inside);
        }

        [Fact]
        public void ConstructorHitOffsetsPoint()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Translation(0, 0, 1));
            var intersection = new Intersection(5, sphere);
            
            var info = new IntersectionInfo(intersection, ray);
            Assert.True(info.OverPoint.Z < -0.01f / 2.0f);
            Assert.True(info.Point.Z > info.OverPoint.Z);
        }
    }
}