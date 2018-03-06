#include "Router.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::Router::AccumulateRoutes() {

	// Accumulate routes serving marked stops from previous round.

	activeRoutes.clear(); // 7th row of pseudocode.

	for (auto&& stop : markedStops) { // 8th row of pseudocode.

		for (auto&& route : stop->ThroughgoingRoutes()) { // 9th row of pseudocode.

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
	
	for (auto&& item : activeRoutes) { // 15th row of pseudocode.

		const Trip* currentTrip = nullptr; // 16th row of pseudocode.
		DateTime currentDateForTrip(0);

		const Stop& startingStop = *item.second;
		const Route& route = *item.first;

		// Find the current stop in route.

		vector<const Stop*>::const_iterator nextStop;

		for (nextStop = route.Stops().cbegin(); nextStop != route.Stops().cend(); ++nextStop)
			if (*nextStop == &startingStop)
				break;

		// Traverse the route from current stop.

		const Stop* boardingStop = nullptr;

		for (; nextStop != route.Stops().cend(); ++nextStop) { // 17th row of pseudocode.

			// Can the label be improved in this round? Includes local and target pruning.

			const Stop& currentStop = **nextStop;

			if (currentTrip != nullptr) { // 18th row of pseudocode.
				
				const StopTime& st = *(currentTrip->StopTimes().cbegin() + (nextStop - route.Stops().cbegin()));

				const DateTime& newArrival = currentDateForTrip.AddSeconds(st.ArrivalSinceTripBeginning()); // 18th row of pseudocode.
					
				auto currentStopBestArrival = tempLabels.find(&currentStop);
				
				auto targetStopBestArrival = tempLabels.cend();

				for (auto&& stop : target.ChildStops())
					if (tempLabels.find(stop) != tempLabels.cend() && (targetStopBestArrival == tempLabels.cend() || tempLabels.find(stop)->second < targetStopBestArrival->second))
						targetStopBestArrival = tempLabels.find(stop); // 18th row of pseudocode.


				if ((currentStopBestArrival == tempLabels.cend() && targetStopBestArrival == tempLabels.cend()) || // Then the minimum is an infinity. Process the if block.
					(currentStopBestArrival == tempLabels.cend() && targetStopBestArrival != tempLabels.cend() && newArrival < targetStopBestArrival->second) ||
					(currentStopBestArrival != tempLabels.cend() && targetStopBestArrival == tempLabels.cend() && newArrival < currentStopBestArrival->second) || 
					(currentStopBestArrival != tempLabels.cend() && targetStopBestArrival != tempLabels.cend() && newArrival < (targetStopBestArrival->second < currentStopBestArrival->second ? targetStopBestArrival->second : currentStopBestArrival->second))
					) { // 18th row of pseudocode.

					(labels.end() - 1)->erase(&currentStop);
					tempLabels.erase(&currentStop);

					(labels.end() - 1)->insert(make_pair(&currentStop, newArrival)); // 19th row of pseudocode.
					
					if (journeys.size() == 2) {

						JourneySegment segment(*currentTrip, newArrival, *boardingStop, currentStop);

						(journeys.end() - 1)->erase(&currentStop);

						(journeys.end() - 1)->insert(make_pair(&currentStop, Journey(move(segment))));
					}

					else {

						Journey currentJourney((journeys.end() - 2)->find(boardingStop)->second);

						JourneySegment segment(*currentTrip, newArrival, *boardingStop, currentStop);

						currentJourney.AddToJourney(move(segment));

						(journeys.end() - 1)->erase(&currentStop);

						(journeys.end() - 1)->insert(make_pair(&currentStop, move(currentJourney)));

					}

					tempLabels.insert(make_pair(&currentStop, newArrival)); // 20th row of pseudocode.

					markedStops.insert(&currentStop); // 21st row of pseudocode.
				}


				// Can we catch an earlier trip?

				auto previousArrival = (labels.end() - 2)->find(&currentStop);

				if (previousArrival != (labels.end() - 2)->cend() && previousArrival->second <= currentDateForTrip.AddSeconds(st.DepartureSinceTripBeginning())) { // 22nd row of pseudocode.

					auto res = FindEarliestTrip(route, previousArrival->second, currentStop);
					currentTrip = res.first; // 23rd row of pseudocode.
					currentDateForTrip = move(res.second);
					boardingStop = &currentStop; // We have just boarded the trip.
				
				}
			}

			// Can we catch an earlier trip?

			if (currentTrip == nullptr) { // 22nd row of pseudocode.

				auto previousArrival = (labels.end() - 2)->find(&currentStop);

				if (previousArrival != (labels.end() - 2)->cend()) {
					auto res = FindEarliestTrip(route, previousArrival->second, currentStop);
					currentTrip = res.first; // 23rd row of pseudocode.
					currentDateForTrip = move(res.second);
					boardingStop = &currentStop; // We have just boarded the trip.
				}
			}

		}

	}

}

void Timetables::Algorithms::Router::LookAtFootpaths() {

	vector<const Stop*> tempMarked;

	for (auto&& stopA : markedStops) { // 24th row of pseudocode.

		for (auto&& footpath : stopA->Footpaths()) { // 25th row of pseudocode.

			size_t duration = footpath.first;

			const Stop* stopB = footpath.second;

			auto arrivalTimeA = (labels.cend() - 1)->find(stopA);

			auto arrivalTimeB = (labels.cend() - 1)->find(stopB);

			DateTime min(0);

			if (arrivalTimeA == (labels.cend() - 1)->cend() && (arrivalTimeB != (labels.cend() - 1)->cend()))

				min = arrivalTimeB->second;

			else if (arrivalTimeA != (labels.cend() - 1)->cend() && (arrivalTimeB == (labels.cend() - 1)->cend()))

				min = arrivalTimeA->second.AddSeconds(duration);

			else if (arrivalTimeA != (labels.cend() - 1)->cend() && (arrivalTimeB != (labels.cend() - 1)->cend()))

				min = arrivalTimeB->second < arrivalTimeA->second.AddSeconds(duration) ? arrivalTimeB->second : arrivalTimeA->second.AddSeconds(duration);

			else

				throw runtime_error("Undefined state.");
				
			if ((arrivalTimeB == (labels.cend() - 1)->cend()) && min != arrivalTimeB->second) {

				(labels.end() - 1)->erase(stopB);

				(labels.end() - 1)->insert(make_pair(stopB, min)); // 26th row of pseudocode.
				
				Journey newJourney((journeys.cend() - 1)->find(stopA)->second); // The same journey, added just some footpath -> arrival time increased.

				(journeys.end() - 1)->erase(stopB);

				(journeys.end() - 1)->insert(make_pair(stopB, newJourney)); 

			}


			tempMarked.push_back(stopB); // Temp because it may invalidate iterators.

		}

	}

	for (auto&& stop : tempMarked)
		markedStops.insert(stop); // 27th row of pseudocode.

}

std::pair<const Timetables::Structures::Trip*, Timetables::Structures::DateTime> Timetables::Algorithms::Router::FindEarliestTrip(const Timetables::Structures::Route& route, const Timetables::Structures::DateTime& arrival, const Timetables::Structures::Stop& stop) {

	// At first we have to precompute index for given stop in route to have a constant access to that via trips. TO-DO: Keep indices in memory.

	size_t index = 0;
	
	for (index = 0; index < route.Stops().size(); index++)
		if (*(route.Stops().cbegin() + index) == &stop)
			break;

	DateTime newArrival(arrival);
	size_t days = 0;

	for (auto it = route.Trips().cbegin(); days < 7; ++it) {
		
		if (it == route.Trips().cend()) {
			it = route.Trips().cbegin();
			days++;
			newArrival = newArrival.AddDays(1);
		}

		const StopTime& st = *((**it).StopTimes().cbegin() + index);

		DateTime startingDateForTrip = st.StartingDateForTrip(newArrival);

		if (startingDateForTrip.AddSeconds(st.DepartureSinceTripBeginning()) > arrival && st.IsOperatingInDate(newArrival))

			return make_pair(*it, startingDateForTrip);

	}

	return make_pair(nullptr, DateTime(0));
}

void Timetables::Algorithms::Router::ObtainJourneys() {

	if (fastestJourneys.size() == 0)
		ObtainJourney(earliestDeparture);

	// We tried to search a journey but no journey found.

	if (fastestJourneys.size() == 0)
		throw JourneyNotFoundException(source.Name(), target.Name());

	for (int i = 1; i < count; i++)
		ObtainJourney((fastestJourneys.cend() - 1)->ArrivalTime().AddSeconds(1));

}

void Timetables::Algorithms::Router::ObtainJourney(const Timetables::Structures::DateTime& departure) {

	labels.clear();
	tempLabels.clear();
	markedStops.clear();
	activeRoutes.clear();
	journeys.clear();

	labels.push_back(unordered_map<const Stop*, DateTime>());

	journeys.push_back(unordered_map<const Stop*, Journey>());

	// Using 0 trips we are able to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source.ChildStops()) {
		labels.at(0).insert(make_pair(stop, departure)); // 4th row of pseudocode.
		markedStops.insert(stop); // 5th row of pseudocode.
	}

	for (size_t k = 1; markedStops.size() > 0 && k <= maxTransfers; k++) { // 6th && 28th && 29th row of pseudocode.

		labels.push_back(unordered_map<const Stop*, DateTime>());

		journeys.push_back(unordered_map<const Stop*, Journey>());

		AccumulateRoutes();
		TraverseEachRoute();
		LookAtFootpaths();

	}

	vector<Journey> tempJourneys;

	for (size_t i = 0; i < maxTransfers && i < journeys.size(); i++)
		for (auto&& stop : target.ChildStops()) {
			
			auto res = journeys[i].find(stop);

			if (res != journeys[i].cend())
				tempJourneys.push_back(res->second);

		}

	const Journey* fastestJourney = nullptr;

	for (auto&& journey : tempJourneys)
		if (fastestJourney == nullptr || fastestJourney->ArrivalTime() > journey.ArrivalTime())
			fastestJourney = &journey;

	if (fastestJourney != nullptr)
		fastestJourneys.push_back(move(*fastestJourney));
}

Timetables::Structures::JourneySegment::JourneySegment(const Timetables::Structures::Trip& trip, const DateTime& arrival, const Stop& source, const Stop& target) : trip(trip), arrival(arrival) {

	for (std::vector<StopTime>::const_iterator it = trip.StopTimes().cbegin(); it != trip.StopTimes().cend(); ++it) {

		if (&it->Stop() == &source) sourceStop = it;

		if (&it->Stop() == &target) targetStop = it;

	}

}

const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> Timetables::Structures::JourneySegment::IntermediateStops() const {
	vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> stops;
	size_t base = sourceStop->Departure();
	for (auto it = sourceStop; it != targetStop + 1; ++it)
		stops.push_back(make_pair(it->Arrival() - base, &it->Stop()));
	return move(stops);
}
