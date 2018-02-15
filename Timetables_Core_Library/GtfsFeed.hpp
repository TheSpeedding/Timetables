#ifndef GTFS_FEED_HPP
#define GTFS_FEED_HPP

#include "Services.hpp"
#include "Shapes.hpp"
#include "Routes.hpp"
#include "Stops.hpp"
#include "Trips.hpp"
#include "Footpaths.hpp"
#include <fstream>
#include <iostream>

namespace Timetables {
	namespace Structures {
		class GtfsFeed {
		private:
			Services services;
			Shapes shapes;
			Routes routes;
			Stops stops;
			Trips trips;
			Footpaths footpaths;
		public:
			GtfsFeed(const std::string& path) :
				services(std::ifstream(path + "calendar.txt"), std::ifstream(path + "calendar_dates.txt")),
				shapes(std::ifstream(path + "shapes.txt")),
				routes(std::wifstream(path + "routes.txt", std::ios::binary)),
				stops(std::wifstream(path + "stops.txt", std::ios::binary)),
				trips(std::wifstream(path + "trips.txt", std::ios::binary), routes, services, shapes),
				footpaths(stops) {
				trips.SetTimetables(std::ifstream(path + "stop_times.txt"), stops);
			}
			GtfsFeed() : GtfsFeed("") {}

			inline const Stops& GetStops() const { return stops; }
			inline const Routes& GetRoutes() const { return routes; }
			inline const Trips& GetTrips() const { return trips; }
		};
	}
}

#endif // !GTFS_FEED_HPP
