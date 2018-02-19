#include "Router.hpp"
#include "Routes.hpp"
#include <vector>
#include <unordered_map>
#include "Utilities.hpp"
#include <queue>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::FindRoutes(const Timetables::Structures::GtfsFeed& feed, const std::wstring& s, const std::wstring& t, const Timetables::Structures::Datetime& datetime, const std::size_t count, const std::size_t transfers) {

	auto it = feed.GetStations().find(s);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(s);

	const Station& source = it->second;

	it = feed.GetStations().find(t);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(t);

	const Station& target = it->second;

	// The i-th position of the vector says in which time we are capable to reach given stop in the map from the source station.
	// Default state is infinity (represented by not found state).

	vector<unordered_map<StopPtrObserver, Datetime>> arrivalTimes;

	unordered_map<StopPtrObserver, Datetime> tempTimes;	

	queue<StopPtrObserver> markedStops;

	// Initialization of the algorithm.

	arrivalTimes.push_back(unordered_map<StopPtrObserver, Datetime>());

	// Using 0 trips we are capable to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source.GetChildStops()) {
		arrivalTimes[0].insert(make_pair(stop, datetime));
		markedStops.push(stop);
	}

	using RoutePtrObserver = const Route*;

	for (size_t k = 1; k <= transfers; k++) {

		arrivalTimes.push_back(unordered_map<StopPtrObserver, Datetime>());

		// Accumulate routes serving marked stops from previous round.

		unordered_map<RoutePtrObserver, StopPtrObserver> Q; // 7th row of pseudocode.

		while (markedStops.size() > 0) { // 8th row of pseudocode.

			const Stop& stop = *markedStops.front();

			for (auto&& route : stop.GetRoutes()) { // 9th row of pseudocode.

				auto it = Q.find(route);

				if (it != Q.cend()) { // 10th row of pseudocode.

					const Stop& p = *it->second;

					if (route->StopComesBefore(stop, *it->second)) { // 11th row of pseudocode.

						Q.erase(route);

						Q.insert(make_pair(route, &p)); // 11th row of pseudocode.

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

					StopTimePtrObserver st = nullptr;

					for (auto&& stopTime : trip->GetStopTimes()) {

						if (&stopTime->GetStop() == &s) {

							st = stopTime.get();

							break;

						}

					}

					// The best time we can get to stop "s" (meaning **it).

					auto tempTimeStop = tempTimes.find(&s);
					
					const Datetime& pi = tempTimeStop == tempTimes.cend() ? Datetime() : tempTimeStop->second;

					// Now we have to look for the best arrival time to target station. We need to iterate over all the stops under the station.

					vector<Datetime> targetStops;

					for (auto&& childStop : target.GetChildStops()) {

						auto it = tempTimes.find(childStop);

						if (it != tempTimes.cend())

							targetStops.push_back(it->second);

					}

					sort(targetStops.begin(), targetStops.end());

					auto pt = targetStops.size() == 0 ? Datetime() : targetStops.at(0);
					
					Datetime min = pi < pt ? pi : pt;

					if (st != nullptr && (min.Infinity() || st->GetArrival() < min.GetTime())) { // 18th row of code.

						arrivalTimes.at(k).insert(make_pair(&s, Datetime(min.GetDate(), st->GetArrival()))); // 19th row of pseudocode.
						
						tempTimes.insert(make_pair(&s, Datetime(min.GetDate(), st->GetArrival()))); // 20th row of pseudocode.

						// This is the point where we finish our journey if we have reached the required station.

						if (target.ContainsStop(s)) {
							throw 1;
						}

						markedStops.push(&s); // 21st row of pseudocode.

					}

					// Can we catch an earlier trip at stop "s"? 

					// If trip is set, we can go for departure time from "s" using that trip. Otherwise we have to set it.
					
					auto arrivalTime = arrivalTimes.at(k - 1).find(&s);

					if (arrivalTime != arrivalTimes.at(k - 1).cend() && arrivalTime->second.GetTime() <= st->GetDeparture()) { // 22nd row of pseudocode.

						trip = GetEarliestTrip(arrivalTime->second, *route.first, s); // 23rd row of pseudocode.

					}

				}

				if (trip == nullptr) { // Infinity.

					auto arrivalTime = arrivalTimes.at(k - 1).find(&s);
					
					if (arrivalTime != arrivalTimes.at(k - 1).cend()) { // 22nd row of pseudocode.

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

				auto arrivalTimeA = arrivalTimes.at(k).find(&A);

				auto arrivalTimeB = arrivalTimes.at(k).find(&B);

				auto cend = arrivalTimes.at(k).cend();

				Datetime min = (arrivalTimeB == cend ? Datetime() : arrivalTimeB->second) < (arrivalTimeA == cend ? Datetime() : arrivalTimeA->second + footpath.first) ?
					(arrivalTimeB == cend ? Datetime() : arrivalTimeB->second) : (arrivalTimeA == cend ? Datetime() : arrivalTimeA->second + footpath.first);

				arrivalTimes.at(k).erase(&B);

				arrivalTimes.at(k).insert(make_pair(&B, min)); // 26th row of pseudocode.

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

}

TripPtrObserver Timetables::Algorithms::GetEarliestTrip(const Timetables::Structures::Datetime& arrival, const Timetables::Structures::Route& route, const Timetables::Structures::Stop& stop) {

	// Look for earliest trip that we can catch using route "route" at stop "s".

	// We have to recall arrival time to "s" using k - 1 trips (variable tempTime).

	if (arrival.Infinity()) return nullptr;

	// Now we will find index of required stop in trip stops sequence to have a constant access via pointer arithmetics.

	size_t i = 0;

	for (auto it = route.GetTrips().cbegin()->second->GetStopTimes().cbegin(); it != route.GetTrips().cbegin()->second->GetStopTimes().cend(); ++it, ++i)
		if (&it->get()->GetStop() == &stop)
			break;

	// Now let's find the closest trip that we can use. That means the earliest departure after the arrival time.

	// TO-DO: Some heuristic may decrease time consumption. E.g. reverse iterator from upperbound of departure from the first stop in the trip.
	
	for (auto it = route.GetTrips().cbegin(); it != route.GetTrips().cend(); ++it) {

		// if (!it->second->GetService().IsOperatingInDate(arrival.GetDate())) continue; // The trip is not operating in this date.

		auto departureFromRequiredStopUsingThisTrip = (it->second->GetStopTimes().cbegin() + i)->get()->GetDeparture();
		
		if (departureFromRequiredStopUsingThisTrip > arrival.GetTime()) // Then return this trip.

			return it->second;

	}

	return nullptr;

}
