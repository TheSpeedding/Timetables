#ifndef ROUTES_INFO_HPP
#define ROUTES_INFO_HPP

#include <vector> // Data structure for routes info.
#include <string> // Used for names.

namespace Timetables {
	namespace Structures {
		enum mean_of_transport { tram = 1, subway = 2, rail = 4, bus = 8, ship = 16, cablecar = 32, gondola = 64, funicular = 128 }; // Mean of the transport.
		inline mean_of_transport operator& (mean_of_transport a, mean_of_transport b) { return static_cast<mean_of_transport>(static_cast<int>(a) & static_cast<int>(b)); }

		// Class collecting basic information about route.
		class route_info {
		private:
			const std::wstring short_name_; // Short name of the route. E.g. A.
			const std::wstring long_name_; // Long name of the rout. E.g. Depo Hostivaø - Nemocnice Motol.
			std::size_t color_; // Color of the route used in GUI in HEX format.
			const mean_of_transport type_; // Mean of the transport.
		public:
			route_info(const std::wstring& short_name, const std::wstring& long_name, mean_of_transport type, std::size_t color) :
				short_name_(short_name), long_name_(long_name), type_(type), color_(color) {}

			inline const std::wstring& short_name() const { return short_name_; } // Short name of the route.
			inline const std::wstring& long_name() const { return long_name_; } // Long name of the route.
			inline const mean_of_transport type() const { return type_; } // Mean of the transport used in the route.
			inline const std::size_t color() const { return color_; } // Color label of the route.
		};

		// Class collecting information about collection of routes info.
		class routes_info {
		private:
			std::vector<route_info> list; // List of all routes info, index of the item is also identificator for route info.
		public:
			routes_info(std::wistream&& routesInfo);

			inline route_info& at(std::size_t id) { return list.at(id); } // Gets the route info with given id.
			inline const route_info& at(std::size_t id) const { return list.at(id); } // Gets the route info with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline route_info& operator[](std::size_t id) { return list[id]; } // Gets the route info with given id.
		};
	}
}

#endif // !ROUTES_INFO_HPP