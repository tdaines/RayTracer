using System;
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
            
            var info = new IntersectionInfo(new Intersections(intersection), intersection, ray);
            Assert.Equal(4, info.Intersection.Time);
            Assert.Equal(sphere, info.Intersection.Shape);
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
            
            var info = new IntersectionInfo(new Intersections(intersection), intersection, ray);
            Assert.Equal(1, info.Intersection.Time);
            Assert.Equal(sphere, info.Intersection.Shape);
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
            
            var info = new IntersectionInfo(new Intersections(intersection), intersection, ray);
            Assert.True(info.OverPoint.Z < -0.0001f / 2);
            Assert.True(info.Point.Z > info.OverPoint.Z);
        }

        [Fact]
        public void ConstructorReflection()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 1, -1), new Vector(0, -MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2));
            var intersection = new Intersection(MathF.Sqrt(2), plane);
            
            var info = new IntersectionInfo(new Intersections(intersection), intersection, ray);
            Assert.Equal(new Vector(0, MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2), info.ReflectVector);
            
        }
        
        [Theory]
        [InlineData(0, 1.0f, 1.5f)]
        [InlineData(1, 1.5f, 2.0f)]
        [InlineData(2, 2.0f, 2.5f)]
        [InlineData(3, 2.5f, 2.5f)]
        [InlineData(4, 2.5f, 1.5f)]
        [InlineData(5, 1.5f, 1.0f)]
        public void ConstructorN1AndN2(int index, float n1, float n2)
        {
            var outer = new Sphere(
                Matrix4x4.Scaling(2, 2, 2),
                Material.Glass());
            
            var innerLeft = new Sphere(
                Matrix4x4.Translation(0, 0, -0.25f),
                new Material(refractiveIndex: 2.0f));
            
            var innerRight = new Sphere(
                Matrix4x4.Translation(0, 0, 0.25f),
                new Material(refractiveIndex: 2.5f));

            var ray = new Ray(new Point(0, 0, -4), Vector.UnitZ);
            
            var intersections = new Intersections(
                new Intersection(2, outer),            // entering outer sphere
                new Intersection(2.75f, innerLeft),    // entering inner left sphere
                new Intersection(3.25f, innerRight),   // entering inner right sphere
                new Intersection(4.75f, innerLeft),    // exiting inner left sphere
                new Intersection(5.25f, innerRight),   // exiting inner right sphere
                new Intersection(6, outer));           // exiting outer sphere
            
            var info = new IntersectionInfo(intersections, intersections[index], ray);
            
            Assert.Equal(n1, info.RefractiveIndex1);
            Assert.Equal(n2, info.RefractiveIndex2);
        }
        
        [Fact]
        public void ConstructorUnderPoint()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Translation(0, 0, 1));
            var intersection = new Intersection(5, sphere);
            
            var info = new IntersectionInfo(new Intersections(intersection), intersection, ray);
            Assert.True(info.UnderPoint.Z > 0.0001f / 2);
            Assert.True(info.Point.Z < info.UnderPoint.Z);
        }

        [Fact]
        public void ReflectanceTotalInternalReflection()
        {
            var sphere = new Sphere(Material.Glass());
            var ray = new Ray(new Point(0, 0, MathF.Sqrt(2) / 2), Vector.UnitY);
            var intersections = new Intersections(
                new Intersection(-MathF.Sqrt(2) / 2, sphere),
                new Intersection(MathF.Sqrt(2) / 2, sphere));
            var info = new IntersectionInfo(intersections, intersections[1], ray);
            
            Assert.Equal(1, info.Reflectance());
        }
        
        [Fact]
        public void ReflectancePerpendicular()
        {
            var sphere = new Sphere(Material.Glass());
            var ray = new Ray(new Point(0, 0, 0), Vector.UnitY);
            var intersections = new Intersections(
                new Intersection(-1, sphere),
                new Intersection(1, sphere));
            var info = new IntersectionInfo(intersections, intersections[1], ray);
            
            Assert.Equal(0.040000003f, info.Reflectance());
        }
        
        [Fact]
        public void ReflectanceSmallAngle()
        {
            var sphere = new Sphere(Material.Glass());
            var ray = new Ray(new Point(0, 0.99f, -2), Vector.UnitZ);
            var intersections = new Intersections(new Intersection(1.8589f, sphere));
            var info = new IntersectionInfo(intersections, intersections[0], ray);
            
            Assert.Equal(0.4887307f, info.Reflectance());
        }
    }
}