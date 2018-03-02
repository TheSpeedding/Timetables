using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Timetables.Preprocessor;

namespace Timetables.Xml
{
    public static class ToXmlExtensions
    {
        #region Footpaths
        public static string ToXml(this Footpaths.Footpath footpath) => $@"		<footpath> <!-- Information about transfer between two stops. -->
			<source stopId=""st{ footpath.First.ID }"">{ footpath.First.Name }</source> <!-- Reference to the source stop. -->
			<target stopId=""st{ footpath.Second.ID }"">{ footpath.Second.Name }</target> <!-- Reference to the target stop. -->
			<duration>{ footpath.Duration }</duration> <!-- Duration in seconds, average walking speed. -->
		</footpath>";
        public static void ToXml(this Footpaths footpaths, StreamWriter sw)
        {
            sw.WriteLine(@"	<footpaths>");

            foreach (var footpath in footpaths)
                sw.WriteLine(footpath.ToXml());

            sw.WriteLine(@"	</footpaths>");
        }
        #endregion

        #region Routes
        public static string ToXml(this Routes.Route route)
        {
            StringBuilder result = new StringBuilder();

            result.Append($@"		<route routeId=""r{ route.ID }"" routeInfoId=""ri{ route.RouteInfo.ID }""> <!-- Basic information about route. Routes can share some basic information. -->
			<stops-ref> <!-- List of the references to stops contained in-order in this route. -->");

            foreach (var stop in route.Stops)
                result.Append($@"
				<stop-ref stopId=""st{ stop.ID }"">{ stop.Name }</stop-ref>");

            result.Append($@"
			</stops-ref>
		</route>");

            return result.ToString();
        }
        public static void ToXml(this Routes routes, StreamWriter sw)
        {
            sw.WriteLine(@"	<routes>");

            foreach (var route in routes)
                sw.WriteLine(route.ToXml());

            sw.WriteLine(@"	</routes>");
        }
		#endregion

		#region Services
		public static string ToXml(this Calendar.Service service)
		{
			StringBuilder result = new StringBuilder();

			result.Append($@"		<service serviceId=""se{ service.ID }""> <!-- Information about service. -->
			<valid-since>{ service.ValidSince }</valid-since> <!-- Date that the timetables are valid since. -->
			<valid-until>{ service.ValidUntil }</valid-until> <!-- Date that the timetables are valid until. -->
			<operating-days> <!-- List of the operating days. -->");

			if (service.OperatingDays[0]) result.Append($@"
				<monday/>");
			if (service.OperatingDays[1]) result.Append($@"
				<tuesday/>");
			if (service.OperatingDays[2]) result.Append($@"
				<wednesday/>");
			if (service.OperatingDays[3]) result.Append($@"
				<thursday/>");
			if (service.OperatingDays[4]) result.Append($@"
				<friday/>");
			if (service.OperatingDays[5]) result.Append($@"
				<saturday/>");
			if (service.OperatingDays[6]) result.Append($@"
				<sunday/>");

			result.Append($@"
			</operating-days>");

			if (service.ExtraordinaryEvents.Count == 0)
				result.Append(@"
			<extraordinary-events/> <!-- List of extraordinary events for this service. -->");
			
			else
			{
				result.Append(@"
			<extraordinary-events> <!-- List of extraordinary events for this service. -->");
				
				foreach (var ev in service.ExtraordinaryEvents)
					result.Append($@"
				<extraordinary-event type=""{ (ev.Item2 ? "added" : "removed") }"">
					<date>{ ev.Item1 }</date>
				</extraordinary-event>");


				result.Append(@"
			</extraordinary-events>");
			}

			result.Append(@"
		</service>");

			return result.ToString();
		}
		public static void ToXml(this Calendar services, StreamWriter sw)
		{
			sw.WriteLine(@"	<services>");

			foreach (var service in services)
				sw.WriteLine(service.Value.ToXml());

			sw.WriteLine(@"	</services>");
		}
		#endregion

		#region Stops
		public static string ToXml(this Stops.Stop stop)
		{
			StringBuilder result = new StringBuilder();

			result.Append($@"		<stop stopId=""st{ stop.ID }""> <!-- Information about stop. -->
			<name>{ stop.Name }</name> <!-- Name of the stop. -->
			<location> <!-- GPS coordinates of the stop. -->
				<latitude>{ stop.Location.Item1 }</latitude>
				<longitude>{ stop.Location.Item2 }</longitude>
			</location>			
		</stop>");

			return result.ToString();
		}
		public static void ToXml(this Stops stops, StreamWriter sw)
		{
			sw.WriteLine(@"	<stops>");

			foreach (var stop in stops)
				sw.WriteLine(stop.Value.ToXml());

			sw.WriteLine(@"	</stops>");
		}
		#endregion

		#region RoutesInfo
		public static string ToXml(this RoutesInfo.RouteInfo routeInfo)
		{
			StringBuilder result = new StringBuilder();

			result.Append($@"		<route-info routeInfoId=""ri{ routeInfo.ID }""> <!-- Information about route. -->
			{ routeInfo.LongName } <!-- The most important stops in the route. -->
			<label>{ routeInfo.ShortName }</label> <!-- Label of the route. -->
			<mean-of-transport type=""");

			switch (routeInfo.Type)
			{
				case RoutesInfo.RouteInfo.RouteType.Tram:
					result.Append("tram");
					break;
				case RoutesInfo.RouteInfo.RouteType.Subway:
					result.Append("subway");
					break;
				case RoutesInfo.RouteInfo.RouteType.Rail:
					result.Append("rail");
					break;
				case RoutesInfo.RouteInfo.RouteType.Bus:
					result.Append("bus");
					break;
				case RoutesInfo.RouteInfo.RouteType.Ship:
					result.Append("ship");
					break;
				case RoutesInfo.RouteInfo.RouteType.CableCar:
					result.Append("cablecar");
					break;
				case RoutesInfo.RouteInfo.RouteType.Gondola:
					result.Append("gondola");
					break;
				case RoutesInfo.RouteInfo.RouteType.Funicular:
					result.Append("funicular");
					break;
			}

			result.Append($@"""/> <!-- Mean of the transport. -->
			<color hex-code=""");

			if (routeInfo.Color == "CC0000") result.Append("&tram-color;");
			if (routeInfo.Color == "00FFFF") result.Append("&bus-color;");
			if (routeInfo.Color == "FFFFFF") result.Append("&cablecar-color;");
			if (routeInfo.Color == "006600") result.Append("&rail-color;");
			if (routeInfo.Color == "0033CC") result.Append("&ship-color;");

			result.Append($@"""/> <!-- Color of the route used in graphics. -->
		</route-info>");

			return result.ToString();
		}
		public static void ToXml(this RoutesInfo routes, StreamWriter sw)
		{
			sw.WriteLine(@"	<routes-info>");

			foreach (var route in routes)
				sw.WriteLine(route.Value.ToXml());

			sw.WriteLine(@"	</routes-info>");
		}
		#endregion
		
		#region Trips
		public static string ToXml(this Trips.Trip trip)
		{
			StringBuilder result = new StringBuilder();

			result.Append($@"		<trip tripId=""t{ trip.ID }"" routeInfoId = ""ri{ trip.RouteInfo.ID }"" routeId=""r{ trip.Route.ID }"" serviceId=""se{ trip.Service.ID }""> <!-- Information about trip. -->
			<headsign>{ trip.Headsign }</headsign> <!-- Headsign of the trip. The stop that the trip goes in. -->
			<stop-times> <!-- List of stop times included in the trip. -->");

			foreach (var stopTime in trip.StopTimes)
				result.Append($@"
				<stop-time>
					<stop-ref stopId=""st{ stopTime.Stop.ID }"">{ stopTime.Stop.Name }</stop-ref>
					<arrival>{ stopTime.ArrivalTime }</arrival>
					<departure>{ stopTime.DepartureTime }</departure>
				</stop-time>");

			result.Append(@"
			</stop-times>
		</trip>");


			return result.ToString();
		}
		public static void ToXml(this Trips trips, StreamWriter sw)
		{
			sw.WriteLine(@"	<trips>");

			foreach (var trip in trips)
				sw.WriteLine(trip.Value.ToXml());

			sw.WriteLine(@"	</trips>");
		}
		#endregion
	}
}
