using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EpApp.Classes
{
    public interface IFileSaver
    {
        string Save(string source, string extension = "jpeg");
    }

    public class FileDownloader : IFileSaver
    {
        public string Save(string source, string extension = "jpeg")
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException(nameof(extension));

            source = SanitizeSource(source, extension);

            //Create a temporary file.
            string temp = Path.GetTempFileName();
            string withExtension = Path.ChangeExtension(temp, extension);

            DownloadFileAsync(source, temp).Wait();

            File.Move(temp, withExtension);

            return withExtension;
        }
        
        private string SanitizeSource(string source, string extension) 
        {
            bool isHttps = source.StartsWith("https://");
            string http = isHttps ? "https://" : "http://";

            //Get rid of the http and www
            source = source.Replace("http://", "")
                        .Replace("https://", "")
                        .Replace("www.", "");

            //imgur links need to start with i. and end with the extension
            //to get the actual image.
            if (source.StartsWith("imgur"))
                source = string.Format("{0}i.{1}.{2}", http, source, extension);
            else
                source = string.Format("{0}{1}", http, source);

            source = source.Replace("&amp;", "&");

            return source;
        }

        private async Task DownloadFileAsync(string source, string destination)
        {
            using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, source))
                {
                    using (Stream contentStream = await 
                        (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                                stream = new FileStream(destination, FileMode.Open, FileAccess.Write))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
        }
    }
}