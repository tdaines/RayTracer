using System.IO;
using CommandLine;
using RayTracer.Lib;

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
            var (world, camera) = new YamlParser().LoadYamlFile(options.InFile);

            var width = options.Width ?? camera.Width;
            var height = options.Height ?? camera.Height;
            
            camera = new Camera(width, height, camera.FieldOfView, camera.Transform);
            
            var canvas = world.Render(camera, options.RecursiveDepth);
            
            File.WriteAllLines(options.OutFile, canvas.GetPortablePixmap());
        }
    }
}