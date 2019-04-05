using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Timetables.Client;

namespace Timetables.Server.Web.Controllers
{
	/// <summary>
	/// Helper methods for controllers.
	/// </summary>
	public static class ControllerBase
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <typeparam name="Req">Request type.</typeparam>
		/// <typeparam name="Proc">Processing object type.</typeparam>
		/// <typeparam name="Res">Response type.</typeparam>
		/// <param name="request">Request to process.</param>
		public static async Task<Res> ProcessAsync<Req, Proc, Res>(Req request) where Proc : Networking<Req, Res>, new() where Res : ResponseBase where Req : RequestBase
		{
			try
			{
				await DataFeedClient.DownloadAsync(true);

				using (var processing = new Proc())
				{
					return await processing.ProcessAsync(request, Settings.TimeoutDuration);
				}
			}
			catch
			{
				return default(Res);
			}
		}
	}
}