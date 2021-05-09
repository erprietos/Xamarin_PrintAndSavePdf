using Android.OS;
using Android.Print;
using Android.Print.Pdf;
using Android.Runtime;
using System;
using System.IO;
using XFWebViewInteropDemo.Droid.Renderers;

namespace XFWebViewInteropDemo.Droid
{
    public class CustomPrintDocumentAdapter : PrintDocumentAdapter
    {

        private PrintedPdfDocument _document;
        private readonly CustomWebViewRenderer _webView;
        private float _scale;

        internal CustomPrintDocumentAdapter(CustomWebViewRenderer webView)
        {
            this._webView = webView;
            //_hybridRenderer.LoadUrl(fileDesc);
        }
        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            _document = new PrintedPdfDocument(_webView.Context, newAttributes);

            CalculateScale(newAttributes);

            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
                return;
            }

            var webpageToPrint = (_webView.Element).Source.ToString();
            PrintDocumentInfo pdi = new PrintDocumentInfo.Builder(webpageToPrint).SetContentType(PrintContentType.Document).Build();

            callback.OnLayoutFinished(pdi, true);
        }
        void CalculateScale(PrintAttributes newAttributes)
        {
            int dpi = Math.Max(newAttributes.GetResolution().HorizontalDpi, newAttributes.GetResolution().VerticalDpi);

            int leftMargin = (int)(dpi * (float)newAttributes.MinMargins.LeftMils / 1000);
            int rightMargin = (int)(dpi * (float)newAttributes.MinMargins.RightMils / 1000);
            int topMargin = (int)(dpi * (float)newAttributes.MinMargins.TopMils / 1000);
            int bottomMargin = (int)(dpi * (float)newAttributes.MinMargins.BottomMils / 1000);

            int w = (int)(dpi * (float)newAttributes.GetMediaSize().WidthMils / 1000) - leftMargin - rightMargin;
            int h = (int)(dpi * (float)newAttributes.GetMediaSize().HeightMils / 1000) - topMargin - bottomMargin;

            _scale = Math.Min((float)_document.PageContentRect.Width() / w, (float)_document.PageContentRect.Height() / h);
        }

        void WritePrintedPdfDoc(ParcelFileDescriptor destination)
        {
            var javaStream = new Java.IO.FileOutputStream(destination.FileDescriptor);
            var osi = new OutputStreamInvoker(javaStream);
            using (var mem = new MemoryStream())
            {
                _document.WriteTo(mem);
                var bytes = mem.ToArray();
                osi.Write(bytes, 0, bytes.Length);
            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            try
            {
                Android.Graphics.Pdf.PdfDocument.Page page = _document.StartPage(0);

                page.Canvas.Scale(_scale, _scale);

                _webView.Draw(page.Canvas);

                _document.FinishPage(page);

                WritePrintedPdfDoc(destination);

                _document.Close();

                _document.Dispose();

                callback.OnWriteFinished(pages);





                ////Create FileInputStream object from the given file
                //input = new FileInputStream(FileToPrint);
                ////Create FileOutputStream object from the destination FileDescriptor instance
                //output = new FileOutputStream(destination.FileDescriptor);

                //byte[] buf = new byte[1024];
                //int bytesRead;

                //while ((bytesRead = input.Read(buf)) > 0)
                //{
                //    //Write the contents of the given file to the print destination
                //    output.Write(buf, 0, bytesRead);
                //}

                //callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });

            }
            catch (Exception ex)
            {
                //Catch exception
                System.Diagnostics.Debug.WriteLine(ex);
            }

            finally
            {

            }
        }
    }
}