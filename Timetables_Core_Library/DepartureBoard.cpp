#include "Stops.hpp"
#include "Trips.hpp"
#include "DataFeed.hpp"
#include "StopTime.hpp"
#include "Exceptions.hpp"
#include <vector>
#include <string>
#include <map>
#include "DepartureBoard.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Departure::Departure(const Timetables::Structures::StopTime& stopTime, std::time_t dep) : departure(dep) {
	
	const vector<StopTime>& stopTimes = stopTime.Trip().StopTimes();

	for (vector<StopTime>::const_iterator it = stopTimes.cbegin(); it != stopTimes.cend(); ++it)
		if (&*it == &stopTime)
			tripBegin = it;

	tripEnd = stopTimes.cend();
}

const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> Departure::FollowingStops() const {
	vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> stops;
	size_t base = tripBegin->Departure();
	for (auto it = tripBegin; it != tripEnd; ++it)
		stops.push_back(make_pair(it->Arrival() - base, &it->Stop()));
	return move(stops);
}

void Timetables::Algorithms::DepartureBoard::ObtainDepartureBoard() {
	
	// In this structure we will store "count" departures from each child stop (meaning count * childStops.size()).
	// Then we will return first "count" departures sorted by time.

	multimap<std::time_t, const StopTime*> departures;

	std::time_t departureTime = DateTime::Time(earliestDeparture); // Lower bound for departures

	for (auto&& childStop : station.ChildStops()) {
		const multimap<std::time_t, const StopTime*>& stopDepartures = childStop->Departures();

		auto firstRelevant = stopDepartures.upper_bound(departureTime);

		std::time_t departureDate = DateTime::Date(earliestDeparture); // A need for operating days checking.

		size_t foundDeparturesCount = 0, days = 0;
		while (foundDeparturesCount < count && days < 7) {

			if (firstRelevant == stopDepartures.cend()) {
				firstRelevant = stopDepartures.cbegin(); // Midnight reached.
				departureDate = DateTime::AddDays(departureDate, 1);
				days++; // We will count the days to prevent infinite cycle. Seven days considered to be a maximum.
				continue;
			}

			// Check if the stop-time (trip respectively) is operating in required date.
			if (firstRelevant->second->IsOperatingInDate(departureDate)) {
				// Check if this stop is not the last stop in the trip (meaning to have no successors).
				if (&firstRelevant->second->Stop() != &(firstRelevant->second->Trip().StopTimes().cend() - 1)->Stop()) {
					departures.insert(make_pair(
						(firstRelevant->second->Trip().Departure() + firstRelevant->second->Departure()) % 86400 // Time of the departure.
						+ DateTime::Date(departureDate) // Date of the departure.
					, firstRelevant->second));
					foundDeparturesCount++;
				}
			}

			firstRelevant++;
		}
	}

	size_t i = 0;
	for (auto it = departures.cbegin(); i < count && it != departures.cend(); ++it, i++)
		foundDepartures.push_back(Departure(*it->second, it->first));

	if (foundDepartures.size() == 0)
		throw NoDeparturesFoundException(station.Name());

}