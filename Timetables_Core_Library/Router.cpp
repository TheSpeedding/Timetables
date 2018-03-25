#include "router.hpp"
#include <tbb/tbb.h>
#include <thread>

using namespace std;
using namespace tbb;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;
using namespace Timetables::Algorithms;

void Timetables::Algorithms::router::accumulate_routes() {

	// Accumulate routes serving marked stops from previous round.

	active_routes_.clear(); // 7th row of pseudocode.

	for (auto&& stop : marked_stops_) { // 8th row of pseudocode.

		for (auto&& route : stop->throughgoing_routes()) { // 9th row of pseudocode.

			auto some_stop = active_routes_.find(route);

			if (some_stop != active_routes_.cend()) { // 10th row of pseudocode.

				if (route->stop_comes_before(*stop, *some_stop->second)) { // 11th row of pseudocode.

					active_routes_.erase(route); 

					active_routes_.insert(make_pair(route, stop)); // 11th row of pseudocode.

				}

			}

			else // 12th row of pseudocode.

				active_routes_.insert(make_pair(route, stop)); // 13th row of pseudocode.

		}

	}

	marked_stops_.clear(); // 14th row of pseudocode.

}


void Timetables::Algorithms::router::traverse_each_route() {

	for (auto&& item : active_routes_)
		traverse_route(*item.first, *item.second);

	/*
	int available_cores = std::thread::hardware_concurrency();

	int threads_to_start = active_routes_.size() / 10 + 1;

	if (threads_to_start > available_cores)
		threads_to_start = available_cores;

	vector<vector<pair<const route*, const stop*>>> buckets;

	for (size_t i = 0; i < threads_to_start; i++)
		buckets.push_back(vector<pair<const route*, const stop*>>());

	size_t i = 0;

	for (auto&& item : active_routes_) {

		buckets[i % threads_to_start].push_back(item); // Divide items into buckets equally so multiple threads cant process it in parallel.

		i++;
	}

	task_group g;

	for (auto task = buckets.begin(); task != buckets.end(); ++task) {
		g.run([=] {
			for (auto&& item : *task) // 15th row of pseudocode.
				traverse_route(*item.first, *item.second);
		});
	}

	g.wait();
	*/
}

void Timetables::Algorithms::router::look_at_footpaths() {

	vector<const stop*> temp_marked;

	for (auto&& stopA : marked_stops_) { // 24th row of pseudocode.

		for (auto&& footpath : stopA->footpaths()) { // 25th row of pseudocode.

			size_t duration = footpath.first;

			const stop* stop_B = footpath.second;

			auto arrival_time_A = (labels_.cend() - 1)->find(stopA);

			auto arrival_time_B = (labels_.cend() - 1)->find(stop_B);

			Timetables::Structures::date_time min = date_time::infinity();

			if (arrival_time_A == (labels_.cend() - 1)->cend() && (arrival_time_B != (labels_.cend() - 1)->cend()))

				min = arrival_time_B->second;

			else if (arrival_time_A != (labels_.cend() - 1)->cend() && (arrival_time_B == (labels_.cend() - 1)->cend()))

				min = arrival_time_A->second.add_seconds(duration);

			else if (arrival_time_A != (labels_.cend() - 1)->cend() && (arrival_time_B != (labels_.cend() - 1)->cend()))

				min = arrival_time_B->second <= arrival_time_A->second.add_seconds(duration) ? arrival_time_B->second : arrival_time_A->second.add_seconds(duration);

			else

				throw runtime_error("Undefined state.");
			
			if (((arrival_time_B == (labels_.cend() - 1)->cend()) || // We have not arrive to the stop yet. Set new arrival time.
				(arrival_time_B == (labels_.cend() - 1)->cend() && min != arrival_time_B->second)) && // We can improve the arrival to the stop.
				&target_ != &stop_B->parent_station()) // The stop is the target station. No need to add footpath.
			{
				
				(labels_.end() - 1)->erase(stop_B); 

				(labels_.end() - 1)->insert(make_pair(stop_B, min)); // 26th row of pseudocode.
				
				journey new_journey((journeys_.cend() - 1)->find(stopA)->second); // The same journey, added just some footpath -> arrival time increased.

				new_journey.set_last_transfer(duration);

				(journeys_.end() - 1)->erase(stop_B); 

				(journeys_.end() - 1)->insert(make_pair(stop_B, new_journey)); 

			}


			temp_marked.push_back(stop_B); // Temp because it may invalidate iterators.

		}

	}

	for (auto&& stop : temp_marked)
		marked_stops_.insert(stop); // 27th row of pseudocode.

}

