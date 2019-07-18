using Accord.Math;
using RayTracer.GeometryObjects;


namespace RayTracer
   {
       public class Sphere : GeometryObject
       {
           public Sphere(Vector3 centerPos, float r, ObjectProperties objProps, Matrix4x4 transform)
               : base(objProps, transform)
           {
               OriginalPos = centerPos;
               Radius = r;
               CenterPos = Transform.TransformVector(OriginalPos, TransformMat);
           }
           
           public Vector3 CenterPos { get; set; }
           public float Radius { get; set; }
           
           public Vector3 OriginalPos { get; }
       }
   }