using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timetables.Client;

namespace Timetables.Server.Web.Controllers
{
	public class LineController : ApiController
	{
		[Route("Line/{Line}/{Departure}/{CountOrArrival}")]
		public DepartureBoardResponse Get(int Line, ulong Departure, ulong CountOrArrival)
		{
			// Count is type of int, maximal arrival is typically a bigger number.
			if (CountOrArrival < int.MaxValue)
				return Get(new LineInfoRequest(new DateTime(1970, 1, 1).AddSeconds(Departure), (int)CountOrArrival, Line));
			else
				return Get(new LineInfoRequest(new DateTime(1970, 1, 1).AddSeconds(Departure), new DateTime(1970, 1, 1).AddSeconds(CountOrArrival), Line));
		}

		[Route("Line/{Line}/{Departure}")]
		public DepartureBoardResponse Get(int Line, ulong Departure)
		{
			return Get(Line, Departure, 5);
		}
		
		[Route("Line/{Line}")]
		public DepartureBoardResponse Get(int Line)
		{
			return Get(Line, RequestBase.ConvertDateTimeToUnixTimestamp(DateTime.Now), 5);
		}

		private DepartureBoardResponse Get(DepartureBoardRequest request)
		{
			return AsyncHelpers.RunSync(() => ControllerBase.ProcessAsync<DepartureBoardRequest, DepartureBoardProcessing, DepartureBoardResponse>(request)) ?? new DepartureBoardResponse();
		}
	}
}
