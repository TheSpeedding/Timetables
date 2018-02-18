#ifndef ROUTES_INFO_HPP
#define ROUTES_INFO_HPP

#include "Exceptions.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Trips.hpp"
#include <vector>
#include <string>
#include <map>

namespace Timetables {
	namespace Structures {
		enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5 };		

		class RouteInfo {
		private:
			const std::string shortName;
			const std::wstring longName;
			const RouteType type;
		public:
			RouteInfo(const std::string& shortName, const std::wstring& longName, RouteType type) :
				shortName(shortName), longName(longName), type(type) {}

			inline const std::string& GetShortName() const { return shortName; }
			inline const std::wstring& GetLongName() const { return longName; }
			inline const RouteType GetType() const { return type; }
		};

		class RoutesInfo {
		private:
			std::map<std::string, RouteInfo> list;
		public:
			RoutesInfo(std::wistream&& routes);
			inline const RouteInfo& GetRouteInfo(const std::string& id) const {
				auto it = list.find(id);
				if (it == list.cend()) throw Timetables::Exceptions::RouteNotFoundException(id);
				else return it->second;
			}
		};
	}
}

#endif // !ROUTES_INFO_HPP
