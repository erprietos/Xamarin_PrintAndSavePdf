using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(XFWebViewInteropDemo.Droid.Renderers.CustomWebViewRenderer))]

namespace XFWebViewInteropDemo.Droid.Renderers
{
    public class CustomWebViewRenderer : Xamarin.Forms.Platform.Android.WebViewRenderer
    {
        private const string JavascriptFunction = "function print(){jsBridge.invokeAction();}";
        private Context _context;

        public CustomWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
            }
            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.AllowContentAccess = true;
                Control.Settings.LoadsImagesAutomatically = true;
                Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.UseWideViewPort = true;
                Control.SetWebViewClient(new CustomJSWebViewClientRenderer($"javascript: {JavascriptFunction}"));
                Control.AddJavascriptInterface(new CustomJSBridge(this, Control.CreatePrintDocumentAdapter("Document")), "jsBridge");
            }

            //base.OnElementChanged(e);

            //if (e.OldElement != null)
            //{
            //    Control.RemoveJavascriptInterface("jsBridge");
            //    //((HybridWebView)Element).Cleanup();
            //}
            //if (e.NewElement != null)
            //{
            //    Control.SetWebViewClient(new CustomJSWebViewClientRenderer($"javascript: {JavascriptFunction}"));
            //    Control.AddJavascriptInterface(new CustomJSBridge(this), "jsBridge");
            //    //// No need this since we're loading dynamically generated HTML content
            //    //Control.LoadUrl($@"file:///android_asset/Content/{((HybridWebView)Element).Uri}");
            //}
        }
    }
}