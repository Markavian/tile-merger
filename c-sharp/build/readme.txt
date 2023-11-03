Tile Merger - Updated July 2023
Release 1.1.0

Requires .NET 2.0 to run

The tile merger is a GUI application that takes a folder full of image files and mergers them into a single tiled image. Use the columns field to set the number of tiles per row. The image size is automatically calculated based on the source images. Images are sorted by their filename. I suggest using a numeric prefix naming scheme such as "001 tree.png", "002 table.png", "003 chair.png" etc. in order to predictably position images.

Version history:

Release 1.1.0

  o Merged XtheOne's Numeric Sorting branch
  o Example Key files F1 to F16 now are ordered correctly:
    o F1 -> F9, F10, F11, instead of F1, F10, F11, F2 -> F9

Release 1.0.0
  o Added command line arguments
  o Fully migrated to Github: https://github.com/Markavian/tile-merger
  o Fixed bug with tiling direction to work as intended

Tile Merger 1.0
Supported arguments:

--src="<path>"            Source folder to find files in; also used as base directory to find relative imgs
--imgs="f1.png,f2.png"    Comma separated list of files, works in tandem with src; file list will still be filtered
--dest="<path>"           Destination file path to output to, defaults to ./ TiledImages_x{ cols}_{ td | lr}.png
--filter="string"         Filter string, inclusive match
--cols=6                  Number of columns before wrapping
--td=lr|tb                Tiling Direction - left-right (lr) or (top-bottom) (tb)
--help                    Show help and version info

Website: https://github.com/Markavian/tile-merger


Release c1 (November 2008)
  o Added Tiling Direction - Left to Right (Original Mode), Top to Bottom (New)
  o Switch between (Number of Columns) and (Number of Rows)

Release b1 (August 2008)
  o Added filter text box
  o Added a tooltips to number of columns and filter boxes
  o Asks for file target if Not set
  o Added process label to display number of files processed
  o Fixed series of crashing bugs
  o Filters out non-image files when processed

Release a1 (2008)
 Initial release
 Supports
  o Source folder
  o Target file
  o Number of columns
  o Remember settings

Supports alpha channels. Tested on Windows XP, and Windows 11.

Written by John Beech
https://mkv25.net/
https://github.com/johnbeech/

With contributions and thanks to XtheOne:
- https://github.com/XtheOne

Used on:
https://mkv25.net/dfma/

Contact
  o csharp@mkv25.net
  o github@mkv25.net

Or raise issue at https://github.com/Markavian/tile-merger/issues

Historically available at:
https://mkv25.net/showcase/

