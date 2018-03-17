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

Departure::Departure(const Timetables::Structures::StopTime& stopTime, const Timetables::Structures::DateTime& dep) : departure(dep) {	
	const vector<StopTime>& stopTimes = stopTime.Trip().StopTimes();
	tripBegin = stopTimes.cbegin() + (&stopTime - stopTimes.data()); // We can do this ONLY because we hold stop times in trips. So they are stored contiguously in vector.
	tripEnd = stopTimes.cend();
}

const std::vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> Departure::FollowingStops() const {
	vector<std::pair<std::size_t, const Timetables::Structures::Stop*>> stops;
	size_t base = tripBegin->Departure();
	for (auto it = tripBegin + 1; it != tripEnd; ++it)
		stops.push_back(make_pair(it->Arrival() - base, &it->Stop()));
	return move(stops);
}

void Timetables::Algorithms::DepartureBoard::ObtainDepartureBoard() {
		
	auto departureTime = earliestDeparture.Time(); // Lower bound for departures

	auto departureDate = earliestDeparture.Date(); // A need for operating days checking.
	
	auto firstRelevant = station.Departures().upper_bound(departureTime);

	size_t days = 0;
	while (foundDepartures.size() < count && days < 7) {

		if (firstRelevant == station.Departures().cend()) {
			firstRelevant = station.Departures().cbegin(); // Midnight reached.
			departureDate = departureDate.AddDays(1);
			days++; // We will count the days to prevent infinite cycle. Seven days considered to be a maximum.
		}

		// Check if the stop-time (trip respectively) is operating in required date.
		if (firstRelevant->second->IsOperatingInDateTime(departureDate + firstRelevant->first))
			// Check if this stop is not the last stop in the trip (meaning to have no successors).
			if (&firstRelevant->second->Stop() != &(firstRelevant->second->Trip().StopTimes().cend() - 1)->Stop())
				foundDepartures.push_back(Departure(*firstRelevant->second, // Stop time.
					DateTime((firstRelevant->second->Trip().Departure() + firstRelevant->second->Departure()) % 86400) // Time of the departure.
					+ departureDate.Date() // Date of the departure.
				));

		firstRelevant++;
	}

	if (foundDepartures.size() == 0)
		throw NoDeparturesFoundException(station.Name());

}