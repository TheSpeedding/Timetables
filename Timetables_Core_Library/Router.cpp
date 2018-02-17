#include "Router.hpp"
#include <vector>
#include <unordered_map>
#include "Utilities.hpp"
#include <queue>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::FindRoutes(const Timetables::Structures::GtfsFeed & feed, const std::wstring & A, const std::wstring & B, const Timetables::Structures::Datetime & datetime, const size_t count) {

	auto it = feed.GetStations().find(A);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(A);

	const Station& source = it->second;

	it = feed.GetStations().find(B);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(B);

	const Station& target = it->second;

	// The i-th position of the vector says in which time we are capable to reach given stop in the map from the source station.
	// Default state is infinity (represented by not found state)

	vector<unordered_map<StopPtrObserver, Datetime>> continuousDepartureTime;

	queue<StopPtrObserver> markedStops;

	// Initialization of the algorithm.

	continuousDepartureTime.push_back(unordered_map<StopPtrObserver, Datetime>());

	// Using 0 trips we are capable to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source.GetChildStops()) {
		continuousDepartureTime[0].insert(make_pair(stop, datetime));
		markedStops.push(stop);
	}
}
