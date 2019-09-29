using System.IO;
using CommandLine;

namespace RayTracer
{
    public class Options
    {
        private string outFile;
        
        [Option(
            'f',
            "file",
            Required = true,
            HelpText = "Input yaml file")]
        public string InFile { get; set; }

        [Option(
            'o',
            "out",
            Required = false,
            HelpText = "Output file")]
        public string OutFile
        {
            get
            {
                if (outFile == null)
                {
                    return Path.ChangeExtension(InFile, ".ppm");
                }
                
                return outFile;
            }
            set => outFile = value;
        }
        
        [Option(
            'r',
            "recurse",
            Default = 5,
            Required = false,
            HelpText = "Number of bounces a reflected ray will take")]
        public int RecursiveDepth { get; set; }
    }
}