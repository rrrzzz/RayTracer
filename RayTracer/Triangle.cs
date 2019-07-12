using System.Numerics;

namespace RayTracer
{
    public class Triangle : GeometryObject
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public Vector3 C { get; }
        public Vector3 FaceNormal { get; }

        //Note the counter-clockwise order
        public Triangle(Vector3 a, Vector3 c, Vector3 b, ObjectProperties objProps, Matrix4x4 transform)
            : base(objProps, transform)
        {
            A = a;
            B = b;
            C = c;
            FaceNormal = Vector3.Cross(C - A, B - A);
        }
    }
}