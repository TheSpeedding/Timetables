#include "Router.hpp"
#include <vector>
#include "Utilities.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::Router::AccumulateRoutes() {

	// Accumulate routes serving marked stops from previous round.

	activeRoutes.clear(); // 7th row of pseudocode.

	for (auto&& stop : markedStops) { // 8th row of pseudocode.

		for (auto&& route : stop->GetRoutes()) { // 9th row of pseudocode.

			auto someStop = activeRoutes.find(route);

			if (someStop != activeRoutes.cend()) { // 10th row of pseudocode.

				if (route->StopComesBefore(*stop, *someStop->second)) { // 11th row of pseudocode.

					activeRoutes.erase(route);

					activeRoutes.insert(make_pair(route, stop)); // 11th row of pseudocode.

				}

			}

			else // 12th row of pseudocode.

				activeRoutes.insert(make_pair(route, stop)); // 13th row of pseudocode.

		}

	}

	markedStops.clear(); // 14th row of pseudocode.

}

void Timetables::Algorithms::Router::TraverseEachRoute() {

	// TEMPORARY, REMOVE AND FIX DATETIMES.

	Date temp = labels.cbegin()->cbegin()->second.GetDate();

	for (auto&& item : activeRoutes) { // 15th row of pseudocode.

		TripPtrObserver currentTrip = nullptr; // 16th row of pseudocode.
		const Stop& startingStop = *item.second;
		const Route& route = *item.first;

		// Find the current stop in route.

		vector<StopPtrObserver>::const_iterator nextStop;

		for (nextStop = route.GetStops().cbegin(); nextStop != route.GetStops().cend(); ++nextStop)
			if (*nextStop == &startingStop)
				break;

		// Traverse the route from current stop.

		for (; nextStop != route.GetStops().cend(); ++nextStop) { // 17th row of pseudocode.

			// Can the label be improved in this round? Includes local and target pruning.

			const Stop& currentStop = **nextStop;

			if (currentTrip != nullptr) { // 18th row of pseudocode.

				const StopTime& st = **(currentTrip->GetStopTimes().cbegin() + (nextStop - route.GetStops().cbegin()));

				const Datetime& newArrival = Datetime(temp, st.GetArrival()); // 18th row of pseudocode.

				Datetime earliestCurrentStopArrival = tempLabels.find(&currentStop) == tempLabels.cend() ? Datetime::GetInvalid() : tempLabels.find(&currentStop)->second; // 18th row of pseudocode.

				Datetime earliestTargetStopArrival = Datetime::GetInvalid();

				for (auto&& stop : target->GetChildStops())
					if (tempLabels.find(stop) != tempLabels.cend() && tempLabels.find(stop)->second < earliestTargetStopArrival)
						earliestTargetStopArrival = tempLabels.find(stop)->second; // 18th row of pseudocode.

				if (newArrival < (earliestCurrentStopArrival < earliestTargetStopArrival ? earliestCurrentStopArrival : earliestTargetStopArrival)) { // 18th row of pseudocode.

					(labels.end() - 1)->erase(&currentStop);
					tempLabels.erase(&currentStop);

					(labels.end() - 1)->insert(make_pair(&currentStop, newArrival)); // 19th row of pseudocode.
										
					tempLabels.insert(make_pair(&currentStop, newArrival)); // 20th row of pseudocode.

					markedStops.insert(&currentStop); // 21st row of pseudocode.
				}


				// Can we catch an earlier trip?

				auto previousArrival = (labels.end() - 2)->find(&currentStop);

				if (previousArrival != (labels.end() - 2)->cend() && previousArrival->second <= Datetime(temp, st.GetDeparture())) // 22nd row of pseudocode.

					currentTrip = FindEarliestTrip(route, previousArrival->second, currentStop); // 23rd row of pseudocode.
			}

			// Can we catch an earlier trip?

			if (currentTrip == nullptr) { // 22nd row of pseudocode.

				auto previousArrival = (labels.end() - 2)->find(&currentStop);

				if (previousArrival != (labels.end() - 2)->cend())
				
					currentTrip = FindEarliestTrip(route, previousArrival->second, currentStop); // 23rd row of pseudocode.

			}

		}

	}

}

void Timetables::Algorithms::Router::LookAtFootpaths() {

	for (auto&& stopA : markedStops) { // 24th row of pseudocode.

		for (auto&& footpath : stopA->GetFootpaths()) { // 25th row of pseudocode.

			const int duration = footpath.first;

			StopPtrObserver stopB = footpath.second;

			auto arrivalTimeA = (labels.cend() - 1)->find(stopA);

			auto arrivalTimeB = (labels.cend() - 1)->find(stopB);

			if (arrivalTimeA == (labels.cend() - 1)->cend() && arrivalTimeB == (labels.cend() - 1)->cend())

				continue;

			else {

				Datetime min = arrivalTimeB->second < arrivalTimeA->second.AddSeconds(duration) ? arrivalTimeB->second : arrivalTimeA->second.AddSeconds(duration);

				(labels.end() - 1)->erase(stopB);

				(labels.end() - 1)->insert(make_pair(stopB, min)); // 26th row of pseudocode.

				(journeys.end() - 1)->insert(make_pair(stopB, (journeys.cend() - 1)->find(stopA)->second)); // The same journey, added just some footpath.

			}

			markedStops.insert(stopB); // 27th row of pseudocode.

		}

	}

}

TripPtrObserver Timetables::Algorithms::Router::FindEarliestTrip(const Timetables::Structures::Route& route, const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Stop& stop) {

	// At first we have to precompute index for given stop in route to have a constant access to that via trips. TO-DO: Keep indices in memory.

	size_t index = 0;

	for (auto it = route.GetStops().cbegin(); it != route.GetStops().cend(); ++it, index++)
		if (*it == &stop)
			break;

	// TO-DO: Binary search.

	for (auto it = route.GetTrips().cbegin(); it != route.GetTrips().cend(); ++it) {

		auto st = (it->second->GetStopTimes().cbegin() + index)->get();

		if (it->second->IsOperatingInDatetime(arrival) && (st->GetDeparture() > arrival.GetTime()) || st->GetDeparture().ExceedsDay())

			return it->second;

	}

	return nullptr;
}

Timetables::Algorithms::Router::Router(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& earliestDeparture, const std::size_t count, const std::size_t transfers) : transfers(transfers), count(count), earliestDeparture(earliestDeparture) {
	auto it = feed.GetStations().find(s);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(s);

	source = &it->second;

	it = feed.GetStations().find(t);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(t);

	target = &it->second;
}

void Timetables::Algorithms::Router::ObtainJourney() {
	
	labels.push_back(unordered_map<StopPtrObserver, Datetime>());

	journeys.push_back(unordered_map<StopPtrObserver, Journey>());

	// Using 0 trips we are able to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source->GetChildStops()) {
		labels.at(0).insert(make_pair(stop, earliestDeparture)); // 4th row of pseudocode.
		markedStops.insert(stop); // 5th row of pseudocode.
	}

	for (size_t k = 1; markedStops.size() > 0 && k <= transfers; k++) { // 6th && 28th && 29th row of pseudocode.

		labels.push_back(unordered_map<StopPtrObserver, Datetime>());

		journeys.push_back(unordered_map<StopPtrObserver, Journey>());

		AccumulateRoutes();
		TraverseEachRoute();
		LookAtFootpaths();

	}
}
