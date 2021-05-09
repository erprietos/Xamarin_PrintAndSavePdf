using System;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFWebViewInteropDemo.iOS.Renderers;

[assembly: ExportRenderer(typeof(WebView), typeof(HybridWebViewRenderer))]

namespace XFWebViewInteropDemo.iOS.Renderers
{
    public class HybridWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        private const string JavaScriptFunction = "function print(){window.webkit.messageHandlers.invokeAction.postMessage('print');}";
        private WKUserContentController _userController;

        public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
        {
        }

        public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            _userController = config.UserContentController;
            var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
            _userController.AddUserScript(script);
            _userController.AddScriptMessageHandler(this, "invokeAction");
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _userController.RemoveAllUserScripts();
                _userController.RemoveScriptMessageHandler("invokeAction");
                WebView hybridWebViewMain = e.OldElement as WebView;
                //hybridWebViewMain?.Cleanup();
            }

            if (e.NewElement != null)
            {
                //// No need this since we're loading dynamically generated HTML content
                //string filename = Path.Combine(NSBundle.MainBundle.BundlePath, $"Content/{((HybridWebView)Element).Uri}");
                //LoadRequest(new NSUrlRequest(new NSUrl(filename, false)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            var webView1 = (WebView)Element;
            var html1 = webView1.EvaluateJavaScriptAsync("document.documentElement.outerHTML");
            var r1 = string.Empty;

            Device.BeginInvokeOnMainThread(async () =>
            {
                r1 = await webView1.EvaluateJavaScriptAsync("getState();");
            });

            HtmlWebViewSource htmlPage = new HtmlWebViewSource();
            htmlPage.BaseUrl = "https://stackoverflow.com/questions/36214247/migrate-uiwebview-to-wkwebview";

            var browser = new WebView
            {
                Source = "https://dotnet.microsoft.com/apps/xamarin"
            };

            var t = browser.Source as HtmlWebViewSource;
            var html = t.Html;

            var urlelement = ((WebView)Element).Source as UrlWebViewSource;
            NSUrl url = NSUrl.FromFilename("https://stackoverflow.com/questions/36214247/migrate-uiwebview-to-wkwebview");

            var f = SafeHTMLToPDF("https://stackoverflow.com/questions/36214247/migrate-uiwebview-to-wkwebview", "atopdf");

            //var mstream = DownloadFileAsStreamAsync(urlelement.Url);

            //System.Threading.Thread.Sleep(2000);

            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ////Get the path of the Library folder within the MyDocuments folder
            //var library = Path.Combine(documents, "..", "Library");
            ////Create a new file with the input file name in the Library folder
            //var filepath = Path.Combine(library, "PrintSampleFile");
            //mstream.wr
            ////Write the contents of the input file to the newly created file
            //using (MemoryStream tempStream = new MemoryStream())
            //{
            //    file.Position = 0;
            //    file.CopyTo(tempStream);
            //    File.WriteAllBytes(filepath, mstream.ToArray());
            //}

            //var appleViewToPrint = Platform.CreateRenderer((WebView)Element).NativeView;

            //var printInfo = UIPrintInfo.PrintInfo;

            //printInfo.OutputType = UIPrintInfoOutputType.General;
            //printInfo.JobName = "Forms EZ-Print";
            //printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            //printInfo.Duplex = UIPrintInfoDuplex.None;

            //var printController = UIPrintInteractionController.SharedPrintController;

            //printController.PrintInfo = printInfo;
            //printController.ShowsPageRange = true;
            ////printController.PrintingItem = url;
            //// printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;
            //printController.PrintingItem = NSUrl.FromFilename(filepath);

            //printController.Present(true, (printInteractionController, completed, error) => { });

            var webView = new WKWebView(CGRect.Empty, new WKWebViewConfiguration());
            NSUrlRequest nSUrlRequest = new NSUrlRequest(url);
            NSString nS = new NSString("document.documentElement.outerHTML");



            ///////////
            

            var appleViewToPrint = Platform.CreateRenderer((WebView)Element).NativeView;

            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "Forms EZ-Print";
            printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            printInfo.Duplex = UIPrintInfoDuplex.None;

            var printController = UIPrintInteractionController.SharedPrintController;

            printController.PrintInfo = printInfo;
            printController.ShowsPageRange = true;
            printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) => { });



            ////////




            webView.LoadRequest(nSUrlRequest);

            //var webView = ((WKWebView)Element);
            //var printInfo = UIPrintInfo.PrintInfo;
            printInfo.JobName = "My first Print Job";
            printInfo.OutputType = UIPrintInfoOutputType.General;

            var printer = UIPrintInteractionController.SharedPrintController;
            //printer.PrintingItem = url;
            printer.PrintingItem = NSUrl.FromFilename(f);
            printer.PrintInfo = printInfo;
            //printer.PrintFormatter = webView.ViewPrintFormatter;
            printer.ShowsPageRange = true;
            printer.Present(true, (handler, completed, error) =>
            {
                if (!completed && error != null)
                {
                    //Console.WriteLine($"Error: {error.LocalizedDescription ?? ""}");
                }
            });

            printInfo.Dispose();
            //textFormatter.Dispose();
        }

        public string SafeHTMLToPDF(string html, string filename)
        {
            UIWebView webView = new UIWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(documents, "ATO" + "_" + ".pdf");

            webView.Delegate = new WebViewCallBack(file);
            webView.ScalesPageToFit = true;
            webView.UserInteractionEnabled = false;
            webView.BackgroundColor = UIColor.White;
            webView.LoadHtmlString(html, null);

            return file;
        }
    }
}