using Accord.Math;

namespace RayTracer
{
    public abstract class GeometryObject
    {
        public ObjectProperties ObjProperties { get;}
        public Matrix4x4 Transform { get; }

        protected GeometryObject(ObjectProperties objProp, Matrix4x4 transform)
        {
            ObjProperties = objProp;
            Transform = transform;
        }
    }
}