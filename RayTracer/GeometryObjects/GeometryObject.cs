using Accord.Math;

namespace RayTracer.GeometryObjects
{
    public abstract class GeometryObject
    {
        public ObjectProperties ObjProperties { get;}
        public Matrix4x4 Transform { get; }
        public Vector3 Normal { get; set; }

        protected GeometryObject(ObjectProperties objProp, Matrix4x4 transform)
        {
            ObjProperties = objProp;
            Transform = transform;
        }
    }
}