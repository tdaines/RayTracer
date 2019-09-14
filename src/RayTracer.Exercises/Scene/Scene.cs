using System;
using System.IO;
using RayTracer.Lib;

namespace RayTracer.Exercises.Scene
{
    public class Scene
    {
        public static void Run()
        {
            var floorTransform = Matrix4x4.Scaling(10, 0.01f, 10);
            var floorMaterial = new Material(new Color(1, 0.9f, 0.9f), specular: 0);
            var floor = new Lib.Sphere(floorTransform, floorMaterial);

            var leftWallTransform = Matrix4x4.Translation(0, 0, 5) *
                                    Matrix4x4.RotationY(-MathF.PI / 4) *
                                    Matrix4x4.RotationX(MathF.PI / 2) *
                                    Matrix4x4.Scaling(10, 0.01f, 10);
            var leftWall = new Lib.Sphere(leftWallTransform, floorMaterial);
            
            var rightWallTransform = Matrix4x4.Translation(0, 0, 5) *
                                     Matrix4x4.RotationY(MathF.PI / 4) *
                                     Matrix4x4.RotationX(MathF.PI / 2) *
                                     Matrix4x4.Scaling(10, 0.01f, 10);
            var rightWall = new Lib.Sphere(rightWallTransform, floorMaterial);
            
            var middleTransform = Matrix4x4.Translation(-0.5f, 1, 0.5f);
            var middleMaterial = new Material(new Color(0.1f, 1, 0.5f), diffuse: 0.7f, specular: 0.3f);
            var middle = new Lib.Sphere(middleTransform, middleMaterial);
            
            var rightTransform = Matrix4x4.Translation(1.5f, 0.5f, -0.5f) *
                                 Matrix4x4.Scaling(0.5f, 0.5f, 0.5f);
            var rightMaterial = new Material(new Color(0.5f, 1, 0.1f), diffuse: 0.7f, specular: 0.3f);
            var right = new Lib.Sphere(rightTransform, rightMaterial);
            
            var leftTransform = Matrix4x4.Translation(-1.5f, 0.33f, -0.75f) *
                                Matrix4x4.Scaling(0.33f, 0.33f, 0.33f);
            var leftMaterial = new Material(new Color(1, 0.8f, 0.1f), diffuse: 0.7f, specular: 0.3f);
            var left = new Lib.Sphere(leftTransform, leftMaterial);
            
            var light = new PointLight(new Point(-10, 10, -10), new Color(0.5f, 0.5f, 0.5f));
            var light2 = new PointLight(new Point(10, 10, -10), new Color(0.2f, 0.2f, 0.2f));
            var light3 = new PointLight(new Point(0, 10, -10), new Color(0.2f, 0.2f, 0.2f));
            
            var from = new Point(0, 1.5f, -5);
            var to = new Point(0, 1, 0);
            var up = new Vector(0, 1, 0);
            var cameraTransform = ViewTransform.Create(from, to, up);
            var camera = new Camera(400, 200, MathF.PI / 3, cameraTransform);
            
            var world = new World();
            world.Lights.Add(light);
            world.Lights.Add(light2);
            world.Lights.Add(light3);
            world.Shapes.Add(floor);
            world.Shapes.Add(leftWall);
            world.Shapes.Add(rightWall);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);
            world.Shapes.Add(left);
            
            var canvas = world.Render(camera);
            
            File.WriteAllLines("scene.ppm", canvas.GetPortablePixmap());
        }
    }
}