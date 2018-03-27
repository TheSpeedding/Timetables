#ifndef STOPS_HPP
#define STOPS_HPP

#include "../Structures/date_time.hpp" // Dealing with time.
#include "../Structures/stations.hpp" // Reference to the parent station in stop.
#include "../Structures/routes.hpp" // Reference to the route in stop (throughgoing routes).
#include <string> // String is a need for names.
#include <vector> // Data structure for stops.
#include <algorithm> // Sorting.
#include <map> // Data structure for footpaths.

namespace Timetables {
	namespace Structures {
		class station; // Forward declaration.

		// Class collecting information about stop.
		class stop {
		private:
			station& parent_station_; // Refernece to the parent stations. A need for departure board. Name of the stop can be accessed via parent station (they have to be the same). be better.
			std::multimap<std::size_t, const stop*> footpaths_; // Stops reachable in walking-distance (< 10 min.) from this stop.
			std::vector<const route*> throughgoing_routes_; // Routes that goes through this stop.
			std::vector<const stop_time*> departures_; // Sorted by departure times.
		public:
			stop(station& parent_station) : parent_station_(parent_station) {}

			inline station& parent_station() const { return parent_station_; } // Parent station for this stop.
			inline const std::wstring& name() const { return parent_station_.name(); } // Name of this stop.
			inline const std::vector<const route*>& throughgoing_routes() const { return throughgoing_routes_; } // Routes that goes through this stop.
			inline const std::multimap<std::size_t, const stop*>& footpaths() const { return footpaths_; } // Stops reachable in walking-distance (< 10 min.) from this stop.
			inline const std::vector<const stop_time*>& departures() const { return departures_; } // Departures from this stop.

			void add_departure(const stop_time& stop_time); // Adds a departure. Used in initialization.
			inline void add_throughgoing_route(const route& route) { throughgoing_routes_.push_back(&route); } // Adds routes that goes through this stop. Used in initialization.
			inline void add_footpath(const stop& stop, std::size_t time) { footpaths_.insert(std::make_pair(time, &stop)); } // Adds a footpath. Used in initialization.
		};

		// Class collecting information about collection of the stops.
		class stops {
		private:
			std::vector<stop> list; // List of all the stops, index of the item is also identificator for the stop.
		public:
			stops(std::istream&& stops, std::istream&& footpaths, stations& stations);

			inline stop& at(std::size_t id) { return list.at(id); } // Gets the stop with given id.
			inline const stop& at(std::size_t id) const { return list.at(id); } // Gets the stop with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline stop& operator[](std::size_t id) { return list[id]; } // Gets the stop with given id.

			void set_throughgoing_routes_for_stops(routes& routes); // Sets throughoing routes for every single stop. Based on the data loaded later than stops.
		};

	}
}

#endif // !STOPS_HPP