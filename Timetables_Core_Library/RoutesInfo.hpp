#ifndef ROUTES_INFO_HPP
#define ROUTES_INFO_HPP

#include <vector>
#include <string>

namespace Timetables {
	namespace Structures {
		enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5, Gondola = 6, Funicular = 7 };

		class RouteInfo {
		private:
			const std::string shortName;
			const std::wstring longName;
			const RouteType type;
		public:
			RouteInfo(const std::string& shortName, const std::wstring& longName, RouteType type) :
				shortName(shortName), longName(longName), type(type) {}

			inline const std::string& ShortName() const { return shortName; }
			inline const std::wstring& LongName() const { return longName; }
			inline const RouteType Type() const { return type; }
		};

		class RoutesInfo {
		private:
			std::vector<RouteInfo> list;
		public:
			RoutesInfo(std::wistream&& routesInfo);

			inline RouteInfo& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline RouteInfo& operator[](std::size_t id) { return list[id]; }
		};
	}
}

#endif // !ROUTES_INFO_HPP
