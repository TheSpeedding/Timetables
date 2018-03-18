#ifndef STATIONS_HPP
#define STATIONS_HPP

#include <vector> // Data sctructure for stations.
#include <string> // A need for names.
#include <algorithm> // Required for finding the station.
#include <map> // Structure for departures.
#include <ctime> // time_t is defined here.
// #include "stops.hpp" // References to child stops from station.
#include "exceptions.hpp" // StopNotFoundException in stations find method.

namespace Timetables {
	namespace Structures {
		class stop; // Forward declaration.
		class stop_time; // Forward declaration.
		class date_time; // Forward declaration.

		// Class collecting information about station.
		class station {
		private:
			const std::wstring name_; // Name of the station is also the name for a stop.
			std::vector<const stop*> child_stops_; // If the station is itself a stop, then the vector contains only one item.
			std::multimap<date_time, const stop_time*> departures_; // Sorted by departure times. Vector might be sufficient.
		public:
			station(const std::wstring& name) : name_(name) {}

			inline const std::wstring& name() const { return name_; } // Name of this station.
			inline const std::vector<const stop*> child_stops() const { return child_stops_; } // List of child stops that this station contain.
			inline const std::multimap<date_time, const stop_time*>& departures() const { return departures_; } // Departures from this station.

			void add_departure(const stop_time& stop_time); // Adds a departure. Used in initialization.
			inline void add_child_stop(const stop& stop) { child_stops_.push_back(&stop); } // Adds child stop for the station.
		};

		// Class collecting information about collection of the stations.
		class stations {
		private:
			std::vector<station> list;// List of all the stations, index of the item is also identificator for the station.
		public:
			stations(std::wistream&& stations);

			inline station& at(std::size_t id) { return list.at(id); }	// Gets the station with given id.
			inline const station& at(std::size_t id) const { return list.at(id); }	// Gets the station with given id.
			inline const std::size_t size() const { return list.size(); } // Gets count of items in the collection.
			inline station& operator[](std::size_t id) { return list[id]; } // Gets the station with given id.

			inline const station& find(const std::wstring& name) const { // Looks for the station with given name.
				auto it = std::find_if(list.cbegin(), list.cend(), [=](const station& st) { return st.name() == name; });
				if (it == list.cend()) throw Timetables::Exceptions::station_not_found(name);
				return *it;
			}
		};
	}
}

#endif // !STATIONS_HPP
