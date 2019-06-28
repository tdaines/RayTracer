using Xunit;

namespace RayTracer.Lib.Test
{
    public class ColorTests
    {
        [Fact]
        public void AddColors()
        {
            var left = new Color(0.9f, 0.6f, 0.75f);
            var right = new Color(0.7f, 0.1f, 0.25f);

            var sum = left + right;
            Assert.Equal(new Color(1.6f, 0.7f, 1.0f), sum);
        }
        
        [Fact]
        public void SubtractingColors()
        {
            var left = new Color(0.9f, 0.6f, 0.75f);
            var right = new Color(0.7f, 0.1f, 0.25f);

            var diff = left - right;
            Assert.Equal(new Color(0.2f, 0.5f, 0.5f), diff);

            var x = System.Drawing.Color.Black;
        }
        
        [Fact]
        public void MultiplyColorByScalar()
        {
            var value = new Color(0.2f, 0.3f, 0.4f);
            var scalar = 2f;
            
            Assert.Equal(new Color(0.4f, 0.6f, 0.8f), value * scalar);
            Assert.Equal(new Color(0.4f, 0.6f, 0.8f), scalar * value);
        }
        
        [Fact]
        public void MultiplyColors()
        {
            var left = new Color(1, 0.2f, 0.4f);
            var right = new Color(0.9f, 1, 0.1f);
            
            Assert.Equal(new Color(0.9f, 0.2f, 0.04f), left * right);
        }
    }
}