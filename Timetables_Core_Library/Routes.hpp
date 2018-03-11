#ifndef ROUTES_HPP
#define ROUTES_HPP

// #include "Stops.hpp" // Reference to stops that the route goes through.
#include "RoutesInfo.hpp" // Reference to the route info from route class.
// #include "Trips.hpp" // Pointers to the trips serving the route.
#include <vector> // Main data structure in this header file.
#include <algorithm> // Usage of find on the vector.

namespace Timetables {
	namespace Structures {
		class Trip; // Forward declaration.
		class Stop; // Forward declaration.

		// Class collecting information about route.
		class Route {
		private:
			std::wstring headsign; // Headsign of the route.
			const RouteInfo& info; // Basic info for the route.
			std::vector<const Stop*> stopsSequence; // Stops that the route goes through
			std::vector<const Trip*> trips; // Pointers to trips which are sorted by departure.
		public:
			Route(const RouteInfo& info, std::size_t numberOfStops, const std::wstring& headsign) : info(info), headsign(headsign) { stopsSequence.reserve(numberOfStops); }

			inline const std::wstring& Headsign() const { return headsign; } // Headsign of the route.
			inline const RouteInfo& Info() const { return info; } // Info for the route.
			inline const std::vector<const Stop*>& Stops() const { return stopsSequence; } // Stops for the route.
			inline const std::vector<const Trip*>& Trips() const { return trips; } // Trips for the route-

			inline void AddTrip(const Trip& trip) { trips.push_back(&trip); } // Adds trip to the route. Used in initialization. Assuming that the trip are sorted by departure time.
			inline void AddStop(const Stop& stop) { stopsSequence.push_back(&stop); } // Adds stop to the route. Used in initialization.

			inline bool StopComesBefore(const Stop& A, const Stop& B) const { // Checks whether stop A comes before stop B in the route. A need for RAPTOR algorithm.
				// We can use properties of vector and just comapare the addresses. There is some space to improve time complexity.
				return std::find(stopsSequence.cbegin(), stopsSequence.cend(), &A) < std::find(stopsSequence.cbegin(), stopsSequence.cend(), &B);
			}
			
		};

		// Class collecting information about collection of the routes.
		class Routes {
		private:
			std::vector<Route> list; // List of all the routes, index of the item is also identificator for the routes.
		public:
			Routes(std::wistream&& routes, RoutesInfo& routesInfo);

			inline Route& Get(std::size_t id) { return list.at(id); } // Gets the route with given id.
			inline const Route& Get(std::size_t id) const { return list.at(id); } // Gets the route with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Route& operator[](std::size_t id) { return list[id]; } // Gets the route with given id.

			void SetStopsForRoutes(); // Sets stops for the routes. Used in initialization.
		};

	}
}

#endif // !ROUTES_HPP