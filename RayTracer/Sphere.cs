using System.Numerics;

namespace RayTracer
{
    public class Sphere : GeometryObject
    {
        public Sphere(Vector3 centerPos, float r, ObjectProperties objProps, Matrix4x4 transform)
            : base(objProps, transform)
        {
            if (!_isOriginalSet)
            {
                OriginalPos = centerPos;
                OriginalRadius = r;
                _isOriginalSet = true;
            }
            CenterPos = centerPos;
            Radius = r;
        }
        
        public Vector3 CenterPos { get;}
        public float Radius { get;}
        
        public Vector3 OriginalPos { get; }
        public float OriginalRadius { get; }
        private readonly bool _isOriginalSet;
    }
}