void Timetables::Algorithms::router::traverse_route(const Timetables::Structures::route& current_route, const Timetables::Structures::stop& starting_stop) {

	const trip* current_trip = nullptr; // 16th row of pseudocode.
	date_time date_for_current_trip = date_time::infinity();
	
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

			date_time new_arrival = date_for_current_trip.add_seconds(st.arrival_since_midnight()); // 18th row of pseudocode.

			auto current_stop_best_arrival = temp_labels_.find(&current_stop);

			auto target_stop_best_arrival = temp_labels_.cend();

			for (auto&& stop : target_.child_stops())
				if (temp_labels_.find(stop) != temp_labels_.cend() && (target_stop_best_arrival == temp_labels_.cend() || temp_labels_.find(stop)->second < target_stop_best_arrival->second))
					target_stop_best_arrival = temp_labels_.find(stop); // 18th row of pseudocode.


			if ((current_stop_best_arrival == temp_labels_.cend() && target_stop_best_arrival == temp_labels_.cend()) || // Then the minimum is an infinity. Process the if block.
				(current_stop_best_arrival == temp_labels_.cend() && target_stop_best_arrival != temp_labels_.cend() && new_arrival < target_stop_best_arrival->second) ||
				(current_stop_best_arrival != temp_labels_.cend() && target_stop_best_arrival == temp_labels_.cend() && new_arrival < current_stop_best_arrival->second) ||
				(current_stop_best_arrival != temp_labels_.cend() && target_stop_best_arrival != temp_labels_.cend() && new_arrival < (target_stop_best_arrival->second < current_stop_best_arrival->second ? target_stop_best_arrival->second : current_stop_best_arrival->second))
				) { // 18th row of pseudocode.

				//(labels_.end() - 1)->erase(&current_stop);
				//temp_labels_.erase(&current_stop);

				(labels_.end() - 1)->insert(make_pair(&current_stop, new_arrival)); // 19th row of pseudocode.
				temp_labels_.insert(make_pair(&current_stop, new_arrival)); // 20th row of pseudocode.

				if (journeys_.size() == 2) {

					journey_segment segment(*current_trip, new_arrival, *boarding_stop, current_stop);

					//(journeys_.end() - 1)->erase(&current_stop);

					(journeys_.end() - 1)->insert(make_pair(&current_stop, journey(move(segment))));
				}

				else {

					journey current_journey((journeys_.end() - 2)->find(boarding_stop)->second);

					journey_segment segment(*current_trip, new_arrival, *boarding_stop, current_stop);

					current_journey.add_to_journey(move(segment));

					if ((journeys_.end() - 1)->find(&current_stop) == (journeys_.end() - 1)->cend() || (journeys_.end() - 1)->find(boarding_stop) == (journeys_.end() - 1)->cend() ||
						current_journey.total_transfer_time() <= (journeys_.end() - 1)->find(boarding_stop)->second.total_transfer_time()) { // The transfer time is no worse. We can update the journey.

						//(journeys_.end() - 1)->erase(&current_stop);

						(journeys_.end() - 1)->insert(make_pair(&current_stop, move(current_journey)));

					}
				}

				marked_stops_.insert(&current_stop); // 21st row of pseudocode.
			}


			// Can we catch an earlier trip?

			auto previous_arrival = (labels_.end() - 2)->find(&current_stop);

			if (previous_arrival != (labels_.end() - 2)->cend() && previous_arrival->second <= date_for_current_trip.add_seconds(st.departure_since_midnight())) { // 22nd row of pseudocode.

				auto res = find_earliest_trip(current_route, previous_arrival->second, next_stop - current_route.stops().cbegin());
				current_trip = res.first; // 23rd row of pseudocode.
				date_for_current_trip = move(res.second);
				boarding_stop = &current_stop; // We have just boarded the trip.

			}
		}

		// Can we catch an earlier trip?

		if (current_trip == nullptr) { // 22nd row of pseudocode.

			auto previous_arrival = (labels_.end() - 2)->find(&current_stop);

			if (previous_arrival != (labels_.end() - 2)->cend()) {
				auto res = find_earliest_trip(current_route, previous_arrival->second, next_stop - current_route.stops().cbegin());
				current_trip = res.first; // 23rd row of pseudocode.
				date_for_current_trip = move(res.second);
				boarding_stop = &current_stop; // We have just boarded the trip.
			}
		}

	}
}

