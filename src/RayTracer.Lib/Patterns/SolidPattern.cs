namespace RayTracer.Lib.Patterns
{
    public class SolidPattern : Pattern
    {
        public Color Color { get; }
        
        public SolidPattern(Matrix4x4 transform, Color color) : base(transform)
        {
            Color = color;
        }

        public SolidPattern(Color color) : this(Matrix4x4.Identity(), color)
        {
        }

        public override Color ColorAt(Shape shape, Point worldPoint)
        {
            return Color;
        }

        public override Color ColorAt(Point point)
        {
            return Color;
        }

        public static SolidPattern Black => new SolidPattern(Color.Black);
        public static SolidPattern Red => new SolidPattern(Color.Red);
        public static SolidPattern Green => new SolidPattern(Color.Green);
        public static SolidPattern Blue => new SolidPattern(Color.Blue);
        public static SolidPattern White => new SolidPattern(Color.White);
        public static SolidPattern Yellow => new SolidPattern(Color.Yellow);
        public static SolidPattern Cyan => new SolidPattern(Color.Cyan);
        public static SolidPattern Purple => new SolidPattern(Color.Purple);
    }
}