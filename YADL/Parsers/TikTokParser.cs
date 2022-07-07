using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YADL.Parsers
{
    public class TikTokParser : IParser
    {
        public string Name => "Tiktok";
        public string[] AllowedUrls => new string[] { "https://tiktok.com", "https://vm.tiktok.com" };

        public string GetDownloadUrl(string uri)
        {
            return Task.Run(async () =>
            {
                using var client = new HttpClient();
                var message = new HttpRequestMessage(HttpMethod.Post, "https://ssstik.io/abc?url=dl");
                message.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("id", uri),
                    new KeyValuePair<string, string>("locale", "en"),
                });
                message.Headers.TryAddWithoutValidation("cookie", "PHPSESSID=v9np5qp5nrb5sp701ia6h3fmbj; ad_client=ssstik; __cflb=02DiuEcwseaiqqyPC5qqJCSC8ncVG3bV95CrK6KKq1y15; _ga=GA1.2.1274397781.1657190951; _gid=GA1.2.1543150674.1657190951; _gat_UA-3524196-6=1");
                message.Headers.TryAddWithoutValidation("hx-current-url", "https://ssstik.io/en");
                message.Headers.TryAddWithoutValidation("origin", "https://ssstik.io");
                message.Headers.TryAddWithoutValidation("referer", "https://ssstik.io/en");
                message.Headers.UserAgent.Clear();
                message.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.66 Safari/537.36 Edg/103.0.1264.44");
                var resp = await client.SendAsync(message);
                var content = await resp.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml("<html><body>" + content + "</body></html>");
                var urls = doc.DocumentNode.SelectNodes("//a");
                return urls.FirstOrDefault(x => x.GetAttributeValue<string>("href", "").Contains("tiktokcdn")).GetAttributeValue<string>("href", "");
            }).GetAwaiter().GetResult();
        }
        public string GetFriendlyName(string uri)
        {
            if (uri.Contains("vm.tiktok.com"))
            {
                return new Uri(uri).PathAndQuery.Split("/", StringSplitOptions.RemoveEmptyEntries)[0] + ".mp4";
            }
            else
            {
                return "download.mp4";
            }
        }
    }
}