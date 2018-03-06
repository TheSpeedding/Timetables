#ifndef ROUTES_HPP
#define ROUTES_HPP

#include "Stops.hpp"
#include "RoutesInfo.hpp"
#include "Trips.hpp"
#include <vector>
#include <algorithm>

namespace Timetables {
	namespace Structures {
		class Trip;
		class Route {
		private:
			std::wstring headsign;
			const RouteInfo& info;
			std::vector<const Stop*> stopsSequence;
			std::vector<const Trip*> trips; // Departure from this first stop in the list. Sorted.
		public:
			Route(const RouteInfo& info, std::size_t numberOfStops, const std::wstring& headsign) : info(info), headsign(headsign) { stopsSequence.reserve(numberOfStops); }

			inline const std::wstring& Headsign() const { return headsign; }
			inline const RouteInfo& Info() const { return info; }
			inline const std::vector<const Stop*>& Stops() const { return stopsSequence; }
			inline const std::vector<const Trip*>& Trips() const { return trips; }

			inline void AddTrip(const Trip& trip) { trips.push_back(&trip); }
			inline void AddStop(const Stop& stop) { stopsSequence.push_back(&stop); }

			inline bool StopComesBefore(const Stop& A, const Stop& B) const {
				// We can use properties of vector and just comapare the addresses. There is some space to improve time complexity.
				return std::find(stopsSequence.cbegin(), stopsSequence.cend(), &A) < std::find(stopsSequence.cbegin(), stopsSequence.cend(), &B);
			}
			
		};

		class Routes {
		private:
			std::vector<Route> list;
		public:
			Routes(std::wistream&& routes, RoutesInfo& routesInfo);

			inline Route& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline Route& operator[](std::size_t id) { return list[id]; }

			void SetStopsForRoutes();
		};

	}
}

#endif // !ROUTES_HPP