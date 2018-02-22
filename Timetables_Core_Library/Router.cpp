#include "Router.hpp"
#include "Routes.hpp"
#include <vector>
#include <unordered_map>
#include "Utilities.hpp"
#include <queue>
#include <memory>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

Timetables::Structures::Journey Timetables::Algorithms::FindRoute(const Timetables::Structures::GtfsFeed & feed, const Timetables::Structures::Station & source, const Timetables::Structures::Station & target, const Timetables::Structures::Datetime & datetime, const std::size_t transfers) {
	
	// The i-th position of the vector says in which time we are capable to reach given stop in the map from the source station.
	// Default state is infinity (represented by not found state).

	vector<unordered_map<StopPtrObserver, Datetime>> labels;

	vector<unordered_map<StopPtrObserver, Journey>> journeys;

	unordered_map<StopPtrObserver, Datetime> tempLabels;

	queue<StopPtrObserver> markedStops;

	// Initialization of the algorithm.

	labels.push_back(unordered_map<StopPtrObserver, Datetime>());

	journeys.push_back(unordered_map<StopPtrObserver, Journey>());

	// Using 0 trips we are able to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source.GetChildStops()) {
		labels[0].insert(make_pair(stop, datetime));
		markedStops.push(stop);
	}

	using RoutePtrObserver = const Route*;

	for (size_t k = 1; k <= transfers; k++) {

		labels.push_back(unordered_map<StopPtrObserver, Datetime>());

		journeys.push_back(unordered_map<StopPtrObserver, Journey>());

		// Accumulate routes serving marked stops from previous round.

		unordered_map<RoutePtrObserver, StopPtrObserver> Q; // 7th row of pseudocode.

		while (markedStops.size() > 0) { // 8th row of pseudocode.

			const Stop& stop = *markedStops.front();

			for (auto&& route : stop.GetRoutes()) { // 9th row of pseudocode.

				auto it = Q.find(route);

				if (it != Q.cend()) { // 10th row of pseudocode.

					if (route->StopComesBefore(stop, *it->second)) { // 11th row of pseudocode.

						Q.erase(route);

						Q.insert(make_pair(route, &stop)); // 11th row of pseudocode.

					}

				}

				else

					Q.insert(make_pair(route, &stop)); // 13th row of pseudocode.

			}

			markedStops.pop(); // 14th row of pseudocode.

		}

		// Traverse each route.

		for (auto&& route : Q) { // 15th row of pseudocode.

			TripPtrObserver trip = nullptr; // 16th row of pseudocode.

			auto beginning = find(route.first->GetStops().cbegin(), route.first->GetStops().cend(), route.second); // 17th row of pseudocode.

			for (auto it = beginning; it != route.first->GetStops().cend(); ++it) { // 17th row of pseudocode.

																					// Can the label be improved in this round? Includes local and target pruning.

				const Stop& s = **it;

				if (trip != nullptr) { // 18th row of code.

									   // We need arrival for given stop using trip "trip".

					vector<unique_ptr<StopTime>>::const_iterator st;

					for (st = trip->GetStopTimes().cbegin(); st != trip->GetStopTimes().cend(); ++st)
						if (&st->get()->GetStop() == &s)
							break;

					// The best time we can get to stop "s" (meaning **it).

					auto tempTimeStop = tempLabels.find(&s);

					const Datetime& pi = tempTimeStop == tempLabels.cend() ? Datetime(datetime.GetDate()) : tempTimeStop->second;

					// Now we have to look for the best arrival time to target station. We need to iterate over all the stops under the station.

					vector<Datetime> targetStops;

					for (auto&& childStop : target.GetChildStops()) {

						auto it = tempLabels.find(childStop);

						if (it != tempLabels.cend())

							targetStops.push_back(it->second);

					}

					sort(targetStops.begin(), targetStops.end()); // TO-DO: Change sort n log n to linear approach of finding a minimum.

					auto pt = targetStops.size() == 0 ? Datetime(datetime.GetDate()) : targetStops.at(0);

					Datetime min = pi < pt ? pi : pt;

					if (st._Ptr != nullptr && (min.Infinity() || st->get()->GetArrival() < min.GetTime())) { // 18th row of code.

						labels.at(k).erase(&s);

						labels.at(k).insert(make_pair(&s, Datetime(min.GetDate(), st->get()->GetArrival()))); // 19th row of pseudocode.

						auto start = find_if(trip->GetStopTimes().cbegin(), trip->GetStopTimes().cend(),
							[=](const unique_ptr<StopTime>& val) { return &val->GetStop() == *beginning; });

						if (k == 1) {

							TripSegment segment(**start, min.GetDate(), min.GetDate()); // TO-DO: Dates fix.

							for (auto next = start + 1; next != st + 1; ++next)

								segment.AddToTripSegment(**next, min.GetDate(), min.GetDate());

							journeys.at(k).insert(make_pair(&s, Journey(move(segment))));

						}


						else {

							Journey currentJourney(journeys.at(k - 1).find(route.second)->second);

							TripSegment segment(**start, min.GetDate(), min.GetDate()); // TO-DO: Dates fix.

							for (auto next = start + 1; next != st + 1; ++next)

								segment.AddToTripSegment(**next, min.GetDate(), min.GetDate());

							currentJourney.AddToJourney(move(segment));

							journeys.at(k).erase(&s);

							journeys.at(k).insert(make_pair(&s, move(currentJourney)));

						}

						tempLabels.erase(&s);

						tempLabels.insert(make_pair(&s, Datetime(min.GetDate(), st->get()->GetArrival()))); // 20th row of pseudocode.

						markedStops.push(&s); // 21st row of pseudocode.

					}

					// Can we catch an earlier trip at stop "s"? 

					// If trip is set, we can go for departure time from "s" using that trip. Otherwise we have to set it.

					auto arrivalTime = labels.at(k - 1).find(&s);

					if (arrivalTime != labels.at(k - 1).cend() && arrivalTime->second.GetTime() <= st->get()->GetDeparture()) { // 22nd row of pseudocode.

						trip = GetEarliestTrip(arrivalTime->second, *route.first, s); // 23rd row of pseudocode.

					}

				}

				if (trip == nullptr) { // Infinity.

					auto arrivalTime = labels.at(k - 1).find(&s);

					if (arrivalTime != labels.at(k - 1).cend()) { // 22nd row of pseudocode.

						trip = GetEarliestTrip(arrivalTime->second, *route.first, s); // 23rd row of pseudocode.

					}

				}

			}

		}

		queue<StopPtrObserver> setAsMarked; // I have to do that because of cycle and queue properties.

											// Look at foot-paths.

		for (size_t i = 0; i < markedStops.size(); i++) { // 24th row of pseudocode.

			const Stop& A = *markedStops.front(); markedStops.pop(); markedStops.push(&A);

			for (auto&& footpath : A.GetFootpaths()) { // 25th row of pseudocode.

				const Stop& B = *footpath.second;

				auto arrivalTimeA = labels.at(k).find(&A);

				auto arrivalTimeB = labels.at(k).find(&B);

				Datetime first = arrivalTimeB == labels.at(k).cend() ? Datetime(datetime.GetDate()) : arrivalTimeB->second;

				Datetime second = arrivalTimeA == labels.at(k).cend() ? Datetime(datetime.GetDate()) : arrivalTimeA->second + footpath.first;

				Datetime min = first < second ? first : second;

				labels.at(k).erase(&B);

				labels.at(k).insert(make_pair(&B, min)); // 26th row of pseudocode.

				journeys.at(k).insert(make_pair(&B, journeys.at(k).find(&A)->second)); // The same journey, added just some footpath.

				setAsMarked.push(&B); // 27th row of pseudocode.

			}

		}

		while (setAsMarked.size() > 0) {

			markedStops.push(setAsMarked.front()); setAsMarked.pop();

		}

		// Stopping criterion.

		if (markedStops.size() == 0) // 28th row of pseudocode.

			break; // 29th row of pseudocode.

	}
	
	const Journey* bestJourney = nullptr;

	for (auto it = journeys.cbegin() + 1; it != journeys.end(); ++it) {

		for (auto&& journey : *it) {

			if (&journey.first->GetParentStation() == &target) {

				if (bestJourney == nullptr || journey.second.GetArrival() < bestJourney->GetArrival())

					bestJourney = &journey.second;

			}

		}

	}

	if (bestJourney == nullptr)

		throw JourneyNotFoundException(source.GetName(), target.GetName());

	return *bestJourney;

}

