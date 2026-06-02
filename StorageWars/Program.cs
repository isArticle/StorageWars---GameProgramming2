using System;
using System.Runtime.InteropServices;

namespace StorageWars
{
    public static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware(); // DPI Scaling

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessDPIAware();
            }

            using var game = new Game1();
            game.Run();
        }
    }
}