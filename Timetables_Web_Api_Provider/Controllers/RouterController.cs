using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timetables.Client;

namespace Timetables.Server.Web.Controllers
{
    public class RouterController : ApiController
	{
		[Route("Router/{Source}/{Target}/{Departure}/{Transfers}/{CountOrArrival}/{Speed}/{MOT}")]
		public RouterResponse Get(int Source, int Target, ulong Departure, int Transfers, ulong CountOrArrival, double Speed, byte MOT)
		{
			// Count is type of int, maximal arrival is typically a bigger number.
			if (CountOrArrival < int.MaxValue) 
				return Get(new RouterRequest(Source, Target, new DateTime(1970, 1, 1).AddSeconds(Departure), Transfers, (int)CountOrArrival, Speed, (MeanOfTransport)MOT));
			else
				return Get(new RouterRequest(Source, Target, new DateTime(1970, 1, 1).AddSeconds(Departure), Transfers, new DateTime(1970, 1, 1).AddSeconds(CountOrArrival), Speed, (MeanOfTransport)MOT));
		}

		[Route("Router/{Source}/{Target}/{Departure}/{Transfers}/{CountOrArrival}")]
		public RouterResponse Get(int Source, int Target, ulong Departure, int Transfers, ulong CountOrArrival)
		{
			return Get(Source, Target, Departure, Transfers, CountOrArrival, 1, 255);
		}

		[Route("Router/{Source}/{Target}/{Departure}/{Transfers}")]
		public RouterResponse Get(int Source, int Target, ulong Departure, int Transfers)
		{
			return Get(Source, Target, Departure, Transfers, 5, 1, 255);
		}

		[Route("Router/{Source}/{Target}/{Departure}")]
		public RouterResponse Get(int Source, int Target, ulong Departure)
		{
			return Get(Source, Target, Departure, 100, 5, 1, 255);
		}

		[Route("Router/{Source}/{Target}")]
		public RouterResponse Get(int Source, int Target)
		{
			return Get(Source, Target, RequestBase.ConvertDateTimeToUnixTimestamp(DateTime.Now), 100, 5, 1, 255);
		}

		private RouterResponse Get(RouterRequest request)
		{
			return AsyncHelpers.RunSync(() => ControllerBase.ProcessAsync<RouterRequest, RouterProcessing, RouterResponse>(request)) ?? new RouterResponse();
		}
    }
}
