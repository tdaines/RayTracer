using Xunit;

namespace RayTracer.Lib.Test
{
    public class CanvasTests
    {
        [Fact]
        public void Constructor()
        {
            var canvas = new Canvas(5, 6);
            var black = Color.Black;
            
            Assert.Equal(5, canvas.Width);
            Assert.Equal(6, canvas.Height);

            for (int i = 0; i < canvas.Width; i++)
            {
                for (int j = 0; j < canvas.Height; j++)
                {
                    Assert.Equal(black, canvas[i, j]);
                }
            }
        }

        [Fact]
        public void GetPortablePixmap()
        {
            var canvas = new Canvas(5, 3);
            var c1 = new Color(1.5f, 0, 0);
            var c2 = new Color(0, 0.5f, 0);
            var c3 = new Color(-0.5f, 0, 1);

            canvas[0, 0] = c1;
            canvas[2, 1] = c2;
            canvas[4, 2] = c3;

            var pixMap = canvas.GetPortablePixmap();
            
            Assert.Equal("P3\n" +
                                  "5 3\n" +
                                  "255\n" +
                                  "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0 \n" +
                                  "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0 \n" +
                                  "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255 \n" +
                                  "\n", pixMap);
        }
        
        [Fact]
        public void GetPortablePixmap_LongLines()
        {
            var background = new Color(1, 0.8f, 0.6f);
            var canvas = new Canvas(10, 2, background);
            var pixMap = canvas.GetPortablePixmap();
            
            Assert.Equal("P3\n" +
                         "10 2\n" +
                         "255\n" +
                         "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 \n" +
                         "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 \n" +
                         "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 \n" +
                         "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 \n" +
                         "\n", pixMap);
        }
    }
}