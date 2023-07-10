namespace ImageMerger
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

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
            int rowCount = (int)Math.Ceiling((double)(listOfTileBitmaps.Count / ((double)columnCount)));
            Bitmap image = new Bitmap(columnCount * width, rowCount * height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0xff, 240, 200));
            graphics.FillRectangle(brush, 0, 0, image.Width, image.Height);
            int n = 0;
            if (horizontalTiling)
            {
                foreach (Bitmap tileBitmap in listOfTileBitmaps)
                {
                    int col = (int)Math.Floor((double)(n / ((double)columnCount)));
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
                    int row = (int)Math.Floor((double)(n / ((double)rowCount)));
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
            string relativeDir = TileMergerArgs.NonEmptyString(tileMergerArgs.SourceDirectory, Directory.GetCurrentDirectory());
            if (!Directory.Exists(relativeDir))
            {
                Console.WriteLine("⚠️ Folder not found");
                Console.WriteLine("The specified source directory (" + relativeDir + ") does not exist.");
                return false;
            }
            string defaultDestinationPath = "TiledImages_x" + tileMergerArgs.Columns + "_" + tileMergerArgs.TilingDirection.Value + ".png";
            if (tileMergerArgs.DestinationPath == "")
            {
                Console.WriteLine("⚠️ Operation stopped");
                Console.WriteLine("An invalid target file was set.");
                return false;
            }
            string startingDir = Directory.GetCurrentDirectory();
            if (relativeDir.Length > 0) {
                Directory.SetCurrentDirectory(relativeDir);
            }
            List<string> sources = tileMergerArgs.ImageList.Count > 0 ? tileMergerArgs.ImageList : ImageLoader.ListFiles(Directory.GetCurrentDirectory(), tileMergerArgs.Filter, defaultDestinationPath);
            Console.WriteLine("Attempting to load {0} images...", sources.Count);
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
                Console.WriteLine("🎨 Tiling direction: {0}, Bitmaps: {1}, Columns: {2}", tileMergerArgs.TilingDirection.Value, bitmaps.Count, tileMergerArgs.Columns);
                bitmap = MergeBitmaps(bitmaps, tileMergerArgs.Columns, tileMergerArgs.TilingDirection == TilingDirection.LeftRight);
            }
            catch (Exception mergeImagesException)
            {
                Console.WriteLine("⚠️ Merge Bitmaps Exception");
                Console.WriteLine(mergeImagesException.Message);
                ImageLoader.DisposeImages(bitmaps);
                return false;
            }
            try
            {
                string destinationPath = TileMergerArgs.NonEmptyString(tileMergerArgs.DestinationPath, Directory.GetCurrentDirectory());
                Console.WriteLine("Destination path: {0}", destinationPath);
                string finalDestination = (tileMergerArgs.DestinationPath != null && tileMergerArgs.DestinationPath.Length > 0) ? tileMergerArgs.DestinationPath : Path.GetFullPath(Path.Combine(destinationPath, defaultDestinationPath));
                Console.WriteLine("Created Image " + bitmap.Width + "x" + bitmap.Height + "px, Destination: " + finalDestination);
                Directory.SetCurrentDirectory(startingDir);
                SaveImage(bitmap, finalDestination);
                Console.WriteLine("Merged " + bitmaps.Count + " images; saved to: " + finalDestination);
            }
            catch (Exception saveImageException)
            {
                Console.WriteLine("⚠️ Save Image Exception");
                Console.WriteLine(saveImageException.Message);
                Console.WriteLine(saveImageException.StackTrace);
                ImageLoader.DisposeImages(bitmaps);
                return false;
            }
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
            List<string> files = ImageLoader.ListFiles(directory, filter);
            List<Bitmap> bitmaps = ImageLoader.LoadImages(files);
            if (bitmaps.Count == 0)
            {
                MessageBox.Show(parentForm, "No images were found, could not create merged image. Filter: " + filter + " Directory: " + directory, "Operation stopped", MessageBoxButtons.OK);
                MessageBox.Show(parentForm, "Files in folder:\n" + String.Join("\n", files.ToArray()), "Files in folder", MessageBoxButtons.OK);
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

