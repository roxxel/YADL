using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADL.Parsers
{
    public interface IParser
    {
        string Name { get; }
        string[] AllowedUrls { get; }
        string GetDownloadUrl(string uri);
        string GetFriendlyName(string uri);
    }
}