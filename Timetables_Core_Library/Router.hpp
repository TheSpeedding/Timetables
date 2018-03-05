#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <string>
#include <algorithm>
#include <vector>
#include <unordered_map>
#include <unordered_set>
#include "DataFeed.hpp"
#include "Routes.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"
#include "Trips.hpp"

namespace Timetables {
	namespace Structures {
		class Trip;
		class StopTime;

		class JourneySegment {
		private:
			const Trip& trip;
			std::vector<StopTime>::const_iterator sourceStop;
			std::vector<StopTime>::const_iterator targetStop;
			DateTime arrival;
		public:
			JourneySegment(const Trip& trip, const DateTime& arrival, const Stop& source, const Stop& target);

			inline const Trip& Trip() const { return trip; }
			inline const Stop& SourceStop() const { return sourceStop->Stop(); }
			inline const Stop& TargetStop() const { return targetStop->Stop(); }
			inline const DateTime DepartureFromSource() const { return arrival.AddSeconds((-1) * DateTime::Difference(targetStop->Arrival(), sourceStop->Departure())); }
			inline const DateTime& ArrivalAtTarget() const { return arrival; }
			const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> IntermediateStops() const;
		};

		class Journey {
		private:
			std::vector<JourneySegment> journeySegments;
		public:
			// TO-DO: ReconstructJourney, DepartureTime, ArrivalTime, TotalDuration
			Journey(const JourneySegment& js) { AddToJourney(js); }

			inline const DateTime DepartureTime() const { return journeySegments.cbegin()->DepartureFromSource(); }
			inline const DateTime& ArrivalTime() const { return (journeySegments.cend() - 1)->ArrivalAtTarget(); }
			inline const int TotalDuration() const { return DateTime::Difference(ArrivalTime(), DepartureTime()); }

			inline const std::vector<JourneySegment>& JourneySegments() const { return journeySegments; }

			inline void AddToJourney(const JourneySegment& js) { journeySegments.push_back(js); }
		};
	}

	namespace Algorithms {
		class Router {
		private:
			const Timetables::Structures::Station& source;
			const Timetables::Structures::Station& target;
			const Timetables::Structures::DateTime& earliestDeparture;
			const std::size_t maxTransfers;
			const std::size_t count;

			std::vector<Timetables::Structures::Journey> fastestJourneys;

			std::vector<std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::DateTime>> labels;
			std::vector<std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::Journey>> journeys;
			std::unordered_map<const Timetables::Structures::Stop*, Timetables::Structures::DateTime> tempLabels;
			std::unordered_set<const Timetables::Structures::Stop*> markedStops;
			std::unordered_map<const Timetables::Structures::Route*, const Timetables::Structures::Stop*> activeRoutes;

			void AccumulateRoutes();
			void TraverseEachRoute();
			void LookAtFootpaths();
			void ObtainJourney(const Timetables::Structures::DateTime& departure);

			std::pair<const Timetables::Structures::Trip*, Timetables::Structures::DateTime>
				FindEarliestTrip(const Timetables::Structures::Route& route, const Timetables::Structures::DateTime& arrival, const Timetables::Structures::Stop& stop);
		public:
			Router(const Timetables::Structures::DataFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::DateTime& earliestDeparture, const std::size_t count, const std::size_t transfers) :
				maxTransfers(transfers), count(count), earliestDeparture(earliestDeparture), source(feed.Stations().Find(s)), target(feed.Stations().Find(t)) {}

			void ObtainJourneys();

			const std::vector<Timetables::Structures::Journey>& ShowJourneys() const { return fastestJourneys; }
		};
	}
}

#endif // !ROUTER_HPP