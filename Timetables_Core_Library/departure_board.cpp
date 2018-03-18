#include "stops.hpp"
#include "trips.hpp"
#include "data_feed.hpp"
#include "stop_time.hpp"
#include "exceptions.hpp"
#include <vector>
#include <string>
#include <map>
#include "departure_board.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

departure::departure(const Timetables::Structures::stop_time& stop_time, const Timetables::Structures::date_time& dep) : departure_(dep) {	
	const vector<Timetables::Structures::stop_time>& stop_times = stop_time.trip().stop_times();
	trip_begin_ = stop_times.cbegin() + (&stop_time - stop_times.data()); // We can do this ONLY because we hold stop times in trips. So they are stored contiguously in a vector.
	trip_end_ = stop_times.cend();
}

const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> departure::following_stops() const {
	vector<std::pair<std::size_t, const Timetables::Structures::stop*>> stops;
	size_t base = trip_begin_->departure(); // Departure time from following stops will be represented as an offset (in seconds) from departure from required stop.
	for (auto it = trip_begin_ + 1; it != trip_end_; ++it)
		stops.push_back(make_pair(it->arrival() - base, &it->stop()));
	return move(stops);
}

void Timetables::Algorithms::departure_board::obtain_departure_board() {
		
	auto departure_time = earliest_departure_.time(); // Lower bound for departures

	auto departure_date = earliest_departure_.date(); // A need for operating days checking.
	
	auto first_relevant = station_.departures().upper_bound(departure_time); // Upper bound of earliest departure.

	size_t days = 0;
	while (found_departures_.size() < count_ && days < 7) {

		if (first_relevant == station_.departures().cend()) {
			first_relevant = station_.departures().cbegin(); // Midnight reached.
			departure_date = departure_date.add_days(1);
			days++; // We will count the days to prevent infinite cycle. Seven days considered to be a maximum.
		}

		// Check if the stop-time (trip respectively) is operating in required date.
		if (first_relevant->second->is_operating_in_date_time(departure_date + first_relevant->first)) // We will determine expected date time of departure and then look back to trip beginning day if it operates.
			// Check if this stop is not the last stop in the trip (meaning to have no successors).
			if (&first_relevant->second->stop() != &(first_relevant->second->trip().stop_times().cend() - 1)->stop())
				found_departures_.push_back(departure(*first_relevant->second, // Stop time.
					date_time((first_relevant->second->trip().departure() + first_relevant->second->departure()) % 86400) // Time of the departure.
					+ departure_date.date() // Date of the departure.
				));

		first_relevant++;
	}

	if (found_departures_.size() == 0) // This may be removed later.
		throw no_departures_found(station_.name());
}