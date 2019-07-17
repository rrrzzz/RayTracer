using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Math;
using static RayTracer.Globals;

namespace RayTracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var testFilePath = @"D:\Docs\Courses\Computer graphics edx\testscenes\scene1.test";
            var parser = new Parser();
            parser.ReadFile(testFilePath);
            CreateImage();
        }

        private static void CreateImage()
        {
            Ray.InitializeCoordinateFrame();
            var p = new Bitmap(ImageWidth,ImageHeight);

            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
                    var ray = new Ray(i + 0.5f, j + 0.5f);
                    var intersection = new Intersection(ray);
                    var intersectionInfo = intersection.FindClosestIntersection();
                    if (intersectionInfo.HitObject != null)
                    {
                        Console.WriteLine("Found object!");
                    }
                    var colorFinder = new ColorFinder(ray, intersectionInfo);
                    var color = colorFinder.FindColor();
                    
                    p.SetPixel(i, j, color);
                }
            }
            
            p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
        }
    }
}