#ifndef TRIP_HPP
#define TRIP_HPP

#include "Services.hpp" // Reference to the service in Trip class.
#include "RoutesInfo.hpp" // Reference to the routes info in Trips class ctor.
#include "Stops.hpp" // Reference to the stops in SetTimetables method.
#include "Routes.hpp" // Reference to the route in Trip class.
// #include "StopTime.hpp" // StopTime used as a data member in Trip class.
#include <vector> // Data structure for trips.

namespace Timetables {
	namespace Structures {
		class StopTime; // Forward declaration.

		// Class collecting information about one trip.
		class Trip {
		private:
			const Route& route; // Reference to the route serving this trip.
			const Service& service; // Reference to the service that give us operating days for the trip.
			std::size_t departureTime; // Departure from the first stop in the trip. Seconds since midnight.
			std::vector<StopTime> stopTimes; // List of the stop times included in this trip.
		public:
			Trip(const Service& service, const Route& route, std::size_t departure) :
				service(service), departureTime(departure), route(route) { stopTimes.reserve(route.Stops().capacity()); }

			inline const std::vector<StopTime>& StopTimes() const { return stopTimes; } // Gets list of stop times belonging to the trip.
			inline const Route& Route() const { return route; } // Gets information about route.
			inline const Service& Service() const { return service; } // Gets service for this trip.
			inline const std::size_t Departure() const { return departureTime; } // Gets departure from the first stop of the trip, seconds since midnight.
			
			inline void AddToTrip(const StopTime& stopTime) { stopTimes.push_back(stopTime); } // Adds stop time into the trip. Used in initialization.
		};

		// Class collecting information about collection of the trips.
		class Trips {
		private:
			std::vector<Trip> list; // List of all the trips, index of the item is also identificator for the trip.
		public:
			Trips(std::istream&& trips, RoutesInfo& routesInfo, Routes& routes, Services& services);

			inline Trip& Get(std::size_t id) { return list.at(id); } // Gets the trip with given id.
			inline const std::size_t Count() const { return list.size(); } // Gets count of items in the collection.
			inline Trip& operator[](std::size_t id) { return list[id]; } // Gets the trip with given id.

			void SetTimetables(std::istream&& stopTimes, Stops& stops); // Loads the file with stop times and sets timetables.
		};
	}
}

#endif // !TRIP_HPP
