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
                if (_minDist < distanceToLight) return true;
            }
            
            foreach (var triangle in Globals.Triangles)
            {
                FindTriangleIntersection(triangle);
                if (_minDist < distanceToLight) return true;
            }

            return false;
        }

        private void FindSphereIntersection(Sphere sphere)
        {
            var rayOrigin = Transform.TransformVector(_ray.Origin, sphere.InverseTransformMat);
            
            var rayDir = Transform.TransformZeroWVector(_ray.Direction, sphere.InverseTransformMat);
            
            var a = Vector3.Dot(rayDir, rayDir);
            var b = 2 * Vector3.Dot(rayDir, rayOrigin - sphere.OriginalPos);
            var c = Vector3.Dot(rayOrigin - sphere.OriginalPos, rayOrigin - sphere.OriginalPos) -
                    sphere.Radius * sphere.Radius;
            
            var multiplier = GetSmallestPositiveQuadraticRoot(a, b, c);
            if (multiplier <= 0) return;

            var intersectionPoint = rayOrigin + rayDir * multiplier;

            var transpose = Transform.TransposeMat4X4(sphere.InverseTransformMat);
            
            sphere.Normal = intersectionPoint - sphere.OriginalPos;
            sphere.Normal = Transform.TransformZeroWVector(sphere.Normal, transpose);
            sphere.Normal = sphere.Normal.Normalization();

            intersectionPoint = Transform.TransformVector(intersectionPoint, sphere.TransformMat);
            
            CheckDistanceUpdateHit(intersectionPoint, sphere);
        }
        
        private float GetSmallestPositiveQuadraticRoot(float a, float b, float c)
        {
            float root;
            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0) return -1;
            
            if (Math.Abs(discriminant) < Globals.Delta) root = -b / (2*a);
            else
            {
                var mult1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                var mult2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

                root = (float)Math.Min(mult1, mult2);
                root = root > 0 ? root : (float)Math.Max(mult1, mult2);
            }

            return root;
        }

        private void FindTriangleIntersection(Triangle triangle)
        {
            var rayVector = _ray.Direction;
            var rayOrigin = _ray.Origin;
            var vertex0 = triangle.A;
            var vertex1 = triangle.B;
            var vertex2 = triangle.C;
            
            var edge1 = vertex1 - vertex0;
            var edge2 = vertex2 - vertex0;
            var h = Vector3.Cross(rayVector, edge2);
            var a = Vector3.Dot(edge1,h);
            
            if (a > -Globals.Delta && a < Globals.Delta) {
                return;    // This ray is parallel to this triangle.
            }
            var f = 1.0 / a;
            var s = rayOrigin - vertex0;
            var u = f * Vector3.Dot(s,h);
            if (u < 0.0 || u > 1.0) {
                return;
            }
            
            var q = Vector3.Cross(s, edge1);
            var v = f * Vector3.Dot(rayVector,q);
            if (v < 0.0 || u + v > 1.0) {
                return;
            }
            // At this stage we can compute t to find out where the intersection point is on the line.
            
            var t = (float)(f * Vector3.Dot(edge2, q));
            
            if (!(t > Globals.Delta)) return;
            var intersectionPoint = rayOrigin + rayVector * t;
            CheckDistanceUpdateHit(intersectionPoint, triangle);
        }
        
        private void FindTriangleIntersectionEdges(Triangle triangle)
        {
            var a = triangle.B - triangle.A;
            var b = triangle.C - triangle.A;

            var n = Vector3.Cross(a, b);
            var nDot = Vector3.Dot(n, triangle.A);

            var res = Vector3.Dot(n, _ray.Origin);

            var res2 = Vector3.Dot(n, new Vector3(0, 0, 0));
            
            var triVertMinusRayOrigin = triangle.A - _ray.Origin;
            var rayDirDotTriNormal = Vector3.Dot(_ray.Direction, triangle.Normal);
            
            if (Math.Abs(rayDirDotTriNormal) < Globals.Delta) return;

            var multiplier = Vector3.Dot(triVertMinusRayOrigin, triangle.Normal) / 
                             rayDirDotTriNormal;

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
            var distance = (intersectionPoint - _ray.Origin).Norm;

            if (!(distance < _minDist) || distance <= 0) return;
            _minDist = distance;
            _hit = intersectionPoint;
            _hitObject = objectHit;
        }
    }
}