using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YADL.Parsers
{
    public class Downloader
    {
        private string _uri;
        private MainActivity _activity;

        public Downloader(MainActivity activity, string uri)
        {
            _uri = uri;
            _activity = activity;
        }
        public string Uri => _uri;
        private static Dictionary<string, IParser> _parsers =
            Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => !x.IsInterface && typeof(IParser).IsAssignableFrom(x))
            .ToDictionary(k => ((IParser)Activator.CreateInstance(k)).Name, v => (IParser)Activator.CreateInstance(v));

        public bool TryDownload()
        {
            var url = _uri;
            var parser = _parsers.Values.FirstOrDefault(x => x.AllowedUrls.Any(x => url.Contains(x)));
            if (parser == null)
                return false;

            try
            {
                var request = new DownloadManager.Request(Android.Net.Uri.Parse(parser.GetDownloadUrl(url)));
                request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads,
                $"YADL/{parser.Name}/{parser.GetFriendlyName(url)}");
                request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                request.SetTitle($"YADL - {parser.Name} ({parser.GetFriendlyName(url)})");
                request.AllowScanningByMediaScanner();
                Toast.MakeText(_activity, $"Downloads -> YADL -> {parser.Name} -> {parser.GetFriendlyName(url)}", ToastLength.Long).Show();
                DownloadManager.FromContext(_activity).Enqueue(request);
                return true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(_activity, "We failed to find download url. Sorry 😭", ToastLength.Short).Show();
                return false;
            }
        }
    }
}