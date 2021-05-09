using Android.Webkit;

namespace XFWebViewInteropDemo.Droid.Renderers
{
    public class CustomJSWebViewClientRenderer : WebViewClient
    {
        readonly string _javascript;

        public CustomJSWebViewClientRenderer(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }
    }
}