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

            Reddit reddit = new Reddit();
            IFileSaver saver = new FileDownloader();
            IWallpaperSetter setter = CreateWallpaperSetter();

            if (setter == null)
            {
                Console.Error.WriteLine("Platform not detected.");
                return;
            }

            Console.WriteLine("Getting top image from /r/earthporn...");

            //Get the top image to start with.
            RedditReponse response = reddit.GetPostsAsync(limit: 1).Result;

            bool liked = false;

            while(!liked)
            {
                Child post = response.data.children[0];
                
                Console.WriteLine("Downloading image: " + post.data.url);

                string path = saver.Save(post.data.url);

                Console.WriteLine("Image downloaded to: " + path);
                Console.WriteLine("Setting desktop wallpaper...");

                setter.SetWallpaper(path);

                Console.WriteLine("Do you like it? [Y/N]");

                var input = Console.ReadLine().Trim();
                liked = input.ToLower().Equals("y", StringComparison.OrdinalIgnoreCase);
                
                //If they didn't like it, get the next post and try again.
                if (!liked)
                {
                    Console.WriteLine("Getting the next image from /r/earthporn...");

                    response = reddit.GetPostsAsync(limit: 1).Result;

                    Console.WriteLine("Reddit data retrieved.");
                }
            }

            Console.WriteLine("Finished!");
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
