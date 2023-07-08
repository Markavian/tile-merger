using System;
using System.IO;
using System.Collections.Generic;

namespace ImageMerger
{
    internal class NamedArg
    {
        public string Key { get; }
        public string Value { get; }

        public NamedArg(string arg)
        {
            var parts = arg.Split('=');
            Key = parts[0].ToLower().Replace("--", "");
            Value = parts.Length > 1 ? parts[1] : null;
        }
    }

    internal class TileMergerArgs
    {
        private List<NamedArg> namedArgList;

        private string sourceDirectory;
        private string destinationPath;
        private List<string> imageList;
        private string filter;
        private int columns;
        private TilingDirection tilingDirection;
        private bool showHelp = false;

        public TileMergerArgs(string[] args)
        {
            List<string> argsList = new List<string>(args);
            namedArgList = new List<NamedArg>();

            foreach (string str in argsList)
            {
                NamedArg namedArg = new NamedArg(str);
                namedArgList.Add(namedArg);
            }

            sourceDirectory = Find("src");
            destinationPath = Find("dest");
            string images = Find("imgs") ?? "";
            imageList = images.Length > 0 ? new List<string>(images.Split(',')) : new List<string>();
            filter = Find("filter");
            if (!int.TryParse(Find("cols"), out columns))
            {
                columns = 5;
            }
            tilingDirection = Find("td") == "td"
                ? TilingDirection.TopDown
                : TilingDirection.LeftRight;
            showHelp = Find("help") != null;
        }

        public string SourceDirectory { get => sourceDirectory; set => sourceDirectory = value; }
        public string DestinationPath { get => destinationPath; set => destinationPath = value; }
        public List<string> ImageList { get => imageList; set => imageList = value; }
        public string Filter { get => filter; set => filter = value; }
        public int Columns { get => columns; set => columns = value; }
        public bool ShowHelp { get => showHelp; set => showHelp = value; }
        internal TilingDirection TilingDirection { get => tilingDirection; set => tilingDirection = value; }

        private string Find(string paramKey)
        {
            NamedArg namedArg = namedArgList.Find((arg) => arg.Key == paramKey);
            if (namedArg != null)
            {
                return namedArg.Value ?? namedArg.Key;
            }
            return null;
        }

        private string NonEmptyString(string value, string defaultValue = "not-set")
        {
            return (value == string.Empty || value == null) ? defaultValue : value;
        }


        override public string ToString()
        {
            List<string> lines = new List<string>
            {
                "Tile Merger args:",
                "  Source directory:       " + NonEmptyString(SourceDirectory, Directory.GetCurrentDirectory()),
                "  Destination path:       " + NonEmptyString(DestinationPath, Directory.GetCurrentDirectory()),
                "  Image list:             " + NonEmptyString(string.Join(", ", ImageList.ToArray())),
                "  Filter:                 " + NonEmptyString(Filter),
                "  Columns:                " + Columns.ToString(),
                "  Tiling direction:       " + TilingDirection.ToString(),
                "  Show help:              " + ShowHelp.ToString()
            };
            return string.Join("\n", lines.ToArray());
        }
    }
}