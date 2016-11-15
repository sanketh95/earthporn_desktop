using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EpApp.Classes
{
    public interface IWallpaperSetter
    {
        void SetWallpaper(string fileName);
    }

    public class WindowsWallpaperImageSetter : IWallpaperSetter
    {
        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;

        public void SetWallpaper(string fileName)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fileName, SPIF_UPDATEINIFILE);
        }
    }

    public class MacOSWallpaperImageSetter : IWallpaperSetter
    {
        public void SetWallpaper(string fileName)
        {
            var command = "osascript";
            var args = $"-e \"tell application \\\"Finder\\\" to set desktop picture to POSIX file \\\"{fileName}\\\"\"";
            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = command;
            ExternalProcess.StartInfo.Arguments = args;
            ExternalProcess.StartInfo.CreateNoWindow = true;
            ExternalProcess.Start();
            ExternalProcess.WaitForExit();
        }
    }

    public class LinuxGnomeWallpaperImageSetter : IWallpaperSetter
    {
        [DllImport("libgio-2.0.so")]
        private static extern IntPtr g_settings_new(string path);

        [DllImport("libgio-2.0.so")]
        private static extern string g_settings_get_string(IntPtr settings, string key);

        [DllImport("libgio-2.0.so")]
        private static extern bool g_settings_set_string(IntPtr settings, string key, StringBuilder value);

        public void SetWallpaper(string fileName) 
        {
            var settings = g_settings_new("org.gnome.desktop.background");
            g_settings_set_string(settings, "picture-uri",
                new StringBuilder("file://" + fileName));
        }
    }
}