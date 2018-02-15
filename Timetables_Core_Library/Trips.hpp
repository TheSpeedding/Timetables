#ifndef TRIP_HPP
#define TRIP_HPP

#include "Stops.hpp"
#include "Exceptions.hpp"
#include "Utilities.hpp"
#include "Routes.hpp"
#include "Services.hpp"
#include "Shapes.hpp"
#include <string>
#include <memory>

namespace Timetables {
	namespace Structures {
		class StopTime;
		class Stops;
		class Route;
		class Routes;
		class Shapes;

		class Trip {
		private:
			std::wstring headsign;
			const Route& route;
			const Service& service;
			const ShapesSequence& shapes;
			std::vector<std::unique_ptr<StopTime>> stopTimes;
		public:
			Trip(const Route& route, const Service& service, const ShapesSequence& shapes, const std::wstring& headsign) :
				route(route), service(service), shapes(shapes), headsign(headsign) {}

			inline const std::vector<std::unique_ptr<StopTime>>& GetStopTimes() const { return stopTimes; }
			inline const Route& GetRoute() const { return route; }
			inline const std::wstring& GetHeadsign() const { return headsign; }
			inline const Service& GetService() const { return service; }
			inline const ShapesSequence& GetShapesSequence() const { return shapes; }

			inline void AddToTrip(std::unique_ptr<StopTime> stopTime) { stopTimes.push_back(move(stopTime)); }
		};

		class Trips {
		private:
			std::vector<Trip> list;
		public:
			Trips(std::wistream&& trips, const Routes& routes, const Services& services, const Shapes& shapes);

			inline Trip& GetTrip(std::size_t id) {
				if (id > list.size()) throw Timetables::Exceptions::TripNotFoundException(id);
				else return list[id - 1];
			}

			void SetTimetables(std::istream&& stop_times, Stops& stops);
		};
	}
}

#endif // !TRIP_HPP
