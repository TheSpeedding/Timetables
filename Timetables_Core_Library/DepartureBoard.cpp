#include "Stops.hpp"
#include "Trips.hpp"
#include "Exceptions.hpp"
#include "GtfsFeed.hpp"
#include <vector>
#include <string>
#include <map>
#include "DepartureBoard.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Departure::Departure(const Timetables::Structures::StopTime& stopTime) {
	const vector<unique_ptr<StopTime>>& stopTimes = stopTime.GetTrip().GetStopTimes();

	for (vector<unique_ptr<StopTime>>::const_iterator it = stopTimes.cbegin(); it != stopTimes.cend(); ++it)
		if (it->get() == &stopTime)
			tripBegin = it;

	tripEnd = stopTimes.cend();
}

const std::vector<Departure> Departure::GetFollowingStops() const {
	vector<Departure> stops;
	for (auto it = tripBegin; it != tripEnd; ++it)
		stops.push_back(Departure(*it->get()));
	return move(stops);
}

Timetables::Algorithms::DepartureBoard::DepartureBoard(const Timetables::Structures::GtfsFeed& feed, const std::wstring& stationName, const Timetables::Structures::Datetime& earliestDeparture, const size_t count) : count(count), earliestDeparture(earliestDeparture) {

	auto it = feed.GetStations().find(stationName);
	if (it == feed.GetStations().cend()) throw StopNotFoundException(stationName);

	// At this point we have the station for which we want to list all the departures.
	station = &it->second;

}

void Timetables::Algorithms::DepartureBoard::ObtainDepartureBoard() {

	Time departureTime = earliestDeparture.GetTime(); // Lower bound for departures.

	// In this structure we will store "count" departures from each child stop (meaning count * childStops.size()).
	// Then we will return first "count" departures sorted by time.

	multimap<Time, StopTimePtrObserver> departures;

	for (auto&& child_stop : station->GetChildStops()) {
		const multimap<Time, StopTimePtrObserver>& stopDep = child_stop->GetDepartures();

		auto firstRelevant = stopDep.upper_bound(departureTime);
		auto preventInfiniteCycle = --stopDep.upper_bound(departureTime);

		Date departureDate = earliestDeparture.GetDate(); // A need for operating days checking.

		size_t i = 0;
		while (i < count) {
			if (firstRelevant == preventInfiniteCycle) break;

			if (firstRelevant == stopDep.cend()) {
				firstRelevant = stopDep.cbegin(); // Midnight reached.
				++departureDate;
				continue;
			}

			// Check if the trip is operating in required date.
			if (firstRelevant->second->GetTrip().GetService().IsOperatingInDate(departureDate)) {
				// Check if this stop is not the last stop in the trip (meaning to have no successors).
				if (&firstRelevant->second->GetStop() != &(firstRelevant->second->GetTrip().GetStopTimes().cend() - 1)->get()->GetStop()) {
					departures.insert(*firstRelevant);
					i++;
				}

			}

			firstRelevant++;
		}
	}

	auto firstRelevant = departures.upper_bound(departureTime); 
	auto preventInfiniteCycle = --departures.upper_bound(departureTime);
	
	size_t i = 0;
	while (i < count) {
		if (firstRelevant == preventInfiniteCycle) break;

		if (firstRelevant == departures.end()) firstRelevant = departures.begin(); // Midnight reached.
		foundDepartures.push_back(Departure(*firstRelevant->second));

		firstRelevant++;
		i++;
	}

	if (foundDepartures.size() == 0)
		throw NoDeparturesFoundException(station->GetName());


}
