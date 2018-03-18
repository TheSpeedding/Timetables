#ifndef GTFS_FEED_HPP
#define GTFS_FEED_HPP

#include "services.hpp"
#include "routes_info.hpp"
#include "routes.hpp"
#include "stops.hpp"
#include "trips.hpp"
#include <fstream>
#include <iostream>

// After initialization strictly immutable and thread-safe. Because of all the observers pointing into the vectors. 
// Pushing back new items into the vectors may lead to memory leaks.

namespace Timetables {
	namespace Structures {

		// Class collecting information about all the data.
		class data_feed {
		private:
			services services_;
			routes_info routes_info_;
			stations stations_;
			stops stops_;
			routes routes_;
			trips trips_;
		public:
			data_feed(const std::string& path) :
				services_(std::ifstream(path + "/calendar.txt"), std::ifstream(path + "/calendar_dates.txt")),
				routes_info_(std::wifstream(path + "/routes_info.txt", std::ios::binary)),
				stations_(std::wifstream(path + "/stations.txt", std::ios::binary)),
				stops_(std::ifstream(path + "/stops.txt"), std::ifstream(path + "/footpaths.txt"), stations_),
				routes_(std::wifstream(path + "/routes.txt", std::ios::binary), routes_info_),
				trips_(std::ifstream(path + "/trips.txt"), routes_info_, routes_, services_) {
				trips_.set_timetables(std::ifstream(path + "/stop_times.txt"), stops_);
				routes_.set_stops_for_routes();
				stops_.set_throughgoing_routes_for_stops(routes_);
			}
			data_feed() : data_feed("data") {}

			inline const services& services() const { return services_; } // Collection of all the services.
			inline const routes_info& routes_info() const { return routes_info_; } // Collection of all the routesinfo.
			inline const stations& stations() const { return stations_; } // Collection of all the stations.
			inline const stops& stops() const { return stops_; } // Collection of all the stops.
			inline const routes& routes() const { return routes_; } // Collection of all the routes.
			inline const trips& trips() const { return trips_; } // Collection of all the trips.
		};
	}
}

#endif // !GTFS_FEED_HPP
