#ifndef STATIONS_HPP
#define STATIONS_HPP

#include <vector> // Data sctructure for stations.
#include <string> // A need for names.
#include <algorithm> // Required for finding the station.
#include <map> // Structure for departures.
#include <ctime> // time_t is defined here.
// #include "Stops.hpp" // References to child stops from station.
#include "Exceptions.hpp" // StopNotFoundException in stations find method.

namespace Timetables {
	namespace Structures {
		class Stop; // Forward declaration.
		class StopTime; // Forward declaration.
		class DateTime; // Forward declaration.

		// Class collecting information about station.
		class Station {
		private:
			const std::wstring name; // Name of the station is also the name for a stop.
			std::vector<const Stop*> childStops; // If the station is itself a stop, then the vector contains only one item.
			std::multimap<DateTime, const StopTime*> departures; // Sorted by departure times. Vector might be sufficient.
		public:
			Station(const std::wstring& name) : name(name) {}

			inline const std::wstring& Name() const { return name; } // Name of this station.
			inline const std::vector<const Stop*> ChildStops() const { return childStops; } // List of child stops that this station contain.
			inline const std::multimap<DateTime, const StopTime*>& Departures() const { return departures; } // Departures from this station.

			void AddDeparture(const StopTime& stopTime); // Adds a departure. Used in initialization.
			inline void AddChildStop(const Stop& stop) { childStops.push_back(&stop); } // Adds child stop for the station.
		};

		// Class collecting information about collection of the stations.
		class Stations {
		private:
			std::vector<Station> list;// List of all the stations, index of the item is also identificator for the station.
		public:
			Stations(std::wistream&& stations);

			inline Station& Get(std::size_t id) { return list.at(id); }	// Gets the station with given id.
			inline const Station& Get(std::size_t id) const { return list.at(id); }	// Gets the station with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Station& operator[](std::size_t id) { return list[id]; } // Gets the station with given id.

			inline const Station& Find(const std::wstring& name) const { // Looks for the station with given name.
				auto it = std::find_if(list.cbegin(), list.cend(), [=](const Station& st) { return st.Name() == name; });
				if (it == list.cend()) throw Timetables::Exceptions::StationNotFoundException(name);
				return *it;
			}
		};
	}
}

#endif // !STATIONS_HPP
