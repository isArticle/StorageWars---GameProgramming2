using System;
using System.Runtime.InteropServices;

namespace StorageWars
{
    public static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware(); // Windows'un oyunu zorla büyütmesini (DPI Scaling) engelleyen sihirli komut

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