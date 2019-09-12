using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Lib
{
    public class Canvas
    {
        public int Width { get; }
        public int Height { get; }
        
        private readonly Color[,] surface;

        public Canvas(int width, int height, Color background)
        {
            Width = width;
            Height = height;
            surface = new Color[width, height];
            InitBackground(background);
        }
        
        public Canvas(int width, int height) : this(width, height, Color.Black)
        {
        }

        private void InitBackground(Color color)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    surface[i, j] = color;
                }
            }
        }
        
        public Color this[int x, int y]
        {
            get => surface[x, y];
            set => surface[x, y] = value;
        }

        public void SetPixel(Point point, Color color)
        {
            int col = (int)point.X;
            int row = Height - (int)point.Y;

            surface[col, row] = color;
        }

        public string[] GetPortablePixmap()
        {
            var lines = new List<string>(((Width * Height) / 5) + 3);
            
            lines.Add("P3");
            lines.Add($"{Width} {Height}");
            lines.Add("255");

            int pixels = Width * Height;
            const int pixelsPerLine = 5;
            int pixelsWritten = 0;
            int row = 0;
            int col = 0;
            
            while (pixelsWritten < pixels)
            {
                int pixelsWrittenOnLine = 0;
                StringBuilder line = new StringBuilder(70);
                while (pixelsWrittenOnLine < pixelsPerLine)
                {
                    var color = surface[col, row];
                    var r = Clamp(color.R);
                    var g = Clamp(color.G);
                    var b = Clamp(color.B);
                    line.Append($"{r} {g} {b} ");

                    pixelsWrittenOnLine++;
                    pixelsWritten++;
                    
                    col++;
                    if (col >= Width)
                    {
                        col = 0;
                        row++;
                    }
                }
                
                lines.Add(line.ToString());
            }
            
            lines.Add("");

            return lines.ToArray();
        }

        private static int Clamp(float value)
        {
            value = Math.Max(value, 0);
            value = Math.Min(value, 1);

            return (int)MathF.Round(value * 255);
        }
    }
}