using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Math;

namespace RayTracer
{
    public class ColorFinder
    {
        private Ray _ray;
        private GeometryObject _hitObject;
        private Vector3 _hitPoint;
        private List<Light> _visibleLights;

        public ColorFinder(Ray ray, IntersectionInfo info)
        {
            _ray = ray;
            _hitPoint = info.HitPoint;
            _hitObject = info.HitObject;
        }

        public Color FindColor()
        {
            //color ranges from 0 to 1, need to map to 255
            if (_hitObject == null || !IsRayVisible()) return Color.Black;

            var pixelColor = _hitObject.ObjProperties.Ambient + _hitObject.ObjProperties.Emission;
            
            foreach (var visibleLight in _visibleLights)
            {
                pixelColor += CalculateColorFromLight(visibleLight);
            }

            var rgb = pixelColor.ToArray().Select(x => (int)x.Map(0, 1, 0, 255)).ToArray();
            return Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
        }

        private Vector3 CalculateColorFromLight(Light visibleLight)
        {
            throw new System.NotImplementedException();
        }

        private bool IsRayVisible()
        {
            var isVisible = false;
            
            //need to process directional light here somehow
            foreach (var light in Globals.Lights)
            {
                var lightPos = Helpers.ConvertToVector3(light.Coordinates);
                var vectorToLight = lightPos - _hitPoint;
                var distanceToLight = vectorToLight.Norm;
                var directionToLight = vectorToLight / distanceToLight; 
                
                var rayToLight = new Ray(_hitPoint, directionToLight);
                
                var intersection = new Intersection(rayToLight);

                if (intersection.IsLightObstructed(distanceToLight)) continue;

                isVisible = true;
                _visibleLights.Add(light);
            }

            return isVisible;
        }
    }
}