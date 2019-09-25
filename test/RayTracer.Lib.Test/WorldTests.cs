using System;
using RayTracer.Lib.Patterns;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class WorldTests
    {
        [Fact]
        public void DefaultWorld()
        {
            var world = World.DefaultWorld();
            
            Assert.Equal(new Point(-10, 10, -10),  world.Lights[0].Position);
            Assert.Equal(Color.White, world.Lights[0].Intensity);

            var sphere = world.Shapes[0];
            Assert.Equal(new Color(0.8f, 1.0f, 0.6f), ((SolidPattern)sphere.Material.Pattern).Color);
            Assert.Equal(0.7f, sphere.Material.Diffuse);
            Assert.Equal(0.2f, sphere.Material.Specular);
            
            sphere = world.Shapes[1];
            Assert.Equal(Matrix4x4.Scaling(0.5f, 0.5f, 0.5f), sphere.Transform);
        }

        [Fact]
        public void IntersectDefaultWorldWithRay()
        {
            var world = World.DefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            var intersections = world.Intersect(ray);
            
            Assert.Equal(4, intersections.Count);
            Assert.Equal(4, intersections[0].Time);
            Assert.Equal(4.5f, intersections[1].Time);
            Assert.Equal(5.5f, intersections[2].Time);
            Assert.Equal(6, intersections[3].Time);
        }

        [Fact]
        public void ShadeHitIntersectionFromOutside()
        {
            var world = World.DefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = world.Shapes[0];
            var intersection = new Intersection(4, shape);
            var intersectionInfo = new IntersectionInfo(intersection, ray);
            
            Assert.Equal(new Color(0.38066f, 0.47583f, 0.2855f), world.ShadeHit(intersectionInfo));
        }
        
        [Fact]
        public void ShadeHitIntersectionFromInside()
        {
            var world = World.DefaultWorld(new PointLight(new Point(0, 0.25f, 0), Color.White));
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = world.Shapes[1];
            var intersection = new Intersection(0.5f, shape);
            var intersectionInfo = new IntersectionInfo(intersection, ray);
            
            Assert.Equal(new Color(0.9046617f, 0.9046617f, 0.9046617f), world.ShadeHit(intersectionInfo));
        }

        [Fact]
        public void ShadeHitIntersectionInShadow()
        {
            var world = new World();
            world.Lights.Add(new PointLight(new Point(0, 0, -10), Color.White));
            world.Shapes.Add(new Sphere());
            world.Shapes.Add(new Sphere(Matrix4x4.Translation(0, 0, 10)));
            
            var ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var intersection = new Intersection(4, world.Shapes[1]);
            var intersectionInfo = new IntersectionInfo(intersection, ray);
            
            Assert.Equal(new Color(0.1f, 0.1f, 0.1f), world.ShadeHit(intersectionInfo));
        }

        [Fact]
        public void ColorAtRayMiss()
        {
            var world = World.DefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
            
            Assert.Equal(new Color(0, 0, 0), world.ColorAt(ray));
        }
        
        [Fact]
        public void ColorAtRayHit()
        {
            var world = World.DefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            
            Assert.Equal(new Color(0.38066f, 0.47583f, 0.2855f), world.ColorAt(ray));
        }
        
        [Fact]
        public void ColorAtIntersectionBehindRay()
        {
            var world = World.DefaultWorld();
            world.Shapes[0] = new Sphere(new Material(new Color(0.8f, 1.0f, 0.6f), diffuse: 0.7f, specular: 0.2f, ambient: 1));
            var inner = world.Shapes[1] = new Sphere(Matrix4x4.Scaling(0.5f, 0.5f, 0.5f), new Material(ambient: 1));
            var ray = new Ray(new Point(0, 0, 0.75f), new Vector(0, 0, -1));
            
            Assert.Equal(((SolidPattern)inner.Material.Pattern).Color, world.ColorAt(ray));
        }

        [Fact]
        public void RenderDefaultWorld()
        {
            var from = new Point(0, 0, -5);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);
            var transform = ViewTransform.Create(from, to, up);
            var camera = new Camera(11, 11, MathF.PI / 2, transform);

            var world = World.DefaultWorld();
            var canvas = world.Render(camera);
            
            Assert.Equal(new Color(0.38066f, 0.47583f, 0.2855f),  canvas[5, 5]);
        }

        [Fact]
        public void IsShadowed()
        {
            var world = World.DefaultWorld();
            var point = new Point(0, 10, 0);
            
            Assert.False(world.IsShadowed(point, world.Lights[0]));
        }
        
        [Fact]
        public void IsShadowedObjectBetweenPointAndLight()
        {
            var world = World.DefaultWorld();
            var point = new Point(10, -10, 10);
            
            Assert.True(world.IsShadowed(point, world.Lights[0]));
        }
        
        [Fact]
        public void IsShadowedLightBetweenPointAndObject()
        {
            var world = World.DefaultWorld();
            var point = new Point(-20, 20, -20);
            
            Assert.False(world.IsShadowed(point, world.Lights[0]));
        }
        
        [Fact]
        public void IsShadowedPointBetweenLightAndObject()
        {
            var world = World.DefaultWorld();
            var point = new Point(-2, 2, -2);
            
            Assert.False(world.IsShadowed(point, world.Lights[0]));
        }
    }
}