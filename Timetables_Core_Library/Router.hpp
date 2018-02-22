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

		class StopDatetime {
		private:
			const StopTime& stopTime;
			const Date arrivalDate;
			const Date departureDate;
		public:
			StopDatetime(const StopTime& stopTime, const Date& arrivalDate, const Date& departureDate) : 
				stopTime(stopTime), arrivalDate(arrivalDate), departureDate(departureDate) {}

			inline const Trip& GetTrip() const { return stopTime.GetTrip(); }
			inline const Stop& GetStop() const { return stopTime.GetStop(); }
			inline const Datetime GetArrival() const { return Datetime(arrivalDate, stopTime.GetArrival()); }
			inline const Datetime GetDeparture() const { return Datetime(departureDate, stopTime.GetDeparture()); }
		};

		class TripSegment {
		private:
			std::vector<StopDatetime> stops;
		public:
			TripSegment(const StopTime& stopTime, const Date& arrivalDate, const Date& departureDate) { AddToTripSegment(stopTime, arrivalDate, departureDate); }

			inline void AddToTripSegment(const StopTime& stopTime, const Date& arrivalDate, const Date& departureDate) { stops.push_back(StopDatetime(stopTime, arrivalDate, departureDate)); }

			inline const std::wstring& GetHeadsign() const { return stops.at(0).GetTrip().GetHeadsign(); }
			inline const RouteInfo& GetRouteInfo() const { return stops.at(0).GetTrip().GetRouteInfo(); }
			const std::vector<StopDatetime>& GetStopDatetimes() const { return stops; }
		};

		class Journey {
		private:
			std::vector<TripSegment> tripSegments;
		public:
			Journey(const TripSegment& ts) { AddToJourney(ts); }

			inline const std::vector<TripSegment>&  GetSegments() const { return tripSegments; }
			inline const Datetime& GetDeparture() const { return tripSegments.cbegin()->GetStopDatetimes().cbegin()->GetDeparture(); }
			inline const Datetime& GetArrival() const { return ((tripSegments.cend() - 1)->GetStopDatetimes().cend() - 1)->GetArrival(); }
			// inline const Datetime& GetTotalDuration() const { return GetArrival() - GetDeparture(); }

			inline void AddToJourney(const TripSegment& ts) { tripSegments.push_back(ts); }
			
			inline bool operator< (const Journey& other) const { return GetArrival() < other.GetArrival(); }
			inline bool operator> (const Journey& other) const { return GetArrival() > other.GetArrival(); }
			inline bool operator==(const Journey& other) const { return GetArrival() == other.GetArrival(); }
		};
	}

	namespace Algorithms {
		using TripPtrObserver = const Timetables::Structures::Trip*;
		Timetables::Structures::Journey FindRoute(const Timetables::Structures::GtfsFeed& feed, const Timetables::Structures::Station& s, const Timetables::Structures::Station& t, const Timetables::Structures::Datetime& datetime, const std::size_t transfers);
		std::vector<Timetables::Structures::Journey> FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& datetime, const std::size_t count, const std::size_t transfers = 10);
		TripPtrObserver GetEarliestTrip(const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Route& route, const Timetables::Structures::Stop& stop);
	}
}

#endif // !ROUTER_HPP
