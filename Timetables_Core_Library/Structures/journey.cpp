#include "journey.hpp"

using namespace Timetables::Structures;
using namespace std;

Timetables::Structures::journey::journey(std::shared_ptr<journey_segment> js) {
	
	journey_segments_.push_back(js); // Initial segment.
	js = js->previous_;

	while (js != nullptr) { // Something to process.

		if (js->duration() > 0) { 

			journey_segments_.push_back(js->find_later_departure((**(journey_segments_.cend() - 1)).departure_from_source()));

		}

		js = js->previous_; // Some kind of linked list.
	}


	std::reverse(journey_segments_.begin(), journey_segments_.end());
}

bool Timetables::Structures::journey::operator< (const Timetables::Structures::journey& other) const {
	if (arrival_time() != other.arrival_time())
		return arrival_time() < other.arrival_time();
	else if (duration_without_waiting_times() != other.duration_without_waiting_times())
		return duration_without_waiting_times() < other.duration_without_waiting_times();
	else if (journey_segments_.size() != other.journey_segments_.size())
		return journey_segments_.size() < other.journey_segments_.size();
	else if (number_of_stops() != other.number_of_stops())
		return number_of_stops() < other.number_of_stops();
	else  if (duration_of_transfers() != other.duration_of_transfers())
		return duration_of_transfers() < other.duration_of_transfers();
	else if (departure_time() != other.departure_time())
		return departure_time() < other.departure_time();

	// Journeys might look identical, but they might not be. Some of them may use different line.

	else {
		for (auto it1 = journey_segments_.cbegin(), it2 = other.journey_segments_.cbegin(); it1 != journey_segments_.cend(); ++it1, ++it2) {
			if ((**it1).trip() != (**it2).trip())
				return (**it1).trip() < (**it2).trip();
		}
	}
}

std::shared_ptr<journey_segment> Timetables::Structures::trip_segment::find_later_departure(const Timetables::Structures::date_time& latest_arrival) const {

	auto new_arrival_date = latest_arrival.date();

	const Timetables::Structures::trip* best_trip = trip_;

	service_state s = outdated_ ? service_state::outdated : service_state::operating;

	size_t stop_index = target_stop_ - trip_->stop_times().cbegin();

	size_t days = 0;

	date_time new_arrival_date_time = arrival_;

	auto it = trip_->route().trips().cbegin();

	while (&*it != trip_)
		it++;

	for (; days < 7; ++it) {

		if (it == trip_->route().trips().cend()) {
			it = trip_->route().trips().cbegin();
			days++;
			new_arrival_date = date_time(new_arrival_date, DAY);
		}

		const stop_time& st = *(it->stop_times().cbegin() + stop_index);

		if (date_time(date_time(new_arrival_date, DAY * (st.arrival_since_midnight() / DAY)), st.arrival_since_midnight() >= DAY ? st.arrival_since_midnight() % DAY : st.arrival_since_midnight()) > latest_arrival)
			break;

		new_arrival_date_time = date_time(date_time(new_arrival_date, DAY * (st.arrival_since_midnight() / DAY)), st.arrival_since_midnight() >= DAY ? st.arrival_since_midnight() % DAY : st.arrival_since_midnight());
		
		s = st.is_operating_in_date_time(date_time(new_arrival_date_time, st.departure() - st.arrival()));

		if (s != not_operating)
			best_trip = &*it;
	}

	return std::make_shared<trip_segment>(*best_trip, new_arrival_date_time, source_stop(), target_stop(), previous_, s == service_state::outdated);
}
