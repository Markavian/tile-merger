namespace ImageMerger
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;

    public class ImageLoader
    {
        private ImageLoader()
        {
            throw new Exception("No need to create an object of this class, all its methods are static.");
        }

        public static void NumericalSort(string[] ar)
        {
            Regex rgx = new Regex("([^0-9]*)([0-9]+)");
            Array.Sort(ar, (a, b) =>
            {
                var ma = rgx.Matches(a);
                var mb = rgx.Matches(b);
                for (int i = 0; i < ma.Count; ++i)
                {
                    int ret = ma[i].Groups[1].Value.CompareTo(mb[i].Groups[1].Value);
                    if (ret != 0)
                        return ret;

                    ret = int.Parse(ma[i].Groups[2].Value) - int.Parse(mb[i].Groups[2].Value);
                    if (ret != 0)
                        return ret;
                }

                return 0;
            });
        }

        public static void DisposeImages(List<Bitmap> images)
        {
            foreach (Bitmap bitmap in images)
            {
                bitmap.Dispose();
            }
        }

        public static List<string> ListFiles(string folderPath, string whitelistFilter, string blacklistFilter="")
        {
            string[] files = Directory.GetFiles(folderPath);
            NumericalSort(files);
            Console.WriteLine("Found {0} files in " + folderPath, files.Length);
            // Console.WriteLine(String.Join(", ", files));
            List<string> list = new List<string>();
            foreach (string filePath in files)
            {
                if (blacklistFilter != null && blacklistFilter.Length > 0 && filePath.ToLower().Contains(blacklistFilter.ToLower()))
                {
                    continue;
                }
                if (whitelistFilter != null && whitelistFilter.Length > 0 && !filePath.ToLower().Contains(whitelistFilter.ToLower())) {
                    continue;
                }
                list.Add(filePath);
            }
            return list;
        }

        public static List<Bitmap> LoadImages(List<string> sortedFiles)
        {
            List<Bitmap> list = new List<Bitmap>();
            foreach (string filePath in sortedFiles)
            {
                string fullPath = Path.GetFullPath(filePath);
                try
                {
                    Bitmap item = new Bitmap(fullPath);
                    list.Add(item);
                    Console.WriteLine("  {0} loaded.", filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("  {0} could not be loaded: {1} {2}", fullPath, e.Message, e.StackTrace);
                }
            }
            return list;
        }
    }
}

