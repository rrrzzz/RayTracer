using System.IO;
using System.Net;

namespace RayTracer
{
    public static class Parser
    {
        public static void ReadFile(string path)
        {
            var contents = File.ReadLines(path);
            string[] splitLine;
            
            foreach (var line in contents)
            {
                if (line.StartsWith("#") || line == string.Empty) continue;
                else splitLine = line.Split(' ');
                var cmd = splitLine[0];

                switch (cmd)
                {
                    case "size":
                        var x = 5;
                        break;
                        
                }
                


            }
        }
    }
}