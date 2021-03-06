#ifndef ROUTES_HPP
#define ROUTES_HPP

#include "../macros_definition.hpp"
// #include "../Structures/stops.hpp" // Reference to stops that the route goes through.
#include "../Structures/stations.hpp" // Reference parameter in method for checking if route contains station.
#include "../Structures/routes_info.hpp" // Reference to the route info from route class.
// #include "../Structures/trips.hpp" // Pointers to the trips serving the route.
#include <vector> // Main data structure in this header file.
#include <unordered_map> 
#include <algorithm> // Usage of find on the vector.

namespace Timetables {
	namespace Structures {
		class trip; // Forward declaration.
		class stop; // Forward declaration.

		// Class collecting information about route.
		class route {
		private:
			std::wstring headsign_; // Headsign of the route.
			const route_info& info_; // Basic info for the route.
			std::vector<const stop*> stops_sequence_; // Stops that the route goes through.
			std::vector<trip> trips_; // Trips which are sorted by departure.
		public:
			route(const route_info& info, std::size_t number_of_stops, const std::wstring& headsign, std::size_t number_of_trips) : info_(info), headsign_(headsign) {
				trips_.reserve(number_of_trips);
				stops_sequence_.reserve(number_of_stops);
			}

			inline const std::wstring& headsign() const { return headsign_; } // Headsign of the route.
			inline const route_info& info() const { return info_; } // Info for the route.
			inline const std::vector<const stop*>& stops() const { return stops_sequence_; } // Stops for the route.
			inline const std::vector<trip>& trips() const { return trips_; } // Trips for the route.

			bool contains_station(const station& s) const;

			trip& add_trip(const trip& trip); // Adds trip to the route. Used in initialization. Assuming that the trips are sorted by departure time.
			inline void add_stop(const stop& stop) { stops_sequence_.push_back(&stop); } // Adds stop to the route. Used in initialization.
			
			inline bool stop_comes_before(const stop& A, const stop& B) const { // Checks whether stop A comes before stop B in the route. A need for RAPTOR algorithm.
				// We can use properties of vector and just comapare the addresses.
				return std::find(stops_sequence_.cbegin(), stops_sequence_.cend(), &A) < std::find(stops_sequence_.cbegin(), stops_sequence_.cend(), &B);
			}			
		};

		// Class collecting information about collection of the routes.
		class routes {
		private:
			std::vector<route> list; // List of all the routes, index of the item is also identificator for the routes.
		public:
			routes(std::wistream&& routes, routes_info& routes_info);

			inline route& at(std::size_t id) { return list.at(id); } // Gets the route with given id.
			inline const route& at(std::size_t id) const { return list.at(id); } // Gets the route with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline route& operator[](std::size_t id) { return list[id]; } // Gets the route with given id.

			void set_stops_for_routes(); // Sets stops for the routes. Used in initialization.
		};

	}
}

#endif // !ROUTES_HPP