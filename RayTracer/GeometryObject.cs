using System.Numerics;

namespace RayTracer
{
    public class GeometryObject
    {
        public ObjectProperties ObjProperties { get;}
        public Matrix4x4 Transform { get; }

        public GeometryObject(ObjectProperties objProp, Matrix4x4 transform)
        {
            ObjProperties = objProp;
            Transform = transform;
        }
    }
}