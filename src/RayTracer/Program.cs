using System;
using System.Globalization;
using System.IO;
using CommandLine;
using RayTracer.Lib;
using RayTracer.Lib.Patterns;
using RayTracer.Lib.Shapes;
using Vector = RayTracer.Lib.Vector;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        }

        static void Run(Options options)
        {
            Cylinders();
            return;
            
            if (options.RaysPerPixel != 1 &&
                options.RaysPerPixel != 4 &&
                options.RaysPerPixel != 16)
            {
                throw new Exception("Invalid RaysPerPixel value. Value must be either 1, 4, or 16.");
            }
            
            var (world, camera) = new YamlParser().LoadYamlFile(options.InFile);

            var width = options.Width ?? camera.Width;
            var height = options.Height ?? camera.Height;
            
            camera = new Camera(width, height, camera.FieldOfView, camera.Transform);
            
            var canvas = world.Render(camera, options.RecursiveDepth, options.RaysPerPixel);
            
            File.WriteAllLines(options.OutFile, canvas.GetPortablePixmap());
        }

        static void Cylinders()
        {
            var from = new Point(0, 5, -5);
            var to = Point.Zero;
            var up = Vector.UnitY;
            var camera = new Camera(400, 200, 1, ViewTransform.Create(from, to, up));
            
            var world = new World();
            
            var floor = new Plane(new Material(new CheckeredPattern()));
            
            var cylinder = new Cylinder(new Material(new CheckeredPattern())) { Min = 0, Max = 2, Capped = false };
            
            var light = new PointLight(new Point(-5, 5, -5), new Color(0.8f, 0.8f, 0.8f));
            
            world.Add(floor);
            world.Add(cylinder);
            world.Add(light);

            var canvas = world.Render(camera);
            File.WriteAllLines("cylinder.ppm", canvas.GetPortablePixmap());
        }
    }
}