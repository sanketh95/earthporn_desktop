using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EpApp.Classes
{
    public class Reddit 
    {
        private const string URL = "https://www.reddit.com/r/EarthPorn.json?limit=";

        public async Task<RedditReponse> GetPostsAsync(uint imagesLimit = 1)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(URL + imagesLimit);
            req.Headers["User-Agent"] = "dotnetcore:earthporn.desktop:v0.9 (by /u/BlahYourHamster)";
            HttpWebResponse res = (HttpWebResponse)await req.GetResponseAsync();
            
            string result;

            using (StreamReader stream = new StreamReader(res.GetResponseStream()))
                result = stream.ReadToEnd();
            
            return JsonConvert.DeserializeObject<RedditReponse>(result);
        }
    }

#region Response Classes

    public class SecureMediaEmbed
    {
    }

    public class Source
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Variants
    {
    }

    public class Image
    {
        public Source source { get; set; }
        public List<Resolution> resolutions { get; set; }
        public Variants variants { get; set; }
        public string id { get; set; }
    }

    public class Preview
    {
        public List<Image> images { get; set; }
    }

    public class MediaEmbed
    {
    }

    public class Data2
    {
        public bool contest_mode { get; set; }
        public object banned_by { get; set; }
        public string domain { get; set; }
        public string subreddit { get; set; }
        public object selftext_html { get; set; }
        public string selftext { get; set; }
        public object likes { get; set; }
        public object suggested_sort { get; set; }
        public List<object> user_reports { get; set; }
        public object secure_media { get; set; }
        public bool saved { get; set; }
        public string id { get; set; }
        public int gilded { get; set; }
        public SecureMediaEmbed secure_media_embed { get; set; }
        public bool clicked { get; set; }
        public object report_reasons { get; set; }
        public string author { get; set; }
        public object media { get; set; }
        public string name { get; set; }
        public int score { get; set; }
        public object approved_by { get; set; }
        public bool over_18 { get; set; }
        public object removal_reason { get; set; }
        public bool hidden { get; set; }
        public Preview preview { get; set; }
        public string thumbnail { get; set; }
        public string subreddit_id { get; set; }
        public bool edited { get; set; }
        public object link_flair_css_class { get; set; }
        public object author_flair_css_class { get; set; }
        public int downs { get; set; }
        public List<object> mod_reports { get; set; }
        public bool archived { get; set; }
        public MediaEmbed media_embed { get; set; }
        public string post_hint { get; set; }
        public bool is_self { get; set; }
        public bool hide_score { get; set; }
        public bool spoiler { get; set; }
        public string permalink { get; set; }
        public bool locked { get; set; }
        public bool stickied { get; set; }
        public double created { get; set; }
        public string url { get; set; }
        public object author_flair_text { get; set; }
        public bool quarantine { get; set; }
        public string title { get; set; }
        public double created_utc { get; set; }
        public object link_flair_text { get; set; }
        public object distinguished { get; set; }
        public int num_comments { get; set; }
        public bool visited { get; set; }
        public object num_reports { get; set; }
        public int ups { get; set; }
    }

    public class Child
    {
        public string kind { get; set; }
        public Data2 data { get; set; }
    }

    public class Data
    {
        public string modhash { get; set; }
        public List<Child> children { get; set; }
        public string after { get; set; }
        public object before { get; set; }
    }

    public class RedditReponse
    {
        public string kind { get; set; }
        public Data data { get; set; }
    }

#endregion
}