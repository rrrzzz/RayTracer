using System;
using Accord.Math;

namespace RayTracer
{
    public class Intersection
    {
        private Ray _ray;
        private Vector3 _hit;
        private float _minDist = float.MaxValue;
        
        public Intersection(Ray ray)
        {
            _ray = ray;
        }
        
        private void FindSphereIntersection(Sphere sphere)
        {
            float multiplier;
            Vector3 hit;

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
            if (Math.Abs(discriminant) < 0.01) multiplier = -b / 2*a;
            else
            {
                var mult1 = (-b + Math.Sqrt(discriminant)) / 2 * a;
                var mult2 = (-b - Math.Sqrt(discriminant)) / 2 * a;

                multiplier = (float)Math.Min(mult1, mult2);
                multiplier = multiplier > 0 ? multiplier : (float)Math.Max(mult1, mult2);
            }

            var intersectionVector = rayOriginTrans + rayDirectionTrans * multiplier;
            
            intersectionVector = Helpers.TransformVector(intersectionVector, sphere.Transform);

            var distance = (_ray.Origin - intersectionVector).Norm;

            if (distance < _minDist)
            {
                _minDist = distance;
                _hit = intersectionVector;
            }
            _minDist = distance < _minDist ? distance : _minDist;
        }
        
        private void FindTriangleIntersection(Triangle triangle)
        {
            
        }
    }
}