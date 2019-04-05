using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timetables.Client;

namespace Timetables.Server.Web.Controllers
{
	public class StationController : ApiController
	{
		[Route("Station/{Source}/{Departure}/{CountOrArrival}/{IsStation}/{Line}")]
		public DepartureBoardResponse Get(int Source, ulong Departure, ulong CountOrArrival, bool IsStation, int Line)
		{
			// Count is type of int, maximal arrival is typically a bigger number.
			if (CountOrArrival < int.MaxValue)
				return Get(new StationInfoRequest(Source, new DateTime(1970, 1, 1).AddSeconds(Departure), (int)CountOrArrival, IsStation, Line));
			else
				return Get(new StationInfoRequest(Source, new DateTime(1970, 1, 1).AddSeconds(Departure), new DateTime(1970, 1, 1).AddSeconds(CountOrArrival), IsStation, Line));
		}

		[Route("Station/{Source}/{Departure}/{CountOrArrival}/{IsStation}")]
		public DepartureBoardResponse Get(int Source, ulong Departure, ulong CountOrArrival, bool IsStation)
		{
			return Get(Source, Departure, CountOrArrival, IsStation, -1);
		}

		[Route("Station/{Source}/{Departure}/{CountOrArrival}")]
		public DepartureBoardResponse Get(int Source, ulong Departure, ulong CountOrArrival)
		{
			return Get(Source, Departure, CountOrArrival, true, -1);
		}

		[Route("Station/{Source}/{Departure}")]
		public DepartureBoardResponse Get(int Source, ulong Departure)
		{
			return Get(Source, Departure, 5, true, -1);
		}

		[Route("Station/{Source}")]
		public DepartureBoardResponse Get(int Source)
		{
			return Get(Source, RequestBase.ConvertDateTimeToUnixTimestamp(DateTime.Now), 5, true, -1);
		}

		private DepartureBoardResponse Get(DepartureBoardRequest request)
		{
			return AsyncHelpers.RunSync(() => ControllerBase.ProcessAsync<DepartureBoardRequest, DepartureBoardProcessing, DepartureBoardResponse>(request)) ?? new DepartureBoardResponse();
		}
	}
}
