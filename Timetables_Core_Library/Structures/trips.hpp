#ifndef TRIP_HPP
#define TRIP_HPP

#include "../Structures/services.hpp" // Reference to the service in Trip class.
#include "../Structures/routes_info.hpp" // Reference to the routes info in Trips class ctor.
#include "../Structures/stops.hpp" // Reference to the stops in SetTimetables method.
#include "routes.hpp" // Reference to the route in Trip class.
// #include "../Structures/stop_time.hpp" // StopTime used as a data member in Trip class.
#include <vector> // Data structure for trips.

namespace Timetables {
	namespace Structures {
		class stop_time; // Forward declaration.

		// Class collecting information about one trip.
		class trip {
			friend class route; // We need to reserve some space in vector due to move command and this should not be a part of API.
		private:
			const int departure_time_; // Departure from the first stop in the trip. Seconds since midnight.
			std::vector<stop_time> stop_times_; // List of the stop times included in this trip.
			const route& route_; // Reference to the route serving this trip.
			const service& service_; // Reference to the service that give us operating days for the trip.
		public:
			trip(const service& service, const route& route, int departure) : service_(service), departure_time_(departure), route_(route) {}

			inline const std::vector<stop_time>& stop_times() const { return stop_times_; } // Gets list of stop times belonging to the trip.
			inline const route& route() const { return route_; } // Gets information about route.
			inline const service& service() const { return service_; } // Gets service for this trip.
			inline const int departure() const { return departure_time_; } // Gets departure from the first stop of the trip, seconds since midnight.

			date_time find_departure_time_from_station(const station& s) const;
			
			inline void add_to_trip(const stop_time& stop_time) { stop_times_.push_back(stop_time); } // Adds stop time into the trip. Used in initialization.
		};

		// Class collecting information about collection of the trips.
		class trips {
		private:
			std::vector<trip*> list; // List of all the trips, index of the item is also identificator for the trip.
		public:
			trips(std::istream&& trips, routes& routes, services& services);

			inline trip& at(std::size_t id) { return *list.at(id); } // Gets the trip with given id.
			inline const trip& at(std::size_t id) const { return *list.at(id); } // Gets the trip with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline trip& operator[](std::size_t id) { return *list[id]; } // Gets the trip with given id.

			void set_timetables(std::istream&& stop_times, stops& stops); // Loads the file with stop times and sets timetables.
		};
	}
}

#endif // !TRIP_HPP
