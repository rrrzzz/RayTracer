using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Accord.Math;
using RayTracer.GeometryObjects;

namespace RayTracer
{
    public class ColorFinder
    {
        public static int RecursionLevel { get; set; }
        private readonly Ray _ray;
        private readonly GeometryObject _hitObject;
        private readonly Vector3 _hitPoint;
        private List<Light> _visibleLights = new List<Light>();

        public ColorFinder(Ray ray, IntersectionInfo info)
        {
            _ray = ray;
            _hitPoint = info.HitPoint;
            _hitObject = info.HitObject;
        }

        public Vector3 FindColor()
        {
            if (_hitObject == null) return new Vector3(0);
            GetVisibleLights();

            var pixelColor = _hitObject.ObjProperties.Ambient + _hitObject.ObjProperties.Emission; 
            if (_hitObject.ObjProperties.Specular.Norm > 0 && RecursionLevel < Globals.RecursionMaxDepth)
            {
                pixelColor += _hitObject.ObjProperties.Specular * FindRecursiveIntensity();
            }
            foreach (var light in _visibleLights)
            {
                pixelColor += CalculateColorFromLight(light);
            }

            return pixelColor;
        }

        private Vector3 FindRecursiveIntensity()
        {
            RecursionLevel++;
            var reflectedRay = GetReflectedRay();
            var intersection = new Intersection(reflectedRay);
            var intersectionInfo = intersection.FindClosestIntersection();
            var colorFinder = new ColorFinder(reflectedRay, intersectionInfo);
            return colorFinder.FindColor();
        }

        private Ray GetReflectedRay()
        {
            var reflectedDirection = _ray.Direction - _hitObject.Normal * 2 * Vector3.Dot(_ray.Direction, _hitObject.Normal);
            
            return new Ray(_hitPoint + _hitObject.Normal / 1000, reflectedDirection);
        }

        private Vector3 CalculateColorFromLight(Light light)
        {
            var objectNormal = _hitObject.Normal;
            var objectProps = _hitObject.ObjProperties;
            var dirToLight = GetDirectionToLight(light);
            
            var eyeDir = (_ray.Origin - _hitPoint).Normalization();
            
            var halfAngle = (eyeDir + dirToLight).Normalization();

            var lambert = objectProps.Diffuse * light.Color * Math.Max(Vector3.Dot(objectNormal, dirToLight), 0);
            var phong = objectProps.Specular * light.Color * 
                        (float)Math.Pow(Math.Max(Vector3.Dot(objectNormal, halfAngle), 0), objectProps.Shininess);

            var attenuation = GetAttenuation(light);
            return (lambert + phong) * attenuation;
        }

        private float GetAttenuation(Light light)
        {
            var d = GetDistanceToLight(light, _hitPoint);
            var c0 = light.Attenuation[0];
            var c1 = light.Attenuation[1];
            var c2 = light.Attenuation[2];

            return 1 / (c0 + c1 * d + c2 * d * d);
        }
       
        private void GetVisibleLights()
        {
            foreach (var light in Globals.Lights)
            {
                var directionToLight = GetDirectionToLight(light);

                var hitPoint = _hitPoint + _hitObject.Normal / 1000; 
                
                var distanceToLight = GetDistanceToLight(light, hitPoint);
                    
                var rayToLight = new Ray(hitPoint, directionToLight);
                
                var intersection = new Intersection(rayToLight);

                if (intersection.IsLightObstructed(distanceToLight)) continue;

                _visibleLights.Add(light);
            }
        }
        
        private Vector3 GetDirectionToLight(Light light)
        {
            var lightCoords = Transform.ConvertToVector3(light.Coordinates);
            
            if (Math.Abs(light.Coordinates.W) < Globals.Delta)
            {
                lightCoords.Normalize();
                return lightCoords;
            }
            
            var directionToLight = lightCoords - _hitPoint;
            directionToLight.Normalize();
            return directionToLight;
        }

        private float GetDistanceToLight(Light light, Vector3 hitPoint)
        {
            if (Math.Abs(light.Coordinates.W) < Globals.Delta) return float.MaxValue;
            var vectorToLight = Transform.ConvertToVector3(light.Coordinates) - hitPoint;
            return vectorToLight.Norm;
        }
    }
}