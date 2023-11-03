using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageMerger
{
    internal class CommandLineInterface
    {
        private string[] args;

        public CommandLineInterface(string[] args)
        {
            this.args = args;
        }

        public void DisplayHelp()
        {
            var lines = new[] {
                "",
                "Tile Merger 1.1.0",
                "Supported arguments:",
                "",
                "--src=\"<path>\"            Source folder to find files in; also used as base directory to find relative imgs",
                "--imgs=\"f1.png,f2.png\"    Comma separated list of files, works in tandem with src; file list will still be filtered",
                "--dest=\"<path>\"           Destination file path to output to, defaults to ./ TiledImages_x{ cols}_{ td | lr}.png",
                "--filter=\"string\"         Filter string, inclusive match",
                "--cols=6                  Number of columns before wrapping",
                "--td=lr|tb                Tiling Direction - left-right (lr) or (top-bottom) (tb)",
                "--help                    Show help and version info",
                "",
                "Website: https://github.com/Markavian/tile-merger"
            };

            WriteLines(lines);
        }

        internal void ProcessArgs()
        {
            TileMergerArgs tileMergerArgs = new TileMergerArgs(this.args);
            WriteLine("");
            WriteLine("Processed args:");
            WriteLine(tileMergerArgs.ToString());

            if (tileMergerArgs.ShowHelp)
            {
                DisplayHelp();
            } else
            {
                TileMerger.ProcessTileMergerArgs(tileMergerArgs);
            }
            WriteLine("Done!");
        }

        private void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        private void WriteLines(string[] lines)
        {
            foreach (string line in lines) {
                WriteLine(line);
            }
        }
    }
}
