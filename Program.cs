using System;
using System.Runtime.InteropServices;
using EpApp.Classes;

namespace EpApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("App Started");
            Console.WriteLine("Talking to Reddit...");

            uint imagesLimit = 10; // can be read from args

            Reddit reddit = new Reddit();
            RedditReponse response = reddit.GetPostsAsync(imagesLimit).Result;

            var liked = false;
            var imgIndex = 0;

            do
            {
                string topImage = response.data.children[imgIndex].data.url;

                Console.WriteLine("Reddit data retrieved: " + topImage);
                Console.WriteLine("Downloading file...");

                IFileSaver saver = new FileDownloader();
                string path = saver.Save(topImage);

                Console.WriteLine("File downloaded to: " + path);
                Console.WriteLine("Setting desktop wallpaper...");

                IWallpaperSetter setter = CreateWallpaperSetter();
                if (setter == null)
                {
                    Console.Error.WriteLine("Platform not detected.");
                    return;
                }

                setter.SetWallpaper(path);

                Console.WriteLine("Done!");
                Console.WriteLine("Do you like it? [y (or empty line)/ n (or something else)]...");

                var input = Console.ReadLine().Trim();
                liked = string.IsNullOrEmpty(input) ? true : "y".Equals(input, StringComparison.OrdinalIgnoreCase) ? true : false;
                imgIndex++;

            } while (!liked && imgIndex < imagesLimit);
        }

        private static IWallpaperSetter CreateWallpaperSetter()
        {
            IWallpaperSetter setter = null;

            //If Windows
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                setter = new WindowsWallpaperImageSetter();
            
            //If OSX
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                setter = new MacOSWallpaperImageSetter();

            //If Linux
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var desktopEnvironment = Environment.GetEnvironmentVariable("DESKTOP_SESSION") ??
                                         Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP") ??
                                         Environment.GetEnvironmentVariable("XDG_SESSION_DESKTOP");
                switch(desktopEnvironment.ToLower())
                {
                    case "gnome":
                        setter = new LinuxGnomeWallpaperImageSetter();
                        break;
                }
                
            }

            return setter;
        }
    }
}
