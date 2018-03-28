#include "journey.hpp"

bool Timetables::Structures::journey::add_to_journey(std::shared_ptr<journey_segment> js) {
	if (js->arrival_at_target() == js->departure_from_source()) { // Duration 0 secs. No point of adding it. Move to next. This is an initial state.
		
		if (js->previous_ == nullptr) { // This is the last segment. Sets the previous one as its successor is nullptr and returns false.

			(journey_segments_.end() - 1)->get()->previous_ = nullptr;

			return false;
		}

		else // Empty, but not the last one.
			return add_to_journey(js->previous_);
	}

	else if (js->previous_ == nullptr) { // End of the journey.

		journey_segments_.push_back(js); // Adds the last segment.

		return false; // The next addition will not be performed.
	}

	else {

		journey_segments_.push_back(js);

		return true; // Not the last one.
	}
}

Timetables::Structures::journey::journey(std::shared_ptr<journey_segment> js) {

	add_to_journey(js);

	while (add_to_journey((journey_segments_.end() - 1)->get()->previous_)); // Something to process.

	(**(journey_segments_.end() - 1)).previous_ = nullptr;

	if ((**(journey_segments_.cend() - 1)).trip() == nullptr) { // Change initial departure time if footpath comes first.

		// "The problem" is the journey segment is immutable.

		const stop& source = (**(journey_segments_.cend() - 1)).source_stop();
		const stop& target = (**(journey_segments_.cend() - 1)).target_stop();
		const size_t duration = date_time::difference((**(journey_segments_.cend() - 1)).arrival_at_target(), (**(journey_segments_.cend() - 1)).departure_from_source());

		journey_segments_.pop_back();

		journey_segments_.push_back(std::make_shared<footpath_segment>((**(journey_segments_.cend() - 1)).departure_from_source(), source, target, duration, nullptr));
	}

	std::reverse(journey_segments_.begin(), journey_segments_.end());
}

bool Timetables::Structures::journey::operator< (const Timetables::Structures::journey& other) const {
	if (arrival_time() != other.arrival_time())
		return arrival_time() < other.arrival_time();
	else if (journey_segments_.size() != other.journey_segments_.size())
		return journey_segments_.size() < other.journey_segments_.size();
	else if (departure_time() != other.departure_time())
		return departure_time() < other.departure_time();
	else if (number_of_stops() != other.number_of_stops())
		return number_of_stops() < other.number_of_stops();
	else
		return duration_of_transfers() < other.duration_of_transfers();
}
