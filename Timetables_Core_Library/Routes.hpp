#ifndef ROUTES_HPP
#define ROUTES_HPP

#include "Stops.hpp"
#include "Utilities.hpp"
#include "Trips.hpp"
#include <vector>
#include <map>

namespace Timetables {
	namespace Structures {
		class Stop; using StopPtrObserver = const Stop*;
		class Trip; using TripPtrObserver = const Trip*;

		class Route {
		private:
			const RouteInfo& info;
			std::vector<StopPtrObserver> stopsSequence;
			std::map<Time, TripPtrObserver> trips; // Departure from this first stop in the list.
		public:
			Route(const RouteInfo& info, const std::vector<StopPtrObserver>& stops) : info(info), stopsSequence(stops) {}

			inline const RouteInfo& GetInfo() const { return info; }
			inline const std::vector<StopPtrObserver>& GetStops() const { return stopsSequence; }
			inline const std::map<Time, TripPtrObserver>& GetTrips() const { return trips; }

			inline void AddTrip(const Time& time, const Trip& trip) { trips.insert(std::make_pair(time, &trip)); }
			inline void AddStop(const Stop& stop) { stopsSequence.push_back(&stop); }
			
			inline bool operator== (const Route& other) {
				if (stopsSequence.size() != other.stopsSequence.size()) return false;
				for (std::size_t i = 0; i < stopsSequence.size(); i++) if (stopsSequence.at(i) != other.stopsSequence.at(i)) return false;
				return true;
			}
		};

		class Routes {
		private:
			std::vector<Route> list;
		public:
			Routes(const RoutesInfo& info, const Trips& trips);

			inline const std::vector<Route>& GetRoutes() const { return list; }
		};

	}
}

#endif // !ROUTES_HPP