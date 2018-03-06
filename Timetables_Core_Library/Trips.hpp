#ifndef TRIP_HPP
#define TRIP_HPP

#include "Stops.hpp" // Reference to the stops in SetTimetables method.
#include "Routes.hpp" // Reference to the route in Trip class.
#include "RoutesInfo.hpp" // Reference to the routes info in Trips class ctor.
#include "Services.hpp" // Reference to the service in Trip class.
#include "StopTime.hpp" // StopTime used as a data member in Trip class.

namespace Timetables {
	namespace Structures {
		class Trip {
		private:
			const Route& route; // Reference to the route serving this trip.
			const Service& service; // Reference to the service that give us operating days for the trip.
			std::size_t departureTime; // Departure from the first stop in the trip. Seconds since midnight.
			std::vector<StopTime> stopTimes; // List of the stop times included in this trip.
		public:
			Trip(const Service& service, const Route& route, std::size_t departure) :
				service(service), departureTime(departure), route(route) { stopTimes.reserve(route.Stops().capacity()); }

			inline const std::vector<StopTime>& StopTimes() const { return stopTimes; }
			inline const Route& Route() const { return route; }
			inline const Service& Service() const { return service; }
			inline const std::size_t Departure() const { return departureTime; }
			
			inline void AddToTrip(const StopTime& stopTime) { stopTimes.push_back(stopTime); }
		};

		class Trips {
		private:
			std::vector<Trip> list; // List of all the trips, index of the item is also identificator for the trip.
		public:
			Trips(std::istream&& trips, RoutesInfo& routesInfo, Routes& routes, Services& services);

			inline Trip& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline Trip& operator[](std::size_t id) { return list[id]; }

			void SetTimetables(std::istream&& stopTimes, Stops& stops);
		};
	}
}

#endif // !TRIP_HPP
