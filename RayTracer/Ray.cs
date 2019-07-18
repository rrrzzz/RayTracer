using System;
using Accord.Math;
using static RayTracer.Globals;

namespace RayTracer
{
    public class Ray
    {
        private static Vector3 _uAxis;
        private static Vector3 _vAxis;
        private static Vector3 _wAxis;
        
        public Ray(float pixelX, float pixelY)
        {
            GetDirection(pixelX, pixelY);
            Origin = EyeInit;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
        
        public Vector3 Direction { get; private set; }
        public Vector3 Origin { get; }
        
        public static void InitializeCoordinateFrame()
        {
            var axes = Transform.ConstructCoordinateFrame();
            _uAxis = axes[0];
            _vAxis = axes[1];
            _wAxis = axes[2];
        }

        private void GetDirection(float pixelX, float pixelY)
        {
            var halfImageHeight = ImageHeight * .5;
            var halfImageWidth = ImageWidth * .5;
            
            var xDirectionMultiplier = (float)(Math.Tan(FovXRad * .5) * ((pixelX - halfImageWidth) / halfImageWidth));
            var yDirectionMultiplier = (float)(Math.Tan(FovYRad * .5) * ((halfImageHeight - pixelY) / halfImageHeight));

            Direction = (_uAxis * xDirectionMultiplier +
                         _vAxis * yDirectionMultiplier - _wAxis).Normalization();
        }
    }
}