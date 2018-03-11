#ifndef ROUTES_INFO_HPP
#define ROUTES_INFO_HPP

#include <vector> // Data structure for routes info.
#include <string> // Used for names.

namespace Timetables {
	namespace Structures {
		enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5, Gondola = 6, Funicular = 7 }; // Mean of the transport.

		// Class collecting basic information about route.
		class RouteInfo {
		private:
			const std::string shortName; // Short name of the route. E.g. A.
			const std::wstring longName; // Long name of the rout. E.g. Depo Hostivaø - Nemocnice Motol.
			const RouteType type; // Mean of the transport.
			std::size_t color; // Color of the route used in GUI in HEX format.
		public:
			RouteInfo(const std::string& shortName, const std::wstring& longName, RouteType type, std::size_t color) :
				shortName(shortName), longName(longName), type(type), color(color) {}

			inline const std::string& ShortName() const { return shortName; } // Short name of the route.
			inline const std::wstring& LongName() const { return longName; } // Long name of the route.
			inline const RouteType Type() const { return type; } // Mean of the transport used in the route.
			inline const std::size_t Color() const { return color; } // Color label of the route.
		};

		// Class collecting information about collection of routes info.
		class RoutesInfo {
		private:
			std::vector<RouteInfo> list; // List of all routes info, index of the item is also identificator for route info.
		public:
			RoutesInfo(std::wistream&& routesInfo);

			inline RouteInfo& Get(std::size_t id) { return list.at(id); } // Gets the route info with given id.
			inline const RouteInfo& Get(std::size_t id) const { return list.at(id); } // Gets the route info with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline RouteInfo& operator[](std::size_t id) { return list[id]; } // Gets the route info with given id.
		};
	}
}

#endif // !ROUTES_INFO_HPP