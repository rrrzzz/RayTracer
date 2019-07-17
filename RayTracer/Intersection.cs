using System;
using Accord.Math;
using RayTracer.GeometryObjects;

namespace RayTracer
{
    public struct IntersectionInfo
    {
        public IntersectionInfo(GeometryObject hitObject, Vector3 hitPoint)
        {
            HitObject = hitObject;
            HitPoint = hitPoint;
        }
        
        public GeometryObject HitObject { get; }
        public Vector3 HitPoint { get; }
    }
    
    public class Intersection
    {
        private Ray _ray;
        private Vector3 _hit;
        private float _minDist = float.MaxValue;
        private GeometryObject _hitObject;
        
        public Intersection(Ray ray)
        {
            _ray = ray;
        }

        public IntersectionInfo FindClosestIntersection()
        {
            foreach (var sphere in Globals.Spheres)
            {
                FindSphereIntersection(sphere);
            }

            foreach (var triangle in Globals.Triangles)
            {
                FindTriangleIntersection(triangle);
            }

            return new IntersectionInfo(_hitObject, _hit);
        }
        
        public bool IsLightObstructed(float distanceToLight)
        {
            foreach (var sphere in Globals.Spheres)
            {
                FindSphereIntersection(sphere);
                if (_minDist < distanceToLight)
                {
                    return true;
                }
            }
            
            foreach (var triangle in Globals.Triangles)
            {
                FindTriangleIntersection(triangle);
                if (_minDist < distanceToLight)
                {
                    return true;
                }
            }

            return false;
        }

        private void FindSphereIntersection(Sphere sphere)
        {
            float multiplier;

            var inverseTransform = Helpers.GetInverseMatrix4X4(sphere.Transform);
            var rayOriginTrans = Helpers.TransformVector(_ray.Origin, inverseTransform);
            var rayDirectionTrans = Helpers.TransformZeroWVector(_ray.Direction, inverseTransform);
            
            var a = Vector3.Dot(rayDirectionTrans, rayDirectionTrans);
            var b = 2 * Vector3.Dot(rayDirectionTrans, (rayOriginTrans - sphere.OriginalPos));
            var c = Vector3.Dot(rayOriginTrans - sphere.OriginalPos, rayOriginTrans - sphere.OriginalPos) -
                    sphere.Radius * sphere.Radius;

            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0) return;
            
            //this is multiplier of inversed ray, before point transform
            if (Math.Abs(discriminant) < Globals.Delta) multiplier = -b / 2*a;
            else
            {
                var mult1 = (-b + Math.Sqrt(discriminant)) / 2 * a;
                var mult2 = (-b - Math.Sqrt(discriminant)) / 2 * a;

                multiplier = (float)Math.Min(mult1, mult2);
                multiplier = multiplier > 0 ? multiplier : (float)Math.Max(mult1, mult2);
            }
            
            if (multiplier <= 0) return;

            var intersectionPoint = rayOriginTrans + rayDirectionTrans * multiplier;
            
            //is this really vector from rayOrigin? not from 0,0,0?
            // transform sphere normal here inverse transpose! and think about where to store spehere normal
            intersectionPoint = Helpers.TransformVector(intersectionPoint, sphere.Transform);
            
            sphere.Normal = intersectionPoint - sphere.CenterPos;
            sphere.Normal.Normalize();
            
            CheckDistanceUpdateHit(intersectionPoint, sphere);
        }
        
        private void FindTriangleIntersection(Triangle triangle)
        {
            if (Math.Abs(Vector3.Dot(_ray.Direction, triangle.Normal)) < Globals.Delta) return;

            var multiplier = (Vector3.Dot(triangle.A, triangle.Normal) -
                          Vector3.Dot(_ray.Origin, triangle.Normal)) /
                         Vector3.Dot(_ray.Direction, triangle.Normal);

            if (multiplier <= 0) return;

            var intersectionPoint = _ray.Origin + _ray.Direction * multiplier;

            var edge0 = triangle.B - triangle.A;
            var edge1 = triangle.C - triangle.B;
            var edge2 = triangle.A - triangle.C;
            var p0 = intersectionPoint - triangle.A;
            var p1 = intersectionPoint - triangle.B;
            var p2 = intersectionPoint - triangle.C;
            var cross0 = Vector3.Cross(edge0, p0);
            var cross1 = Vector3.Cross(edge1, p1);
            var cross2 = Vector3.Cross(edge2, p2);

            //not sure if can use normalized vector perpendicular to triangle here
            if (!(Vector3.Dot(triangle.Normal, cross0) > 0 &&
                  Vector3.Dot(triangle.Normal, cross1) > 0 &&
                  Vector3.Dot(triangle.Normal, cross2) > 0)) return;
            
            CheckDistanceUpdateHit(intersectionPoint, triangle);
        }

        private void CheckDistanceUpdateHit(Vector3 intersectionPoint, GeometryObject objectHit)
        {
            var distance = intersectionPoint.Norm;

            if (!(distance < _minDist) || distance <= 0) return;
            _minDist = distance;
            _hit = intersectionPoint;
            _hitObject = objectHit;
        }
    }
}