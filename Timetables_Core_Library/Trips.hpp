#ifndef TRIP_HPP
#define TRIP_HPP

#include "Stops.hpp"
#include "Utilities.hpp"
#include "RoutesInfo.hpp"
#include "Services.hpp"
#include "StopTime.hpp"
#include <string>
#include <memory>

namespace Timetables {
	namespace Structures {
		class Routes;
		class Stops;
		class StopTime;
		class Trip {
		private:
			std::wstring headsign;
			const RouteInfo& routeInfo;
			const Service& service;
			std::vector<StopTime> stopTimes;
		public:
			Trip(const RouteInfo& routeInfo, const Service& service, const std::wstring& headsign, std::size_t numberOfStopTimes) :
				routeInfo(routeInfo), service(service), headsign(headsign) { stopTimes.reserve(numberOfStopTimes); }

			inline const std::vector<StopTime>& StopTimes() const { return stopTimes; }
			inline const RouteInfo& RouteInfo() const { return routeInfo; }
			inline const std::wstring& Headsign() const { return headsign; }
			inline const Service& Service() const { return service; }

			inline void AddToTrip(const StopTime& stopTime) { stopTimes.push_back(stopTime); }
		};

		class Trips {
		private:
			std::vector<Trip> list;
		public:
			Trips(std::wistream&& trips, RoutesInfo& routesInfo, Routes& routes, Services& services);

			inline Trip& Get(std::size_t id) { return list.at(id); }
			inline const std::size_t Count() const { return list.size(); }
			inline Trip& operator[](std::size_t id) { return list[id]; }

			void SetTimetables(std::istream&& stopTimes, Stops& stops);
		};
	}
}

#endif // !TRIP_HPP
