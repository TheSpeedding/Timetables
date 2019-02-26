using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Timetables.Application.Mobile.HybridWebView), typeof(Timetables.Application.Mobile.Droid.HybridWebViewRenderer))]
namespace Timetables.Application.Mobile.Droid
{
	public class JSBridge : Java.Lang.Object
	{
		private readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;
		public JSBridge(HybridWebViewRenderer hybridRenderer) => hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
		[Android.Webkit.JavascriptInterface]
		[Java.Interop.Export("invoke")]
		public string InvokeAction(string name, string data) => hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out HybridWebViewRenderer hybridRenderer) ?
			hybridRenderer.Element.InvokeCallback(name, data) : string.Empty;
	}
	public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
	{
		private Context context;

		public HybridWebViewRenderer(Context context) : base(context) => this.context = context;

		protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				var webView = new Android.Webkit.WebView(context);
				webView.Settings.JavaScriptEnabled = true;
				SetNativeControl(webView);
			}

			if (e.OldElement != null)
			{
				Control.RemoveJavascriptInterface("jsBridge");
				var hybridWebView = e.OldElement as HybridWebView;
				hybridWebView.CleanUp();
			}

			if (e.NewElement != null)
			{
				Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
				Control.LoadData(((HtmlWebViewSource)Element.Source).Html, "text/html", null);
			}
		}
	}
}