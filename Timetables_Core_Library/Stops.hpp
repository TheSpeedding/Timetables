#ifndef STOPS_HPP
#define STOPS_HPP

#include "Utilities.hpp" // This may be removed later.

#include "Stations.hpp" // Reference to the parent station in stop.
#include "Routes.hpp" // Reference to the route in stop (throughgoing routes).
#include "StopTime.hpp" // Reference to stop times in stop (departures).
#include <string> // String is a need for names.
#include <vector> // Data structure for stops.
#include <map> // Data structure for departures and footpaths.

namespace Timetables {
	namespace Structures {
		// Class collecting information about stop.
		class Stop {
		private:
			const Station& parentStation; // Refernece to the parent stations. A need for departure board. Name of the stop can be accessed via parent station (they have to be the same).
			std::multimap<DateTime, const StopTime*> departures; // Sorted by departure times. Vector might be sufficient. TO-DO: Move departures to the station might (= will definitely) be better.
			std::multimap<std::size_t, const Stop*> footpaths; // Stops reachable in walking-distance (< 10 min.) from this stop.
			std::vector<const Route*> throughgoingRoutes; // Routes that goes through this stop.
		public:
			Stop(const Station& parentStation) : parentStation(parentStation) {}

			inline const Station& ParentStation() const { return parentStation; } // Parent station for this stop.
			inline const std::wstring& Name() const { return parentStation.Name(); } // Name of this stop.
			inline const std::multimap<DateTime, const StopTime*>& Departures() const { return departures; } // Departures from this stop.
			inline const std::vector<const Route*>& ThroughgoingRoutes() const { return throughgoingRoutes; } // Routes that goes through this stop.
			inline const std::multimap<std::size_t, const Stop*>& Footpaths() const { return footpaths; } // Stops reachable in walking-distance (< 10 min.) from this stop.

			inline void AddThroughgoingRoute(const Route& route) { throughgoingRoutes.push_back(&route); } // Adds routes that goes through this stop. Used in initialization.
			inline void AddFootpath(const Stop& stop, std::size_t time) { footpaths.insert(std::make_pair(time, &stop)); } // Adds a footpath. Used in initialization.
			void AddDeparture(const StopTime& stopTime); // Adds a departure. Used in initialization.
		};

		// Class collecting information about collection of the stops.
		class Stops {
		private:
			std::vector<Stop> list; // List of all the stops, index of the item is also identificator for the stop.
		public:
			Stops(std::wistream&& stops, std::istream&& footpaths, Stations& stations);
						
			inline Stop& Get(std::size_t id) { return list.at(id); } // Gets the stop with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Stop& operator[](std::size_t id) { return list[id]; } // Gets the stop with given id.

			void SetThroughgoingRoutesForStops(Routes& routes); // Sets throughoing routes for every single stop. Based on the data loaded later than stops.
		};

	}
}

#endif // !STOPS_HPP