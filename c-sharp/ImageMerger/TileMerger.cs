namespace ImageMerger
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using static System.Net.WebRequestMethods;

    public class TileMerger
    {
        private int mergedImageCount = 0;

        public static Bitmap MergeBitmaps(List<Bitmap> listOfTileBitmaps, int columnCount, bool horizontalTiling)
        {
            int width = 1;
            int height = 1;
            foreach (Bitmap sourceTile in listOfTileBitmaps)
            {
                if (sourceTile.Width > width)
                {
                    width = sourceTile.Width;
                }
                if (sourceTile.Height > height)
                {
                    height = sourceTile.Height;
                }
            }
            if (columnCount > listOfTileBitmaps.Count)
            {
                columnCount = listOfTileBitmaps.Count;
            }
            int rowCount = (int)Math.Ceiling((double)(((double)listOfTileBitmaps.Count) / ((double)columnCount)));
            if (!horizontalTiling)
            {
#pragma warning disable IDE0180 // Use tuple to swap values
                int temp = rowCount;
                rowCount = columnCount;
                columnCount = temp;
#pragma warning restore IDE0180 // Use tuple to swap values
            }
            Bitmap image = new Bitmap(columnCount * width, rowCount * height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0xff, 240, 200));
            graphics.FillRectangle(brush, 0, 0, image.Width, image.Height);
            int n = 0;
            if (horizontalTiling)
            {
                foreach (Bitmap tileBitmap in listOfTileBitmaps)
                {
                    int col = (int)Math.Floor((double)(((double)n) / ((double)columnCount)));
                    int row = n % columnCount;
                    Point location = new Point(((row * width) + (width / 2)) - (tileBitmap.Width / 2), ((col * height) + (height / 2)) - (tileBitmap.Height / 2));
                    Size size = new Size(tileBitmap.Width, tileBitmap.Height);
                    Graphics.FromImage(image).DrawImage(tileBitmap, new Rectangle(location, size));
                    n++;
                }
            }
            else
            {
                foreach (Bitmap tileBitmap in listOfTileBitmaps)
                {
                    int row = (int)Math.Floor((double)(((double)n) / ((double)rowCount)));
                    int col = n % rowCount;
                    Point location = new Point(((row * width) + (width / 2)) - (tileBitmap.Width / 2), ((col * height) + (height / 2)) - (tileBitmap.Height / 2));
                    Size size = new Size(tileBitmap.Width, tileBitmap.Height);
                    Graphics.FromImage(image).DrawImage(tileBitmap, new Rectangle(location, size));
                    n++;
                }
            }
            return image;
        }

        internal static bool ProcessTileMergerArgs(TileMergerArgs tileMergerArgs)
        {
            Bitmap bitmap;
            Console.WriteLine("Processing Tile Merger arguments...");
            if (!Directory.Exists(tileMergerArgs.SourceDirectory))
            {
                Console.WriteLine("⚠️ Folder not found");
                Console.WriteLine("The specified source directory " + tileMergerArgs.SourceDirectory + " does not exist.");
                return false;
            }
            string defaultDestinationPath = "./TiledImages_x" + tileMergerArgs.Columns + "_" + tileMergerArgs.TilingDirection + ".png";
            if (tileMergerArgs.DestinationPath == "")
            {
                Console.WriteLine("⚠️ Operation stopped");
                Console.WriteLine("An invalid target file was set.");
                return false;
            }
            string relativeDir = tileMergerArgs.SourceDirectory;
            if (relativeDir.Length > 0) {
                Directory.SetCurrentDirectory(tileMergerArgs.SourceDirectory);
                relativeDir = "./";
            }
            List<string> sources = tileMergerArgs.ImageList.Count > 0 ? tileMergerArgs.ImageList : ImageLoader.ListFiles(relativeDir, tileMergerArgs.Filter, defaultDestinationPath);
            Console.WriteLine("Attempting to load {0} images from" + String.Join(", ", sources.ToArray()), sources.Count);
            Console.WriteLine("  Whitelist filter: '" + tileMergerArgs.Filter + "'");
            Console.WriteLine("  Blacklist filter: '" + defaultDestinationPath + "'");
            List<Bitmap> bitmaps = ImageLoader.LoadImages(sources);
            if (bitmaps.Count == 0)
            {
                Console.WriteLine("⚠️ Operation stopped");
                Console.WriteLine("No images were found, could not create merged image.");
                return false;
            }
            try
            {
                bitmap = MergeBitmaps(bitmaps, tileMergerArgs.Columns, tileMergerArgs.TilingDirection == TilingDirection.LeftRight);
            }
            catch (Exception exception)
            {
                Console.WriteLine("⚠️ Merge Bitmaps Exception");
                Console.WriteLine(exception.Message);
                ImageLoader.DisposeImages(bitmaps);
                return false;
            }
            try
            {
                string finalDestination = tileMergerArgs.DestinationPath.Length > 0 ? tileMergerArgs.DestinationPath : defaultDestinationPath;
                SaveImage(bitmap, finalDestination);
            }
            catch (Exception exception2)
            {
                Console.WriteLine("⚠️ Save Image Exception");
                Console.WriteLine(exception2.Message);
                return false;
            }
            Console.WriteLine("Merged " + bitmaps.Count + "images.");
            ImageLoader.DisposeImages(bitmaps);
            return true;
        }

        internal bool ProcessDirectoryToFile(Form parentForm, string directory, string fileTarget, int columnCount, string filter, bool horizontalTiling=true)
        {
            Bitmap bitmap;
            this.mergedImageCount = 0;
            if (!Directory.Exists(directory))
            {
                MessageBox.Show(parentForm, "The specified source directory " + directory + " does not exist.", "Folder not found error", MessageBoxButtons.OK);
                return false;
            }
            if (fileTarget == "")
            {
                MessageBox.Show(parentForm, "An invalid target file was set.", "Operation stopped", MessageBoxButtons.OK);
                return false;
            }
            List<Bitmap> bitmaps = ImageLoader.LoadImages(ImageLoader.ListFiles(directory, filter));
            if (bitmaps.Count == 0)
            {
                MessageBox.Show(parentForm, "No images were found, could not create merged image.", "Operation stopped", MessageBoxButtons.OK);
                return false;
            }
            try
            {
                bitmap = MergeBitmaps(bitmaps, columnCount, horizontalTiling);
            }
            catch (Exception exception)
            {
                MessageBox.Show(parentForm, exception.Message, "Merge Bitmaps Exception", MessageBoxButtons.OK);
                ImageLoader.DisposeImages(bitmaps);
                return false;
            }
            try
            {
                SaveImage(bitmap, fileTarget);
            }
            catch (Exception exception2)
            {
                MessageBox.Show(parentForm, exception2.Message, "Save Image Exception", MessageBoxButtons.OK);
                return false;
            }
            this.mergedImageCount = bitmaps.Count;
            ImageLoader.DisposeImages(bitmaps);
            return true;
        }

        public static void SaveImage(Bitmap image, string fileTarget)
        {
            image.Save(fileTarget, ImageFormat.Png);
        }

        public int MergedImageCount
        {
            get
            {
                return this.mergedImageCount;
            }
        }
    }
}

