using Accord.Math;
using RayTracer.GeometryObjects;

namespace RayTracer
{
    public class Triangle : GeometryObject
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public Vector3 C { get; }

        //Note the counter-clockwise order
        public Triangle(Vector3 a, Vector3 c, Vector3 b, ObjectProperties objProps, Matrix4x4 transform)
            : base(objProps, transform)
        {
            A = Transform.TransformVector(a, TransformMat);
            B = Transform.TransformVector(b, TransformMat);
            C = Transform.TransformVector(c, TransformMat);

            CalculateNormal();
        }

        private void CalculateNormal() => Normal = Vector3.Cross(C - A, B - A).Normalization();
    }
}