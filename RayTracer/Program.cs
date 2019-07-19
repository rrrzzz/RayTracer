using System;
using System.Collections.Generic;
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
            
            Ray.InitializeCoordinateFrame();
            //CreateSeparateTriangles();
            CreateImage();
        }

        private static void CreateSeparateTriangles()
        {
            var triangles = Globals.Triangles;
            for (int i = 0; i < triangles.Count; i+=2)
            {
                Triangles = new List<Triangle>();
                Triangles.Add(triangles[i]);
                Triangles.Add(triangles[i+1]);
                CreateImage();
            }
        }
        
        private static void CreateImage()
        {
            var p = new Bitmap(ImageWidth,ImageHeight);

            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
                    Console.WriteLine($"Current x pixel: {i}. Current y pixel: {j}.");
                    var ray = new Ray(i + 0.5f, j + 0.5f);
                    var intersection = new Intersection(ray);
                    var intersectionInfo = intersection.FindClosestIntersection();

                    var colorFinder = new ColorFinder(ray, intersectionInfo);
                    var color = colorFinder.FindColor();
                    
                    p.SetPixel(i, j, color);
                }
            }
            //p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
            p.Save(GetFileNameGen(), ImageFormat.Png);
        }

        private static string GetFileNameGen()
        {
            var filename = @"D:\Docs\Courses\Computer graphics edx\img.png";
            var postfix = 0;
            while (File.Exists(filename))
            {
                filename = $@"D:\Docs\Courses\Computer graphics edx\img{postfix++}.png";
            }

            return filename;
        }
    }
}