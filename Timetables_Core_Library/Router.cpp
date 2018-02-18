#include "Router.hpp"
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

	queue<StopPtrObserver> markedStops;

	// Initialization of the algorithm.

	arrivalTimes.push_back(unordered_map<StopPtrObserver, Datetime>());

	// Using 0 trips we are capable to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source.GetChildStops()) {
		arrivalTimes[0].insert(make_pair(stop, datetime));
		markedStops.push(stop);
	}

	for (int k = 1; k <= transfers; k++) {

		// First stage. 
		// Upper bound for earliest arrival time using k trips.
		arrivalTimes.push_back(*(arrivalTimes.cend() - 1));
		
		// Second stage.
		// We will process each route exactly once.

	}
}