using Android.Content;
using Android.Print;
using Android.Webkit;
using Java.Interop;

namespace XFWebViewInteropDemo.Droid.Renderers
{
    public class CustomJSBridge : Java.Lang.Object
    {
        private readonly CustomWebViewRenderer _webViewRenderer;
        private PrintDocumentAdapter _printDocumentAdapter;

        public CustomJSBridge(CustomWebViewRenderer webViewrenderer, PrintDocumentAdapter printDocumentAdapter)
        {
            this._printDocumentAdapter = printDocumentAdapter;
            this._webViewRenderer = webViewrenderer;
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction()
        {
            if (_webViewRenderer != null)
            {
                PrintManager printManager = (PrintManager)_webViewRenderer.Context.GetSystemService(Context.PrintService);

                //Assign document from web view control
                PrintDocumentAdapter pda = _printDocumentAdapter;
                //Print with null PrintAttributes
                printManager.Print("Print ATO document", pda, null);
            }
        }
    }
}