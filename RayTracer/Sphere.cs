using Accord.Math;


namespace RayTracer
   {
       public class Sphere : GeometryObject
       {
           public Sphere(Vector3 centerPos, float r, ObjectProperties objProps, Matrix4x4 transform)
               : base(objProps, transform)
           {
               OriginalPos = centerPos;
               Radius = r;
               CenterPos = Helpers.TransformVector(OriginalPos, Transform);
           }
           
           
           // how will radius change when transform is applied???
           public Vector3 CenterPos { get; set; }
           public float Radius { get; set; }
           
           // Don't forget to implement somehow
           public Vector3 Normal { get; set; }
           
           public Vector3 OriginalPos { get; }
       }
   }