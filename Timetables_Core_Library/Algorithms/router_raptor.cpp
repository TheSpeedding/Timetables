#include "../Algorithms/router_raptor.hpp"
// #include <tbb/tbb.h>
#define PARALLEL_VERSION false

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::router_raptor::accumulate_routes() {

	// Accumulate routes serving marked stops from previous round.

	active_routes_.clear(); // 7th row of pseudocode.

	for (auto&& stop : marked_stops_) { // 8th row of pseudocode.

		for (auto&& route : stop->throughgoing_routes()) { // 9th row of pseudocode.
			
			if (static_cast<int>(mot_ & route->info().type()) != 0) { // Flag for this mean of transport is set on. Continue with accumulating.

				auto some_stop = active_routes_.find(route);

				if (some_stop != active_routes_.cend()) { // 10th row of pseudocode.

					if (route->stop_comes_before(*stop, *some_stop->second)) { // 11th row of pseudocode.

						active_routes_[route] = stop; // 11th row of pseudocode.

					}

				}

				else // 12th row of pseudocode.

					active_routes_.insert(make_pair(route, stop)); // 13th row of pseudocode.

			}

		}

	}

	marked_stops_.clear(); // 14th row of pseudocode.

}


void Timetables::Algorithms::router_raptor::traverse_each_route() {

#if PARALLEL_VERSION
	// This cannot be used at the moment because of data races in containers.
	tbb::parallel_for(active_routes_.begin(), active_routes_.end(), [&](decltype(active_routes_)::iterator it) {
		traverse_route(*it->first, *it->second);
	}, auto_partitioner());
#else	
	for (auto&& item : active_routes_)
		traverse_route(*item.first, *item.second);
#endif
}

void Timetables::Algorithms::router_raptor::look_at_footpaths() {

	vector<const stop*> temp_marked;

	for (auto&& stop_A : marked_stops_) { // 24th row of pseudocode.

		for (auto&& footpath : stop_A->footpaths()) { // 25th row of pseudocode.

			int duration = (int)(footpath.first * transfers_coefficient_) + 1;

			const stop* stop_B = footpath.second;

			auto arrival_time_A = (journeys_.cend() - 1)->find(stop_A);

			auto arrival_time_B = (journeys_.cend() - 1)->find(stop_B);

			date_time min;

			if (arrival_time_A == (journeys_.cend() - 1)->cend() && (arrival_time_B != (journeys_.cend() - 1)->cend()))

				min = arrival_time_B->second->arrival_at_target();

			else if (arrival_time_A != (journeys_.cend() - 1)->cend() && (arrival_time_B == (journeys_.cend() - 1)->cend()))

				min = date_time(arrival_time_A->second->arrival_at_target(), duration);

			else {

				date_time arrival_with_transfer(arrival_time_A->second->arrival_at_target(), duration);

				min = arrival_time_B->second->arrival_at_target() <= arrival_with_transfer ? arrival_time_B->second->arrival_at_target() : arrival_with_transfer;

			}

			if ((arrival_time_B == (journeys_.cend() - 1)->cend() || // We have not arrive to the stop yet. Set new arrival time.
				(arrival_time_A->second->trip() != nullptr && min < arrival_time_B->second->arrival_at_target())) && // We can improve the arrival to the stop and the previous segment is not a footpath.
				&target_ != &stop_B->parent_station()) { // The stop is the target station. No need to add footpath.

				shared_ptr<journey_segment> previous = (journeys_.cend() - 1)->find(stop_A)->second; // The same journey, added just some footpath -> arrival time increased.

				(journeys_.end() - 1)->operator[](stop_B).reset(new footpath_segment(min, *stop_A, *stop_B, duration, previous));

			}


			temp_marked.push_back(stop_B); // Temp because it may invalidate iterators.

		}

	}

	for (auto&& stop : temp_marked)
		marked_stops_.insert(stop); // 27th row of pseudocode.

}