std::pair<const Timetables::Structures::trip*, Timetables::Structures::date_time> Timetables::Algorithms::router::find_earliest_trip(const Timetables::Structures::route& route, const Timetables::Structures::date_time& arrival, std::size_t stop_index) {

	date_time new_departure_date = arrival.date();
	size_t days = 0;
	
	// Searches for trip in horizon of a week. If not found, terminates looking for a journey using this route.

	for (auto it = route.trips().cbegin(); days < 7; ++it) {
		
		if (it == route.trips().cend()) {
			it = route.trips().cbegin();
			days++;
			new_departure_date = new_departure_date.add_days(1);
		}

		const stop_time& st = *((**it).stop_times().cbegin() + stop_index);

		date_time new_departure_date_time = new_departure_date.add_seconds(st.departure_since_midnight() % 86400);
				
		if (new_departure_date_time > arrival && st.is_operating_in_date_time(new_departure_date_time))

			return make_pair(*it, new_departure_date_time.add_seconds((-1) * st.departure_since_midnight()));

	}

	return make_pair(nullptr, 0);
}

void Timetables::Algorithms::router::obtain_journeys() {
	
	const journey* previous_fastest_journey = obtain_journey(earliest_departure_);

	// We tried to search a journey but no journeys found. No point of continuing.

	if (fastest_journeys_.size() == 0)
		return;		

	for (int i = 1; i < count_; i++) {
		const journey* temp = obtain_journey(previous_fastest_journey->departure_time().add_seconds(1));

		if (fastest_journeys_.size() >= count_ + 1 && previous_fastest_journey->arrival_time() <= temp->arrival_time()) // Number of total journeys reached. We have found some journey but it is worse than each from the previous one. No point of continuing.
			break;		

		previous_fastest_journey = temp;
	}

	auto it = fastest_journeys_.begin();
	for (int i = 0; i < count_ && it != fastest_journeys_.end(); ++it, i++);

	fastest_journeys_.erase(it, fastest_journeys_.end()); // Delete unwanted journeys.
}

const Timetables::Structures::journey* Timetables::Algorithms::router::obtain_journey(const Timetables::Structures::date_time& departure) {

	labels_.clear();
	temp_labels_.clear();
	marked_stops_.clear();
	active_routes_.clear();
	journeys_.clear();

	labels_.push_back(unordered_map<const stop*, date_time>());

	journeys_.push_back(unordered_map<const stop*, journey>());

	// Using 0 trips we are able to reach all the stops in the station in departure time (meaning 0 seconds).

	for (auto&& stop : source_.child_stops()) {
		labels_.at(0).insert(make_pair(stop, departure)); // 4th row of pseudocode.
		marked_stops_.insert(stop); // 5th row of pseudocode.

		// We are also to reach the stops that are connected with footpaths.

		for (auto&& footpath : stop->footpaths()) {
			if (&footpath.second->parent_station() != &source_) {
				labels_.at(0).insert(make_pair(footpath.second, departure.add_seconds(footpath.first)));
				marked_stops_.insert(footpath.second);
			}
		}
	}

	for (size_t k = 1; marked_stops_.size() > 0 && k < max_transfers_; k++) { // 6th && 28th && 29th row of pseudocode.

		labels_.push_back(unordered_map<const stop*, date_time>());

		journeys_.push_back(unordered_map<const stop*, journey>());

		accumulate_routes();
		traverse_each_route();
		look_at_footpaths();

	}
	
	// Adds all the suitable journeys to the map.

	const journey* fastest_journey = nullptr;

	for (size_t i = 0; i < max_transfers_ && i < journeys_.size(); i++)
		for (auto&& stop : target_.child_stops()) {
			
			auto res = journeys_[i].find(stop);

			if (res != journeys_[i].cend()) {
				fastest_journeys_.insert(make_pair(res->second.arrival_time(), res->second));
				if (fastest_journey == nullptr || fastest_journey->arrival_time() > res->second.arrival_time())
					fastest_journey = &(--fastest_journeys_.upper_bound(res->second.arrival_time()))->second; // Items are being inserted into an upper bound.
			}

		}

	return fastest_journey; 
}

Timetables::Structures::journey_segment::journey_segment(const Timetables::Structures::trip& trip, const Timetables::Structures::date_time& arrival, const stop& source, const stop& target) : trip_(&trip), arrival_(arrival) {

	for (std::vector<stop_time>::const_iterator it = trip.stop_times().cbegin(); it != trip.stop_times().cend(); ++it) {

		if (&it->stop() == &source) source_stop_ = it;

		if (&it->stop() == &target) target_stop_ = it;

	}

}

const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> Timetables::Structures::journey_segment::intermediate_stops() const {
	vector<std::pair<std::size_t, const Timetables::Structures::stop*>> stops;
	size_t base = source_stop_->departure();
	for (auto it = source_stop_; it <= target_stop_; ++it)
		stops.push_back(make_pair(it->arrival() - base, &it->stop()));
	return move(stops);
}
