#ifndef GTFS_FEED_HPP
#define GTFS_FEED_HPP

#include "Services.hpp"
#include "Shapes.hpp"
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

		class GtfsFeed {
		private:
			Services services;
			Shapes shapes;
			RoutesInfo routesInfo;
			std::unique_ptr<Routes> routes;
			Stops stops;
			Trips trips;
		public:
			GtfsFeed(const std::string& path) :
				services(std::ifstream(path + "calendar.txt"), std::ifstream(path + "calendar_dates.txt")),
				shapes(std::ifstream(path + "shapes.txt")),
				routesInfo(std::wifstream(path + "routes.txt", std::ios::binary)),
				stops(std::wifstream(path + "stops.txt", std::ios::binary)),
				trips(std::wifstream(path + "trips.txt", std::ios::binary), routesInfo, services, shapes) {
				trips.SetTimetables(std::ifstream(path + "stop_times.txt"), stops);
				routes = std::make_unique<Routes>(routesInfo, trips); // I need to initialize it at this moment and no default constructor is given.
				// stops.SetThroughgoingRoutesForStops();
			}
			GtfsFeed() : GtfsFeed("") {}

			inline const std::map<std::string, Stop>& GetStops() const { return stops.GetStops(); }
			inline const std::map<std::wstring, Station>& GetStations() const { return stops.GetStations(); }
			inline const RoutesInfo& GetRoutesInfo() const { return routesInfo; }
			inline const Routes& GetRoutes() const { return *routes; }
			inline const Trips& GetTrips() const { return trips; }
			inline const Services& GetServices() const { return services; }
		};
	}
}

#endif // !GTFS_FEED_HPP