void Timetables::Algorithms::router_raptor::traverse_route(const Timetables::Structures::route& current_route, const Timetables::Structures::stop& starting_stop) {

	const trip* current_trip = nullptr; // 16th row of pseudocode.
	date_time date_for_current_trip;
	service_state state;

	// Find the current stop in route.

	vector<const stop*>::const_iterator next_stop;

	for (next_stop = current_route.stops().cbegin(); next_stop != current_route.stops().cend(); ++next_stop) // PERFORMANCE OK. Faster than std::find.
		if (*next_stop == &starting_stop)
			break;

	// Traverse the route from current stop.

	const stop* boarding_stop = nullptr;

	for (; next_stop != current_route.stops().cend(); ++next_stop) { // 17th row of pseudocode.

																	 // Can the label be improved in this round? Includes local and target pruning.

		const stop& current_stop = **next_stop;

		if (current_trip != nullptr) { // 18th row of pseudocode.

			const stop_time& st = *(current_trip->stop_times().cbegin() + (next_stop - current_route.stops().cbegin()));

			date_time new_arrival(date_for_current_trip, st.arrival_since_midnight()); // 18th row of pseudocode.

			auto current_stop_best_arrival = temp_labels_.find(&current_stop);

			auto target_stop_best_arrival = temp_labels_.cend();

			for (auto&& stop : target_.child_stops()) {
				auto found = temp_labels_.find(stop);
				if (found != temp_labels_.cend() && (target_stop_best_arrival == temp_labels_.cend() || found->second < target_stop_best_arrival->second))
					target_stop_best_arrival = found; // 18th row of pseudocode.
			}

			if ((current_stop_best_arrival == temp_labels_.cend() && target_stop_best_arrival == temp_labels_.cend()) || // Then the minimum is an infinity. Process the if block.
				(current_stop_best_arrival == temp_labels_.cend() && target_stop_best_arrival != temp_labels_.cend() && new_arrival < target_stop_best_arrival->second) ||
				(current_stop_best_arrival != temp_labels_.cend() && target_stop_best_arrival == temp_labels_.cend() && new_arrival < current_stop_best_arrival->second) ||
				(current_stop_best_arrival != temp_labels_.cend() && target_stop_best_arrival != temp_labels_.cend() && new_arrival < (target_stop_best_arrival->second < current_stop_best_arrival->second ? target_stop_best_arrival->second : current_stop_best_arrival->second))
				) { // 18th row of pseudocode.

				temp_labels_[&current_stop] = new_arrival; // 20th row of pseudocode.

				shared_ptr<journey_segment> previous = (journeys_.end() - 2)->find(boarding_stop)->second;

				(journeys_.end() - 1)->operator[](&current_stop).reset(new trip_segment(*current_trip, new_arrival, *boarding_stop, current_stop, previous, state == outdated)); // 19th row of pseudocode.

				marked_stops_.insert(&current_stop); // 21st row of pseudocode.
			}


			// Can we catch an earlier trip?

			auto previous_arrival = (journeys_.end() - 2)->find(&current_stop);

			if (previous_arrival != (journeys_.end() - 2)->cend() && previous_arrival->second->arrival_at_target() <= date_time(date_for_current_trip, st.departure_since_midnight())) { // 22nd row of pseudocode.

				auto res = find_earliest_trip(current_route, previous_arrival->second->arrival_at_target(), next_stop - current_route.stops().cbegin());
				current_trip = get<0>(res); // 23rd row of pseudocode.
				date_for_current_trip = get<1>(res);
				state = get<2>(res);
				boarding_stop = &current_stop; // We have just boarded the trip.

			}
		}

		// Can we catch an earlier trip?

		if (current_trip == nullptr) { // 22nd row of pseudocode.

			auto previous_arrival = (journeys_.end() - 2)->find(&current_stop);

			if (previous_arrival != (journeys_.end() - 2)->cend()) {
				auto res = find_earliest_trip(current_route, previous_arrival->second->arrival_at_target(), next_stop - current_route.stops().cbegin());
				current_trip = get<0>(res); // 23rd row of pseudocode.
				date_for_current_trip = get<1>(res);
				state = get<2>(res);
				boarding_stop = &current_stop; // We have just boarded the trip.
			}
		}

	}
}

std::tuple<const Timetables::Structures::trip*, Timetables::Structures::date_time, Timetables::Structures::service_state> Timetables::Algorithms::router_raptor::find_earliest_trip(const Timetables::Structures::route& route, const Timetables::Structures::date_time& arrival, std::size_t stop_index) {

	date_time new_departure_date = arrival.date();
	size_t days = 0;

	// Searches for trip in horizon of a week. If not found, terminates looking for a journey using this route.

	for (auto it = route.trips().cbegin(); days < 7; ++it) {

		if (it == route.trips().cend()) {
			it = route.trips().cbegin();
			days++;
			new_departure_date = date_time(new_departure_date, DAY);
		}

		const stop_time& st = *(it->stop_times().cbegin() + stop_index);

		date_time new_departure_date_time(date_time(new_departure_date, DAY * (st.departure_since_midnight() / DAY)), st.departure_since_midnight() % DAY);

		if (new_departure_date_time >= arrival) {

			service_state s = st.is_operating_in_date_time(new_departure_date_time);

			if (s != not_operating)
				return make_tuple(&*it, date_time(new_departure_date_time, (-1) * st.departure_since_midnight()), s);

		}

	}

	return make_tuple(nullptr, 0, not_operating);
}

