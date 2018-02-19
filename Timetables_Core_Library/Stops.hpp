#ifndef STOPS_HPP
#define STOPS_HPP

#include "Utilities.hpp"
#include "Exceptions.hpp"
#include "RoutesInfo.hpp"
#include "Routes.hpp"
#include "Trips.hpp"
#include <exception>
#include <string>
#include <vector>
#include <map>

namespace Timetables {
	namespace Structures {
		class Trip;
		class Routes;
		class StopTime; using StopTimePtrObserver = const StopTime*;
		class Stop; using StopPtrObserver = const Stop*;
		class Route; using RoutePtrObserver = const Route*;

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
			std::vector<StopPtrObserver> childStops; // If the station is itself a stop, then the vector contains only one item.
		public:
			Station(const std::wstring& name, double latitude, double longitude) :
				name(name), coords(GpsCoords(latitude, longitude)) {}
			Station(const std::wstring& name, const GpsCoords& coords) :
				name(name), coords(coords) {}

			inline void AddStop(const Stop& stop) { childStops.push_back(&stop); }

			inline const std::vector<StopPtrObserver>& GetChildStops() const { return childStops; }
			inline const std::wstring& GetName() const { return name; }
			inline const GpsCoords& GetLocation() const { return coords; }
		};


		class Stop {
		private:
			const std::wstring name;
			const GpsCoords coords;
			Station* parentStation;
			std::multimap<Time, StopTimePtrObserver> departures; // Sorted by departure times.
			std::multimap<int, StopPtrObserver> footpaths; // Stops reachable in walking-distance (< 15 min.) from this stop.
			std::vector<RoutePtrObserver> throughgoingRoutes; // Routes that goes through this stop.
		public:
			Stop(const std::wstring& name, double latitude, double longitude) :
				name(name), coords(GpsCoords(latitude, longitude)), parentStation(nullptr) {}
			Stop(const std::wstring& name, double latitude, double longitude, Station* parentStation) :
				name(name), coords(GpsCoords(latitude, longitude)), parentStation(parentStation) {}

			inline Station& GetParentStation() { return *parentStation; }
			inline const GpsCoords& GetLocation() const { return coords; }
			inline const std::wstring& GetName() const { return name; }
			inline const std::multimap<Time, StopTimePtrObserver>& GetDepartures() const { return departures; }
			inline const std::vector<RoutePtrObserver>& GetRoutes() const { return throughgoingRoutes; }
			inline const std::multimap<int, StopPtrObserver>& GetFootpaths() const { return footpaths; }

			inline void SetParentStation(Station& parent) {
				if (parentStation != nullptr) throw std::runtime_error("Parent station already set.");
				parentStation = &parent;
			}

			inline void AddRoute(const Route& route) { throughgoingRoutes.push_back(&route); }
			inline void AddDeparture(const Time& time, const StopTime& stopTime) { departures.insert(std::make_pair(time, &stopTime)); }
			inline void AddFootpath(const Stop& stop, int time) { footpaths.insert(std::make_pair(time, &stop)); }

			inline bool operator==(const Stop& other) const { return coords == other.coords; }
		};

		class Stops {
		private:
			std::map<std::string, Stop> stopsList; // Indexed by stop ID, used for computations.
			std::map<std::wstring, Station> stationsList; // User-friendly structure indexed by station name. Contains all the stop with such a name.

			inline Stop& GetStop(const Stop& ptr) { // I have const reference to the stop from another list but for my purpose I need the non-const.
				for (auto&& stop : stopsList) if (&stop.second == &ptr) return stop.second; // TO-DO: Možná lehce pøedìlat, zamyslet se nad návrhem.
				throw Timetables::Exceptions::InvalidDataFormatException("Stop not found in the list.");
			}
		public:
			Stops(std::wistream&& stops);

			inline Stop& GetStop(const std::string& id) {
				auto it = stopsList.find(id);
				if (it == stopsList.cend()) throw Timetables::Exceptions::StopNotFoundException(id);
				else return it->second;
			}

			const Station& GetStation(const std::wstring& name) const;
			const std::map<std::string, Stop>& GetStops() const { return stopsList; }
			const std::map<std::wstring, Station>& GetStations() const { return stationsList; }

			void SetThroughgoingRoutesForStops(const Routes& routes);

		};

	}
}

#endif // !STOPS_HPP