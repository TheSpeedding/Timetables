#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <vector> // Used in journeys.
#include <unordered_map> // Structure for algorithm
#include <unordered_set>// Structure for algorithm
#include "stop_time.hpp" // Used in journeys.
#include "data_feed.hpp" // Reference to data feed.

namespace Timetables {
	namespace Structures {
		class trip; // Forward declaration.
		class stop_time; // Forward declaration.

		// Class used in journeys.
		class journey_segment {
		private:
			const trip* trip_; // Trip that serves the journey segment.
			std::vector<stop_time>::const_iterator source_stop_; // Source stop.
			std::vector<stop_time>::const_iterator target_stop_; // Target stop.
			const Timetables::Structures::date_time arrival_; // Arrival at target stop.
		public:
			journey_segment(const trip& trip, const Timetables::Structures::date_time& arrival, const stop& source, const stop& target);

			inline const trip& trip() const { return *trip_; } // Trip.
			inline const stop& source_stop() const { return source_stop_->stop(); } // Source stop.
			inline const stop& target_stop() const { return target_stop_->stop(); } // Target stop.
			inline const date_time departure_from_source() const { return arrival_.add_seconds((-1) * (target_stop_->arrival() - source_stop_->departure())); } // Gets departure from source stop.
			inline const date_time& arrival_at_target() const { return arrival_; } // Gets arrival at target.
			const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const; // Gets intermediate stops between source and target stop.
		};

		// Class collecting information about one journey.
		class journey {
		private:
			std::vector<journey_segment> journey_segments_; // Journey consists of multiple trips, we will store them here.
			std::vector<std::size_t> transfers_; // There is a footpath between every two journey segments.
		public:
			journey(const journey_segment& js) { add_to_journey(js); }

			inline const date_time departure_time() const { return journey_segments_.cbegin()->departure_from_source(); } // Departure time from source stop.
			inline const date_time& arrival_time() const { return (journey_segments_.cend() - 1)->arrival_at_target().add_seconds(*(transfers_.cend() - 1)); } // Arrival time at target stop.
			inline const int duration() const { return date_time::difference(arrival_time(), departure_time()); } // Total duration of the journey.

			inline const std::vector<journey_segment>& journey_segments() const { return journey_segments_; } // Gets journey segments.
			inline const std::vector<std::size_t>& transfers() const { return transfers_; } // Gets transfer. There is a footpath between every two journey segments.

			inline void add_to_journey(const journey_segment& js) { journey_segments_.push_back(js); transfers_.push_back(0); } // Adds journey segment to the journey.
			inline void set_last_transfer(std::size_t duration) { *(transfers_.end() - 1) = duration; } // Sets last transfer.
		};
	}

	namespace Algorithms {
		// Class ensuring functionality of the main algorithm.
		class router {
		private:
			const Timetables::Structures::station& source_; // Source station defined by the user..
			const Timetables::Structures::station& target_; // Target station defined by the user.
			const Timetables::Structures::date_time earliest_departure_; // Earliest departure defined by the user.
			const std::size_t max_transfers_; // Maximum number of transfers defined by the user.
			const std::size_t count_; // Count of journeys to search defined by the user.
			
			std::vector<std::unordered_map<const Timetables::Structures::stop*, Timetables::Structures::date_time>> labels_; // The best arrival date time at a stop in k-th round. TO-DO: Move to journeys.
			std::vector<std::unordered_map<const Timetables::Structures::stop*, Timetables::Structures::journey>> journeys_; // The best journey we can get from source stop to a stop using k transfers.
			std::unordered_map<const Timetables::Structures::stop*, Timetables::Structures::date_time> temp_labels_; // The best time we can get to a stop.
			std::unordered_set<const Timetables::Structures::stop*> marked_stops_; // Stops to be processed by the algorithm.
			std::unordered_map<const Timetables::Structures::route*, const Timetables::Structures::stop*> active_routes_; // Routes that will be traversed in current round.

			std::multimap<Timetables::Structures::date_time, const Timetables::Structures::journey> fastest_journeys_; // Fastest journeys found by the router, key is the arrival time to the target station. That means, they are sorted.

			void accumulate_routes(); // Accumulates routes that will be traversed in next method.
			void traverse_each_route(); // Traverses each route.
			void look_at_footpaths(); // Tries to improve journeys using footpaths.

			const Timetables::Structures::journey* obtain_journey(const Timetables::Structures::date_time& departure); // Returns the best journey obtained in this iteration.

			void traverse_route(const Timetables::Structures::route& current_route, const Timetables::Structures::stop& starting_stop); // Traverses one route.
			std::pair<const Timetables::Structures::trip*, Timetables::Structures::date_time> // Returns pointer to the trip and starting date of the trip.
				find_earliest_trip(const Timetables::Structures::route& route, const Timetables::Structures::date_time& arrival, std::size_t stop_index); // Finds the earliest trip that can be caught in given stop. 
		public:
			router(const Timetables::Structures::data_feed& feed, const std::size_t source_id, const std::size_t target_id, const Timetables::Structures::date_time& earliest_departure, const std::size_t count, const std::size_t transfers) :
				max_transfers_(transfers + 1), count_(count), earliest_departure_(earliest_departure), source_(feed.stations().at(source_id)), target_(feed.stations().at(target_id)) {}

			void obtain_journeys(); // Obtains given count of the best journeys.

			const std::multimap<Timetables::Structures::date_time, const Timetables::Structures::journey>& show_journeys() const { return fastest_journeys_; } // Shows the best journeys found by the algorithm.
		};
	}
}

#endif // !ROUTER_HPP