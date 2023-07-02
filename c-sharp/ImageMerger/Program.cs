namespace ImageMerger
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [STAThread]
        private static void Main(string[] args)
        {
            // Check if any arguments were provided
            if (args.Length == 0)
            {
                WriteLine("No arguments provided.");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                AllocConsole();
                WriteLine("Hello world!");
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