std::vector<Timetables::Structures::Journey> Timetables::Algorithms::FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& datetime, const std::size_t count, const std::size_t transfers) {

	auto it = feed.GetStations().find(s);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(s);

	const Station& source = it->second;

	it = feed.GetStations().find(t);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(t);

	const Station& target = it->second;

	vector<Journey> journeys;

	journeys.push_back(FindRoute(feed, source, target, datetime, transfers));

	for (size_t i = 1; i < count; i++)
		journeys.push_back(FindRoute(feed, source, target, journeys.at(i - 1).GetDeparture() + 1, transfers));

	return move(journeys);
}

TripPtrObserver Timetables::Algorithms::GetEarliestTrip(const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Route& route, const Timetables::Structures::Stop& stop) {

	// Look for earliest trip that we can catch using route "route" at stop "s".

	// We have to recall arrival time to "s" using k - 1 trips (variable tempTime).

	if (arrival.Infinity()) return nullptr;

	// Now we will find index of required stop in trip stops sequence to have a constant access using pointer arithmetics.

	size_t i = 0;

	for (auto it = route.GetTrips().cbegin()->second->GetStopTimes().cbegin(); it != route.GetTrips().cbegin()->second->GetStopTimes().cend(); ++it, ++i)
		if (&it->get()->GetStop() == &stop)
			break;

	// Now let's find the closest trip that we can use. That means the earliest departure after the arrival time.

	// TO-DO: Some heuristic may decrease time consumption. E.g. reverse iterator from upperbound of departure from the first stop in the trip.
	
	for (auto it = route.GetTrips().cbegin(); it != route.GetTrips().cend(); ++it) {

		if (!it->second->IsOperatingInDatetime(arrival)) continue; // The trip is not operating in this date.

		auto departureFromRequiredStopUsingThisTrip = (it->second->GetStopTimes().cbegin() + i)->get()->GetDeparture();
		
		if (departureFromRequiredStopUsingThisTrip > arrival.GetTime()) // Then return this trip.

			return it->second;

	}

	return nullptr;

}