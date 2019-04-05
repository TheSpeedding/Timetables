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
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
			return DataFeedClient.Basic.Stations.FindByPartOfName(Name).Select(x => new StationBasicSerializable { ID = x.ID, Name = x.Name });
		}
	}
	public class ThroughgoingRoutesDataController : ApiController
	{
		[Route("Data/Station/{Name}/Lines")]
		public IEnumerable<LineBasicSerializable> Get(string Name)
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
			try
			{
				return DataFeedClient.Basic.Stations.FindByName(Name).GetThroughgoingRoutes().Select(x => new LineBasicSerializable { ID = x.ID, Label = x.Label });
			}
			catch
			{
				return new List<LineBasicSerializable>();
			}
		}
	}
	public class LineDataController : ApiController
	{
		[Route("Data/Lines/{Name}")]
		public IEnumerable<LineBasicSerializable> Get(string Name)
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
			return DataFeedClient.Basic.RoutesInfo.FindByPartOfLabel(Name).Select(x => new LineBasicSerializable { ID = x.ID, Label = x.Label });
		}
	}
	public class StopDataController : ApiController
	{
		[Route("Data/Stops")]
		public IEnumerable<StopBasicSerializable> Get()
		{
			AsyncHelpers.RunSync(() => DataFeedClient.DownloadAsync(true));
			return DataFeedClient.Basic.Stops.Select(x => new StopBasicSerializable { ID = x.ID, Name = x.ParentStation.Name, Latitude = x.Latitude, Longitude = x.Longitude });
		}
	}

	[Serializable]
	public class StationBasicSerializable
	{
		public int ID { get; set; }
		public string Name { get; set; }
	}
	[Serializable]
	public class StopBasicSerializable
	{
		public int ID { get; set; }
		public string Name { get; set; }
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