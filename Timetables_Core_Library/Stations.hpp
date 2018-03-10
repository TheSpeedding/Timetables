#ifndef STATIONS_HPP
#define STATIONS_HPP

#include <vector> // Data sctructure for stations.
#include <string> // A need for names.
#include <algorithm> // Required for finding the station.
// #include "Stops.hpp" // References to child stops from station.
#include "Exceptions.hpp" // StopNotFoundException in stations find method.

namespace Timetables {
	namespace Structures {
		class Stop; // Forward declaration.

		// Class collecting information about station.
		class Station {
		private:
			const std::wstring name; // Name of the station is also the name for a stop.
			std::vector<const Stop*> childStops; // If the station is itself a stop, then the vector contains only one item.
		public:
			Station(const std::wstring& name) : name(name) {}

			inline const std::wstring& Name() const { return name; } // Name of this station.
			inline const std::vector<const Stop*> ChildStops() const { return childStops; } // List of child stops that this station contain.

			inline void AddChildStop(const Stop& stop) { childStops.push_back(&stop); } // Adds child stop for the station.
		};

		// Class collecting information about collection of the stations.
		class Stations {
		private:
			std::vector<Station> list;// List of all the stations, index of the item is also identificator for the station.
		public:
			Stations(std::wistream&& stations);

			inline Station& Get(std::size_t id) { return list.at(id); }	// Gets the station with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Station& operator[](std::size_t id) { return list[id]; } // Gets the station with given id.

			inline const Station& Find(const std::wstring& name) const { // Looks for the station with given name.
				auto it = std::find_if(list.cbegin(), list.cend(), [=](const Station& st) { return st.Name() == name; });
				if (it == list.cend()) throw Timetables::Exceptions::StopNotFoundException(name);
				return *it;
			}
		};
	}
}

#endif // !STATIONS_HPP
