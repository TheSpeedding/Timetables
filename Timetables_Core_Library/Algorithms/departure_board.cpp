#include "../Structures/stops.hpp"
#include "../Structures/trips.hpp"
#include "../Structures/data_feed.hpp"
#include "../Structures/stop_time.hpp"
#include <vector>
#include <string>
#include <map>
#include "../Algorithms/departure_board.hpp"

using namespace std;
using namespace Timetables::Structures;

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
	
	// We have to find "count" departure from each stop of the station. This cannot be done better since we want to support showing departure board from map in mobile application.

	for (auto&& stop : stops_) {

		if (stop->departures().cend() == stop->departures().cbegin()) continue; // The stop contains no departures.

		auto it = stop->departures().cbegin();

		size_t days = 0;
		size_t counter = 0;

		while (counter < count_ && days < 7) {

			if (it == stop->departures().cend()) {
				it = stop->departures().cbegin(); // Midnight reached.
				departure_date = departure_date.add_days(1);
				days++; // We will count the days to prevent infinite cycle. Seven days considered to be a maximum.
			}

			// Check if the departure does not leave before required time.
			if ((days > 0 || (days == 0 && date_time((**it).departure_since_midnight()) >= departure_time)) &&
				// Check if the stop-time (trip respectively) is operating in required date.
				(**it).is_operating_in_date_time(departure_date + (**it).departure_since_midnight())) // We will determine expected date time of departure and then look back to trip beginning day if it operates.
																			  // Check if this stop is not the last stop in the trip (meaning to have no successors).
				if (&(**it).stop() != &((**it).trip().stop_times().cend() - 1)->stop()) {

					date_time dep = date_time(((**it).trip().departure() + (**it).departure()) % 86400) // Time of the departure.
						+ departure_date.date(); // Date of the departure.

					found_departures_.push_back(departure(**it // Stop time.
						, dep)); // Date time of the departure.

					counter++;
				}


			it++;
		}
	}	

	sort(found_departures_.begin(), found_departures_.end());

	if (found_departures_.size() > count_)
		found_departures_.erase(found_departures_.begin() + count_, found_departures_.end());	
}