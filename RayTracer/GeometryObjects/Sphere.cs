using Accord.Math;

namespace RayTracer.GeometryObjects
   {
       public class Sphere : GeometryObject
       {
           public Sphere(Vector3 centerPos, float r, ObjectProperties objProps, Matrix4x4 transform)
               : base(objProps, transform)
           {
               OriginalPos = centerPos;
               Radius = r;
               CenterPos = Transform.TransformVector(OriginalPos, TransformMat);
               InverseTransformMat = Transform.GetInverseMatrix4X4(TransformMat);
           }
           
           public Vector3 CenterPos { get; set; }
           public float Radius { get; set; }
           public Matrix4x4 InverseTransformMat { get; }
           public Vector3 OriginalPos { get; }
       }
   }