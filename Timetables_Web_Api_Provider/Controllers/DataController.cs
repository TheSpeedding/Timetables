using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timetables.Client;
using Timetables.Structures.Basic;

namespace Timetables.Server.Web.Controllers
{
	public class StationDataController : ApiController
	{
		[Route("Data/Stations/{Name}")]
		public IEnumerable<StationBasicSerializable> Get(string Name)
		{
			return DataFeedClient.Basic.Stations.FindByPartOfName(Name).Select(x => new StationBasicSerializable { ID = x.ID, Name = x.Name });
		}
		static StationDataController()
		{ 
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
		}
	}
	public class ThroughgoingLinesDataController : ApiController
	{
		[Route("Data/Station/{Name}/Lines")]
		public IEnumerable<LineBasicSerializable> Get(string Name)
		{
			try
			{
				return DataFeedClient.Basic.Stations.FindByName(Name).GetThroughgoingRoutes().Select(x => new LineBasicSerializable { ID = x.ID, Label = x.Label });
			}
			catch
			{
				return new List<LineBasicSerializable>();
			}
		}
		static ThroughgoingLinesDataController()
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
		}
	}
	public class LineDataController : ApiController
	{
		[Route("Data/Lines/{Name}")]
		public IEnumerable<LineBasicSerializable> Get(string Name)
		{
			return DataFeedClient.Basic.RoutesInfo.FindByPartOfLabel(Name).Select(x => new LineBasicSerializable { ID = x.ID, Label = x.Label });
		}

		static LineDataController()
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
		}
	}
	public class StopDataController : ApiController
	{
		[Route("Data/Stops")]
		public IEnumerable<StopBasicSerializable> Get()
		{
			return DataFeedClient.Basic.Stops.Select(x => new StopBasicSerializable { ID = x.ID, Name = x.ParentStation.Name, Latitude = x.Latitude, Longitude = x.Longitude });
		}
		static StopDataController()
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
		}
	}
	public class StopSimplifiedDataController : ApiController
	{
		[Route("Data/SimplifiedStops")]
		public IEnumerable<SimplifiedStopBasicSerializable> Get()
		{
			return DataFeedClient.Basic.Stops.Select(x => new SimplifiedStopBasicSerializable { ID = x.ID, Name = x.ParentStation.Name });
		}
		static StopSimplifiedDataController()
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
		}
	}

	[Serializable]
	public class StationBasicSerializable
	{
		public int ID { get; set; }
		public string Name { get; set; }
	}
	[Serializable]
	public class SimplifiedStopBasicSerializable
	{
		public int ID { get; set; }
		public string Name { get; set; }
	}
	[Serializable]
	public class StopBasicSerializable : SimplifiedStopBasicSerializable
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
	[Serializable]
	public class LineBasicSerializable
	{
		public int ID { get; set; }
		public string Label { get; set; }
	}
}