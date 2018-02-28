#ifndef STOPS_HPP
#define STOPS_HPP

#include "Utilities.hpp"
#include "Stations.hpp"
#include "Routes.hpp"
#include "StopTime.hpp"
#include <string>
#include <vector>
#include <algorithm>
#include <map>

namespace Timetables {
	namespace Structures {
		class Route;
		class Routes;
		class Station;
		class Stations;
		class Stop {
		private:
			const std::wstring name;
			const GpsCoords coords;
			const Station& parentStation;
			std::multimap<DateTime, const StopTime*> departures; // Sorted by departure times.
			std::multimap<std::size_t, const Stop*> footpaths; // Stops reachable in walking-distance (< 15 min.) from this stop.
			std::vector<const Route*> throughgoingRoutes; // Routes that goes through this stop.
		public:
			Stop(const std::wstring& name, double latitude, double longitude, const Station& parentStation) :
				name(name), coords(GpsCoords(latitude, longitude)), parentStation(parentStation) {}

			inline const Station& ParentStation() const { return parentStation; }
			inline const GpsCoords& Location() const { return coords; }
			inline const std::wstring& Name() const { return name; }
			inline const std::multimap<DateTime, const StopTime*>& Departures() const { return departures; }
			inline const std::vector<const Route*>& ThroughgoingRoutes() const { return throughgoingRoutes; }
			inline const std::multimap<std::size_t, const Stop*>& Footpaths() const { return footpaths; }

			inline void AddThroughgoingRoute(const Route& route) { throughgoingRoutes.push_back(&route); }
			inline void AddDeparture(const StopTime& stopTime) { departures.insert(std::make_pair(stopTime.Departure(), &stopTime)); }
			inline void AddFootpath(const Stop& stop, std::size_t time) { footpaths.insert(std::make_pair(time, &stop)); }

			inline bool operator==(const Stop& other) const { return coords == other.coords; }
		};

		class Stops {
		private:
			std::vector<Stop> list;
		public:
			Stops(std::wistream&& stops, Stations& stations);

			void SetFootpaths(std::istream&& footpaths);
			
			inline Stop& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline Stop& operator[](std::size_t id) { return list[id]; }

			void SetThroughgoingRoutesForStops(Routes& routes);
		};

	}
}

#endif // !STOPS_HPP