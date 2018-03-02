#ifndef GTFS_FEED_HPP
#define GTFS_FEED_HPP

#include "Services.hpp"
#include "RoutesInfo.hpp"
#include "Routes.hpp"
#include "Stops.hpp"
#include "Trips.hpp"
#include <fstream>
#include <iostream>
#include <memory>

namespace Timetables {
	namespace Structures {

		// After initialization strictly immutable. Because of all the observers pointing into the vectors. 
		// Pushing back new items into the vectors may lead to memory leaks.

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
				stops(std::wifstream(path + "/stops.txt", std::ios::binary), stations),
				routes(std::ifstream(path + "/routes.txt"), routesInfo),
				trips(std::wifstream(path + "/trips.txt", std::ios::binary), routesInfo, routes, services) {
				stops.SetFootpaths(std::ifstream(path + "/footpaths.txt"));
				trips.SetTimetables(std::ifstream(path + "/stop_times.txt"), stops);
				routes.SetStopsForRoutes();
				stops.SetThroughgoingRoutesForStops(routes);
			}
			DataFeed() : DataFeed("data") {}

			inline const Services& Services() const { return services; }
			inline const RoutesInfo& RoutesInfo() const { return routesInfo; }
			inline const Stations& Stations() const { return stations; }
			inline const Stops& Stops() const { return stops; }
			inline const Routes& Routes() const { return routes; }
			inline const Trips& Trips() const { return trips; }
		};
	}
}

#endif // !GTFS_FEED_HPP