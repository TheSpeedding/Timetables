using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Timetables.Application.Mobile
{
	public delegate string CSharpCallbackHandler(string args);
	public class HybridWebView : WebView
	{
		private Dictionary<string, CSharpCallbackHandler> callbacks = new Dictionary<string, CSharpCallbackHandler>();
		public Interop.Scripting Scripting { get; set; }
		public void RegisterCallback(string name, CSharpCallbackHandler handler) => callbacks[name] = handler;
		public void RemoveCallback(string name) => callbacks.Remove(name);
		public void CleanUp() => callbacks = new Dictionary<string, CSharpCallbackHandler>();
		public string InvokeCallback(string name, string args) => name != null && callbacks.ContainsKey(name) ?
			callbacks[name](args) : string.Empty;
	}
}
