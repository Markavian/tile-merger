namespace ImageMerger
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [STAThread]
        private static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            // Check if any arguments were provided
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                WriteLine("");
                WriteLine("Tile Merger");
                // Process the arguments
                foreach (string arg in args)
                {
                    WriteLine("Argument: " + arg);
                }
            }
        }

        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}

