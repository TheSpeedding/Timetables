#ifndef ROUTES_HPP
#define ROUTES_HPP

#include "Exceptions.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include <vector>
#include <string>
#include <map>

namespace Timetables {
	namespace Structures {
		class Stop; using StopPtrObserver = const Stop*;

		enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5 };		

		class Route {
		private:
			const std::string shortName;
			const std::wstring longName;
			const RouteType type;
			std::vector<StopPtrObserver> stopsSequence;
		public:
			Route(const std::string& shortName, const std::wstring& longName, RouteType type) :
				shortName(shortName), longName(longName), type(type) {}

			inline const std::string& GetShortName() const { return shortName; }
			inline const std::wstring& GetLongName() const { return longName; }
			inline const RouteType GetType() const { return type; }
			inline const std::vector<StopPtrObserver>& GetStopsSequence() const { return stopsSequence; }

			inline void SetStopsSequence(const std::vector<StopPtrObserver>& seq) { stopsSequence = seq; }
		};

		class Routes {
		private:
			std::map<std::string, Route> list;
		public:
			Routes(std::wistream&& routes);
			inline const Route& GetRoute(const std::string& id) const {
				auto it = list.find(id);
				if (it == list.cend()) throw Timetables::Exceptions::RouteNotFoundException(id);
				else return it->second;
			}
		};
	}
}

#endif // !ROUTES_HPP
