using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Application.Mobile;
using Timetables.Client;
using Timetables.Structures.Basic;

namespace Timetables.Application.Mobile
{
	// <summary>
	/// Offers functions that can be called from Javascript code.
	/// </summary>
	public class Scripting : Timetables.Interop.Scripting
	{
		protected HybridWebView view;

		/// <summary>
		/// Sets JS callbacks for given HybridWebView.
		/// </summary>
		/// <param name="view">HybridWebView.</param>
		public Scripting(HybridWebView view) : base(Settings.Localization, true)
		{
			this.view = view;

			view.RegisterCallback(nameof(Iso8601ToSimpleString), Iso8601ToSimpleString);
			view.RegisterCallback(nameof(IsMobileVersion), _ => IsMobileVersion().ToString());
			view.RegisterCallback(nameof(TransferStringConstant), _ => TransferStringConstant());
			view.RegisterCallback(nameof(TotalTransfersToString), arg => TotalTransfersToString(int.Parse(arg)));
			view.RegisterCallback(nameof(TotalDurationToString), TotalDurationToString);
			view.RegisterCallback(nameof(LeavingTimeToString), LeavingTimeToString);
			view.RegisterCallback(nameof(ReplaceIdWithName), arg => ReplaceIdWithName(int.Parse(arg)));
			view.RegisterCallback(nameof(MapStringConstant), _ => MapStringConstant());
			view.RegisterCallback(nameof(DetailStringConstant), _ => DetailStringConstant());
			view.RegisterCallback(nameof(PrintStringConstant), _ => PrintStringConstant());
			view.RegisterCallback(nameof(OutdatedStringConstant), _ => OutdatedStringConstant());
			view.RegisterCallback(nameof(EditParametersStringConstant), _ => EditParametersStringConstant());
			view.RegisterCallback(nameof(PrintListStringConstant), _ => PrintListStringConstant());
		}

		/// <summary>
		/// Replaces stop ID with coressponding name.
		/// </summary>
		/// <param name="id">ID of the stop.</param>
		/// <returns>Name of the stop.</returns>
		public string ReplaceIdWithName(int id) => DataFeedClient.Basic.Stops.FindByIndex(id).Name;
	}
}