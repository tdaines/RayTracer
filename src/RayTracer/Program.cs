using System.IO;
using CommandLine;

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

            var canvas = world.Render(camera, options.RecursiveDepth);
            
            File.WriteAllLines(options.OutFile, canvas.GetPortablePixmap());
        }
    }
}