#ifndef GTFS_FEED_HPP
#define GTFS_FEED_HPP

#include "Services.hpp"
#include "RoutesInfo.hpp"
#include "Routes.hpp"
#include "Stops.hpp"
#include "Trips.hpp"
#include <fstream>
#include <iostream>

// After initialization strictly immutable and thread-safe. Because of all the observers pointing into the vectors. 
// Pushing back new items into the vectors may lead to memory leaks.

namespace Timetables {
	namespace Structures {

		// Class collecting information about all the data.
		class DataFeed {
		private:
			Services services;
			RoutesInfo routesInfo;
			Stations stations;
			Stops stops;
			Routes routes;
			Trips trips;
		public:
			DataFeed(const std::string& path) :
				services(std::ifstream(path + "/calendar.txt"), std::ifstream(path + "/calendar_dates.txt")),
				routesInfo(std::wifstream(path + "/routes_info.txt", std::ios::binary)),
				stations(std::wifstream(path + "/stations.txt", std::ios::binary)),
				stops(std::wifstream(path + "/stops.txt", std::ios::binary), std::ifstream(path + "/footpaths.txt"), stations),
				routes(std::wifstream(path + "/routes.txt", std::ios::binary), routesInfo),
				trips(std::ifstream(path + "/trips.txt"), routesInfo, routes, services) {
				trips.SetTimetables(std::ifstream(path + "/stop_times.txt"), stops);
				routes.SetStopsForRoutes();
				stops.SetThroughgoingRoutesForStops(routes);
			}
			DataFeed() : DataFeed("data") {}

			inline const Services& Services() const { return services; } // Collection of all the services.
			inline const RoutesInfo& RoutesInfo() const { return routesInfo; } // Collection of all the routesinfo.
			inline const Stations& Stations() const { return stations; } // Collection of all the stations.
			inline const Stops& Stops() const { return stops; } // Collection of all the stops.
			inline const Routes& Routes() const { return routes; } // Collection of all the routes.
			inline const Trips& Trips() const { return trips; } // Collection of all the trips.
		};
	}
}

#endif // !GTFS_FEED_HPP
