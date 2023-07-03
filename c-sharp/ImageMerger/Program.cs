using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageMerger
{
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
                var cli = new CommandLineInterface(args);
                cli.ProcessArgs();
            }
        }
    }
}

