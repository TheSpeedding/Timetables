#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include <memory>
#include <algorithm>
#include <vector>
#include <unordered_map>
#include <unordered_set>
#include "GtfsFeed.hpp"
#include "Routes.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Trips.hpp"

namespace Timetables {
	namespace Structures {
		class Trip;
		class StopTime; using StopTimePtrObserver = const StopTime*;
		
		
		class Journey {
		private:
			std::vector<std::pair<TripPtrObserver, StopPtrObserver>> journeySegments;
		public:
			inline const std::vector<std::pair<TripPtrObserver, StopPtrObserver>>&  GetSegments() const { return journeySegments; }
			inline const Time& GetDeparture() const {
				return std::find_if(journeySegments.cbegin()->first->GetStopTimes().cbegin(), journeySegments.cbegin()->first->GetStopTimes().cend(),
					[=](const std::unique_ptr<Timetables::Structures::StopTime>& st) { return &st->GetStop() == journeySegments.cbegin()->second; })->get()->GetDeparture();
			}
			inline const Time& GetArrival() const {
				return std::find_if((journeySegments.cend() - 1)->first->GetStopTimes().cbegin(), (journeySegments.cend() - 1)->first->GetStopTimes().cend(),
					[=](const std::unique_ptr<Timetables::Structures::StopTime>& st) { return &st->GetStop() == (journeySegments.cend() - 1)->second; })->get()->GetArrival();
			} // Toto je špatnì. Když to jde pøes pùlnoc...
			inline const Time& GetTotalDuration() const { return GetArrival() - GetDeparture(); }

			inline void AddToJourney(const Timetables::Structures::Trip& trip, const Timetables::Structures::Stop& stop) { journeySegments.push_back(std::make_pair(&trip, &stop)); }
		};
		
	}

	namespace Algorithms {
		using TripPtrObserver = const Timetables::Structures::Trip*;
		using RoutePtrObserver = const Timetables::Structures::Route*;
		using StationPtrObserver = const Timetables::Structures::Station*;

		class Router {
		private:
			StationPtrObserver source;
			StationPtrObserver target;
			const Timetables::Structures::Datetime& earliestDeparture;
			const std::size_t transfers;
			const std::size_t count;

			std::vector<Timetables::Structures::Journey> fastestJourneys;

			std::vector<std::unordered_map<Timetables::Structures::StopPtrObserver, Timetables::Structures::Datetime>> labels;
			std::vector<std::unordered_map<Timetables::Structures::StopPtrObserver, Timetables::Structures::Journey>> journeys;
			std::unordered_map<Timetables::Structures::StopPtrObserver, Timetables::Structures::Datetime> tempLabels;
			std::unordered_set<Timetables::Structures::StopPtrObserver> markedStops;
			std::unordered_map<Timetables::Structures::RoutePtrObserver, Timetables::Structures::StopPtrObserver> activeRoutes;

			void AccumulateRoutes();
			void TraverseEachRoute();
			void LookAtFootpaths();
			TripPtrObserver FindEarliestTrip(const Timetables::Structures::Route& route, const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Stop& stop);
		public:
			Router(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& earliestDeparture, const std::size_t count, const std::size_t transfers);
			
			void ObtainJourney();

			const std::vector<Timetables::Structures::Journey>& GetJourneys() const { return fastestJourneys; }
		};
	}
}

#endif // !ROUTER_HPP
