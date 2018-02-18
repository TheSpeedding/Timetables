#ifndef ROUTES_HPP
#define ROUTES_HPP

#include "Exceptions.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Trips.hpp"
#include <vector>
#include <string>
#include <map>

namespace Timetables {
	namespace Structures {
		class Stop; using StopPtrObserver = const Stop*;
		class Trip; using TripPtrObserver = const Trip*;

		enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5 };		

		class Route {
		private:
			const std::string shortName;
			const std::wstring longName;
			const RouteType type;
			std::vector<TripPtrObserver> trips; // TODO
		public:
			Route(const std::string& shortName, const std::wstring& longName, RouteType type) :
				shortName(shortName), longName(longName), type(type) {}

			inline const std::string& GetShortName() const { return shortName; }
			inline const std::wstring& GetLongName() const { return longName; }
			inline const RouteType GetType() const { return type; }
			inline const std::vector<TripPtrObserver>& GetTripsForRoute() const { return trips; }

			inline void AddTrip(const Trip& trip) { trips.push_back(&trip); }
		};

		class Routes {
		private:
			std::map<std::string, Route> list;
		public:
			Routes(std::wistream&& routes);
			inline Route& GetRoute(const std::string& id) {
				auto it = list.find(id);
				if (it == list.cend()) throw Timetables::Exceptions::RouteNotFoundException(id);
				else return it->second;
			}
		};
	}
}

#endif // !ROUTES_HPP
