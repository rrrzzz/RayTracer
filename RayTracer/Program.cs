using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using static RayTracer.Globals;

namespace RayTracer
{
    internal class Program
    {
        private const string DefaultPath = @"D:\Docs\Courses\Computer graphics edx\testscenes\mytest.txt";
        
        public static void Main(string[] args)
        {
            const string testFilePath = DefaultPath;
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

                    var colorFinder = new ColorFinder(ray, intersectionInfo);
                    var color = colorFinder.FindColor();
                    
                    p.SetPixel(i, j, color);
                }
            }

            var postfix = 0;
            var filename = @"D:\Docs\Courses\Computer graphics edx\img.png";
            while (File.Exists(filename))
            {
                filename = $@"D:\Docs\Courses\Computer graphics edx\img{postfix++}.png";
            }
            p.Save(filename, ImageFormat.Png);
        }
    }
}