namespace RayTracer.Lib
{
    public abstract class BaseObject
    {
        public Matrix4x4 Transform { get; set; }
        
        protected BaseObject()
        {
            Transform = Matrix4x4.Identity();
        }

        protected BaseObject(Matrix4x4 transform)
        {
            Transform = transform;
        }
    }
}