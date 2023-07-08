Tile Merger
===========

The tile merger is a GUI application that takes a folder full of image files and mergers them into a single tiled image. Use the columns field to set the number of tiles per row. The image size is automatically calculated based on the source images. Images are sorted by their filename. I suggest using a numeric prefix naming scheme such as "001 tree.png", "002 table.png", "003 chair.png" etc. in order to predictably position images.

Latest Release
--------------
Download [TileMerger_rel_c1.zip](https://cdn.rawgit.com/Markavian/tile-merger/bb84593c5712c202e2c602a40f13bf8234361b4b/c-sharp/releases/TileMerger_rel_c1.zip) (34Kb)

* **Requires .NET 2.0 to run**

### Installation
* Open and extract the files from the zip
* Run `TileMerger.exe`

### How to use

![image](./c-sharp/build/2011-11-29%20Tile%20Merger%20preview%20c1.png)

* **Source directory** - browse to a folder full of images that you want to merge
* **Target file** - name the file you want to create after merging the images
* **Number of Rows / Columns** - the number of rows or columns to fill before wrapping - this changes depending on the **Tiling Direction**
* **Filename filter** - a pattern to match against, filtering only the files that match the filter. Leave blank for all files.
* **Tiling Direction** 
  * Left to Right - Fill a row from left to right before moving down
  * Top to Bottom - Fill a column from top to bottom before moving right
* **Remember these settings** - remembers all the settings in the form
* **Process Images** - Attempts to merge the supplied images and save to the `Target file` path
* **Quit** - Closes the application


### Command Line Mode

New as of July 2023, you can use TilerMerger from the command line.

```
>TileMerger.exe --help
Tile Merger 1.0
Supported arguments:

--src="<path>"          Source folder to find files in
--imgs="f1.png,f2.png"  Comma separated list of files, overrides src, will still be filtered
--dest="<path>"         Destination file path to output to, defaults to ./TiledImages_x{cols}_{td|lr}.png
--filter="string"       Filter string, inclusive match
--cols=6                Number of columns before wrapping
--td=lr|tb              Tiling Direction
--help                  Show help and version info

Website: https://github.com/Markavian/tile-merger
```

# Local Test Command

Example of using a dev build from the `./c-sharp/` folder:

```
.\bin\Release\TileMerger.exe --src="./sample-images/" --dest="sample-outputs/bananas.png" --filter="banana" --cols="3" --td="tb"
```

Version history
---------------

### Release c1 
* Added Tiling Direction - Left to Right (Original Mode), Top to Bottom (New)
* Switch between (Number of Columns) and (Number of Rows)

### Release b1
* Added filter text box
* Added a tooltips to number of columns and filter boxes
* Asks for file target if Not set
* Added process label to display number of files processed
* Fixed series of crashing bugs
* Filters out non-image files when processed

### Release a1
* Initial release
* Supports:
  * Source folder
  * Target file
  * Number of columns
  * Remember settings
* Supports alpha channels. Tested on Windows XP.

Credits
-------
Written by John Beech
* http://mkv25.net/

Used on:
* http://mkv25.net/dfma/

Contact
-------
* csharp@mkv25.net
* github@mkv25.net

Please create an issue or send feedback here on github!

### Available at:
* https://github.com/Markavian/tile-merger

### Older versions:
* http://mkv25.net/showcase/


