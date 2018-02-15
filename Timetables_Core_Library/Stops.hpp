#ifndef STOPS_HPP
#define STOPS_HPP

#include "Utilities.hpp"
#include "Exceptions.hpp"
#include "Trips.hpp"
#include <string>
#include <vector>
#include <map>

namespace Timetables {
	namespace Structures {
		class Trip;
		class StopTime; using StopTimePtrObserver = const StopTime*;
		class Stop; using StopPtrObserver = const Stop*;

		class StopTime {
		private:
			const Stop& stop;
			const Trip& trip;
			const Time arrival;
			const Time departure;
		public:
			StopTime(const Stop& stop, const Trip& trip, const Time& arrival, const Time& departure) :
				stop(stop), trip(trip), arrival(arrival), departure(departure) {}

			inline const Trip& GetTrip() const { return trip; }
			inline const Stop& GetStop() const { return stop; }
			inline const Time& GetArrival() const { return arrival; }
			inline const Time& GetDeparture() const { return departure; }
		};

		class Station {
		private:
			const std::wstring name;
			const GpsCoords coords;
			std::map<std::string, StopPtrObserver> childStops; // If the station is itself a stop, then the map contains only one item.
		public:
			Station(const std::wstring& name, double latitude, double longitude) :
				name(name), coords(GpsCoords(latitude, longitude)) {}
			Station(const std::wstring& name, const GpsCoords& coords) :
				name(name), coords(coords) {}

			inline void AddStop(const std::string& id, const Stop& stop) { childStops.insert(std::make_pair(id, &stop)); }

			inline const std::map<std::string, StopPtrObserver>& GetStops() const { return childStops; }
			inline const std::wstring& GetName() const { return name; }
			inline const GpsCoords& GetLocation() const { return coords; }
		};


		class Stop {
		private:
			const std::wstring name;
			const GpsCoords coords;
			Station* parentStation;
			std::multimap<Time, StopTimePtrObserver> departures; // Sorted by departure times.
		public:
			using PtrObserver = const Stop*;
			Stop(const std::wstring& name, double latitude, double longitude) :
				name(name), coords(GpsCoords(latitude, longitude)), parentStation(nullptr) {}
			Stop(const std::wstring& name, double latitude, double longitude, Station* parentStation) :
				name(name), coords(GpsCoords(latitude, longitude)), parentStation(parentStation) {}

			inline Station& GetParentStation() { return *parentStation; }
			inline const GpsCoords& GetLocation() const { return coords; }
			inline const std::wstring& GetName() const { return name; }
			inline const std::multimap<Time, StopTimePtrObserver>& GetDepartures() const { return departures; }

			inline void SetParentStation(Station& parent) { parentStation = &parent; }
			inline void AddDeparture(const Time& time, const StopTime& stopTime) { departures.insert(std::make_pair(time, &stopTime)); }
			
			inline bool operator==(const Stop& other) const { return coords == other.coords; }
		};

		class Stops {
		private:
			std::map<std::string, Stop> stopsList; // Indexed by stop ID, used for computations.
			std::map<std::wstring, Station> stationsList; // User-friendly structure indexed by station name. Contains all the stop with such a name.
		public:
			Stops(std::wistream&& stops);

			inline Stop& GetStop(const std::string& id) {
				auto it = stopsList.find(id);
				if (it == stopsList.cend()) throw Timetables::Exceptions::StopNotFoundException(id);
				else return it->second;
			}
			const Station& GetStation(const std::wstring& name) const;
			const std::map<std::string, Stop>& GetStops() const { return stopsList; }
		};

	}
}

#endif // !STOPS_HPP