void Timetables::Algorithms::router_raptor::obtain_journeys() {

	const journey* previous_fastest_journey = obtain_journey(earliest_departure_);

	// We tried to search a journey but no journeys found. No point of continue.

	if (fastest_journeys_.size() == 0)
		return;

	time_t inc_time = SECOND;

	for (/*size_t i = 1*/; search_by_arrival_ ? previous_fastest_journey->arrival_time() < maximal_arrival_ : /*i* < count_*/ true; /*i++*/) {

		const journey* current_fastest_journey = obtain_journey(date_time(previous_fastest_journey->departure_time(), inc_time));

		if (current_fastest_journey == nullptr) // No journey found.
			break;
		
		if (!search_by_arrival_ && fastest_journeys_.size() >= count_ + 1 && previous_fastest_journey < current_fastest_journey) // Number of total journeys reached. We have found some journey but it is worse than each from the previous one. No point of continuing.
			break;
		
		inc_time = previous_fastest_journey->arrival_time() >= current_fastest_journey->arrival_time() ? 
			date_time::difference(previous_fastest_journey->arrival_time(), current_fastest_journey->arrival_time()) + 2 * inc_time + MINUTE : SECOND;

		if (inc_time > DAY) 
			break;

		previous_fastest_journey = current_fastest_journey;
	}

	if (!search_by_arrival_) {
		auto it = fastest_journeys_.begin();
		for (size_t i = 0; i < count_ && it != fastest_journeys_.end(); ++it, i++);

		fastest_journeys_.erase(it, fastest_journeys_.end()); // Delete unwanted journeys.
	}
}

const Timetables::Structures::journey* Timetables::Algorithms::router_raptor::obtain_journey(const Timetables::Structures::date_time& departure) {

	temp_labels_.clear();
	marked_stops_.clear();
	active_routes_.clear();
	journeys_.clear();

	journeys_.push_back(map<const stop*, shared_ptr<journey_segment>>());

	// Using 0 trips we are able to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source_.child_stops()) {
		marked_stops_.insert(stop); // 5th row of pseudocode.
		journeys_[0][stop].reset(new footpath_segment(departure, *stop, *stop, 0, nullptr)); // 4th row of pseudocode.

		// We are also able to reach the stops that are connected with footpaths.

		for (auto&& footpath : stop->footpaths()) {
			if (&footpath.second->parent_station() != &source_) {

				bool suitable = true;

				for (auto&& route : footpath.second->throughgoing_routes())
					if (route->contains_station(source_))
						suitable = false;
				
				if (suitable) {
					marked_stops_.insert(footpath.second);
					journeys_[0][footpath.second].reset(new footpath_segment(date_time(departure, (int)(footpath.first * transfers_coefficient_) + 1), *stop, *footpath.second, (int)(footpath.first * transfers_coefficient_) + 1, nullptr));	
				}
			}
		}
	}

	for (size_t k = 1; marked_stops_.size() > 0 && k < max_transfers_ + 1; k++) { // 6th && 28th && 29th row of pseudocode.

		journeys_.push_back(map<const stop*, shared_ptr<journey_segment>>());

		accumulate_routes();
		traverse_each_route();
		look_at_footpaths();

	}

	// Adds all the suitable journeys to the map.

	const journey* fastest_journey = nullptr; // Fastest journey found in this round.
	
	for (size_t i = 0; i < max_transfers_ && i < journeys_.size(); i++)
		for (auto&& stop : target_.child_stops()) {

			auto res = journeys_[i].find(stop);

			if (res != journeys_[i].cend()) {

				journey found_journey(res->second);

				auto inserted = fastest_journeys_.insert(found_journey);

				if (fastest_journey == nullptr || *inserted.first < *fastest_journey) // No check if journey was added is intended. Otherwise it would broke the algorithm.
					fastest_journey = &*inserted.first;
			}

		}

	return fastest_journey;
}