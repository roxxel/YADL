using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using YADL.Parsers;
using System.Threading.Tasks;
using System.Linq;

namespace YADL
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
     [IntentFilter(new[] { "android.intent.action.SEND" }, 
        Categories = new[] { "android.intent.category.BROWSABLE", "android.intent.category.DEFAULT" }, 
        DataMimeTypes = new[] { "text/plain" })]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (this.Intent.Action == "android.intent.action.SEND")
            {
                Finish();
                var uri = this.Intent.Extras.GetString("android.intent.extra.TEXT");
                Toast.MakeText(this, "Attempting downloading...", ToastLength.Short).Show();
                var downloader = new Downloader(this, uri);
                downloader.TryDownload();
            }

            var infoView = FindViewById<TextView>(Resource.Id.infoTextView);
            infoView.Text = "Hi! 👋\n" +
                "Nothing fancy here\n" +
                "\n" +
                "Supported applications:\n" +
                $"{string.Join("\n", Downloader.Parsers.Select(x=>$"- {x.Name}"))}";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
