namespace ImageMerger
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public class ImageLoader
    {
        private ImageLoader()
        {
            throw new Exception("No need to create an object of this class, all its methods are static.");
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
            Console.WriteLine("Found {0} files in " + folderPath, files.Length);
            Array.Sort<string>(files);
            List<string> list = new List<string>();
            foreach (string str in files)
            {
                if (str.ToLower().Contains(blacklistFilter))
                {
                    break;
                }
                if (whitelistFilter != null && whitelistFilter.Length > 0 && !str.ToLower().Contains(whitelistFilter)) {
                    break;
                }
                list.Add(str);
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

