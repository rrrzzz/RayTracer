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

        public Color FindColor()
        {
//            if (_hitObject != null)
//            {
//                return Color.Aqua;   
//            }
//            return Color.Black;
            
//            if (_hitObject == null) return Color.Black;
//            _visibleLights = Globals.Lights;
            
            if (_hitObject == null || !IsRayVisible()) return Color.Black;

            var pixelColor = _hitObject.ObjProperties.Ambient + _hitObject.ObjProperties.Emission;
            
            foreach (var light in _visibleLights)
            {
                pixelColor += CalculateColorFromLight(light);
            }

            var rgb = pixelColor.ToArray().Select(x => (int)x.Map(0, 1, 0, 255)).ToArray();
            return Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
        }

        private Vector3 CalculateColorFromLight(Light light)
        {
            var objectNormal = _hitObject.Normal;
            var objectProps = _hitObject.ObjProperties;
            var dirToLight = GetDirectionToLight(light);
            
            var eyeDir = (_ray.Origin - _hitPoint).Normalization();
            //var eyeDir = (new Vector3(0, 0, 0) - _hitPoint).Normalization();
            
            
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
       

        //Is this correct? Ambient light may affect the scene even if there are no point lights on object
        private bool IsRayVisible()
        {
            if (Globals.Lights.Count == 0) return true;
            
            //move hitPoint closer lo light a little before checking visibility
            var isVisible = false;
            
            foreach (var light in Globals.Lights)
            {
                var directionToLight = GetDirectionToLight(light);

                var hitPoint = _hitPoint + directionToLight / 1000; 
                
                var distanceToLight = GetDistanceToLight(light, hitPoint);
                    
                var rayToLight = new Ray(hitPoint, directionToLight);
                
                var intersection = new Intersection(rayToLight);

                if (intersection.IsLightObstructed(distanceToLight)) continue;

                isVisible = true;
                _visibleLights.Add(light);
            }

            return isVisible;
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