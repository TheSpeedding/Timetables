#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include <vector>
#include "GtfsFeed.hpp"
#include "Routes.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Trips.hpp"

namespace Timetables {
	namespace Structures {
		class Trip;
		class StopTime; using StopTimePtrObserver = const StopTime*;

		class TripSegment {
		private:
			const Trip& trip;
			const std::vector<std::unique_ptr<StopTime>>::const_iterator begin;
			const std::vector<std::unique_ptr<StopTime>>::const_iterator end;
		public:
			TripSegment(const Trip& trip, const std::vector<std::unique_ptr<StopTime>>::const_iterator begin, const std::vector<std::unique_ptr<StopTime>>::const_iterator end) : 
				trip(trip), begin(begin), end(end) {}
			inline const std::wstring& GetHeadsign() const { return trip.GetHeadsign(); }
			inline const RouteInfo& GetRouteInfo() const { return trip.GetRouteInfo(); }
			const std::vector<StopTimePtrObserver> GetStopTimes() const;
		};
	}

	namespace Algorithms {
		using TripPtrObserver = const Timetables::Structures::Trip*;
		void FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& datetime, const std::size_t count, const std::size_t transfers = 10);
		TripPtrObserver GetEarliestTrip(const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Route& route, const Timetables::Structures::Stop& stop);
	}
}

#endif // !ROUTER_HPP
