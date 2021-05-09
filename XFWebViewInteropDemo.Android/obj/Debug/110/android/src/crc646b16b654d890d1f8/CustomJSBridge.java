package crc646b16b654d890d1f8;


public class CustomJSBridge
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_InvokeAction:()V:__export__\n" +
			"";
		mono.android.Runtime.register ("XFWebViewInteropDemo.Droid.Renderers.CustomJSBridge, XFWebViewInteropDemo.Android", CustomJSBridge.class, __md_methods);
	}


	public CustomJSBridge ()
	{
		super ();
		if (getClass () == CustomJSBridge.class)
			mono.android.TypeManager.Activate ("XFWebViewInteropDemo.Droid.Renderers.CustomJSBridge, XFWebViewInteropDemo.Android", "", this, new java.lang.Object[] {  });
	}

	public CustomJSBridge (crc646b16b654d890d1f8.CustomWebViewRenderer p0, android.print.PrintDocumentAdapter p1)
	{
		super ();
		if (getClass () == CustomJSBridge.class)
			mono.android.TypeManager.Activate ("XFWebViewInteropDemo.Droid.Renderers.CustomJSBridge, XFWebViewInteropDemo.Android", "XFWebViewInteropDemo.Droid.Renderers.CustomWebViewRenderer, XFWebViewInteropDemo.Android:Android.Print.PrintDocumentAdapter, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}

	@android.webkit.JavascriptInterface

	public void invokeAction ()
	{
		n_InvokeAction ();
	}

	private native void n_InvokeAction ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
