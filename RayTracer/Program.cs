using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Accord.Math;
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
            //CreateDragon();
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
            //404 264
            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
//                    if (i == 539 && j == 620)
//                    {
//                        Console.WriteLine("Gotcha!");
//                    }
                    
                    var ray = new Ray(i + 0.5f, j + 0.5f);
                    var intersection = new Intersection(ray);
                    var intersectionInfo = intersection.FindClosestIntersection();
                    var colorFinder = new ColorFinder(ray, intersectionInfo,0);
                    var pixelColor = colorFinder.FindColor();
                    var rgb = pixelColor.ToArray().Select(x => (int)x.Map(0, 1, 0, 255)).ToArray();

                    p.SetPixel(i, j, Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));
                }
            }
            //p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
            p.Save(GetFileNameGen(), ImageFormat.Png);
        }
        
        private static void CreateDragon()
        {
            var p = new Bitmap(ImageWidth,ImageHeight);
            
            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
                    var rgb = GetColor(i, j);

                    p.SetPixel(i, j, Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));
                }
            }
            //p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
            p.Save(GetFileNameGen(), ImageFormat.Png);
        }

        private static int[] GetColor(int i, int j)
        {
            var rgb = new []{0,0,0};
            for (int k = 0; k < 8; k++)
            {
                for (int z = 0; z < 8; z++)
                {
                    var ray = new Ray(i + 0.125f * k, j + 0.125f * z);
                    var intersection = new Intersection(ray);
                    var intersectionInfo = intersection.FindClosestIntersection();
                    var colorFinder = new ColorFinder(ray, intersectionInfo,0);
                    var pixelColor = colorFinder.FindColor();

                    if (!(pixelColor.Norm > Globals.Delta)) continue;
                    rgb = pixelColor.ToArray().Select(x => (int)x.Map(0, 1, 0, 255)).ToArray();
                    return rgb;
                }
            }

            return rgb;
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