#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include <algorithm>
#include <vector>
#include <unordered_map>
#include <unordered_set>
#include "StopTime.hpp"
#include "DataFeed.hpp"

namespace Timetables {
	namespace Structures {
		class Trip;
		class StopTime;

		class JourneySegment {
		private:
			const Trip& trip;
			std::vector<StopTime>::const_iterator sourceStop;
			std::vector<StopTime>::const_iterator targetStop;
			const Timetables::Structures::DateTime arrival;
		public:
			JourneySegment(const Trip& trip, const Timetables::Structures::DateTime& arrival, const Stop& source, const Stop& target);

			inline const Trip& Trip() const { return trip; }
			inline const Stop& SourceStop() const { return sourceStop->Stop(); }
			inline const Stop& TargetStop() const { return targetStop->Stop(); }
			inline const DateTime DepartureFromSource() const { return arrival.AddSeconds((-1) * (targetStop->Arrival() - sourceStop->Departure())); }
			inline const DateTime& ArrivalAtTarget() const { return arrival; }
			const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> IntermediateStops() const;
		};

		class Journey {
		private:
			std::vector<JourneySegment> journeySegments;
			std::vector<std::size_t> transfers; // There is a footpath between every two journey segments.
		public:
			Journey(const JourneySegment& js) { AddToJourney(js); }

			inline const DateTime DepartureTime() const { return journeySegments.cbegin()->DepartureFromSource(); }
			inline const DateTime& ArrivalTime() const { return (journeySegments.cend() - 1)->ArrivalAtTarget(); }
			inline const int TotalDuration() const { return DateTime::Difference(ArrivalTime(), DepartureTime()); }

			inline const std::vector<JourneySegment>& JourneySegments() const { return journeySegments; }
			inline const std::vector<std::size_t>& Transfers() const { return transfers; }

			inline void AddToJourney(const JourneySegment& js) { journeySegments.push_back(js); }
			inline void AddFootpath(std::size_t duration) { transfers.push_back(duration); }
		};
	}

	namespace Algorithms {
		class Router {
		private:
			const Timetables::Structures::Station& source;
			const Timetables::Structures::Station& target;
			const Timetables::Structures::DateTime earliestDeparture;
			const std::size_t maxTransfers;
			const std::size_t count;
			
			std::vector<std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::DateTime>> labels;
			std::vector<std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::Journey>> journeys;
			std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::DateTime> tempLabels;
			std::unordered_set<const Timetables::Structures::Stop*> markedStops;
			std::unordered_map<const Timetables::Structures::Route*, const Timetables::Structures::Stop*> activeRoutes;

			std::multimap<Timetables::Structures::DateTime, const Timetables::Structures::Journey> fastestJourneys; // Fastest journeys found by the router, key is the arrival time to the target station. That means, they are sorted.

			void AccumulateRoutes();
			void TraverseEachRoute();
			void LookAtFootpaths();
			const Timetables::Structures::Journey& ObtainJourney(const Timetables::Structures::DateTime& departure); // Returns the best journey obtained in this round.

			std::pair<const Timetables::Structures::Trip*, Timetables::Structures::DateTime>
				FindEarliestTrip(const Timetables::Structures::Route& route, const Timetables::Structures::DateTime& arrival, std::size_t stopIndex);
		public:
			Router(const Timetables::Structures::DataFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::DateTime& earliestDeparture, const std::size_t count, const std::size_t transfers) :
				maxTransfers(transfers + 1), count(count), earliestDeparture(earliestDeparture), source(feed.Stations().Find(s)), target(feed.Stations().Find(t)) {}

			void ObtainJourneys();

			const std::multimap<Timetables::Structures::DateTime, const Timetables::Structures::Journey>& ShowJourneys() const { return fastestJourneys; }
		};
	}
}

#endif // !ROUTER_HPP