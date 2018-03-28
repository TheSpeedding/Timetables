#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <memory> // Polymorphism.
#include <vector> // Used in journeys.
#include <unordered_map> // Structure for algorithm
#include <unordered_set> // Structure for algorithm
#include <set> // Structure for algorithm
#include "../Structures/journey.hpp" // Journey.
#include "../Structures/data_feed.hpp" // Reference to data feed.

namespace Timetables {
	namespace Algorithms {
		// Class ensuring functionality of the main algorithm.
		class router {
		private:
			const Timetables::Structures::station& source_; // Source station defined by the user..
			const Timetables::Structures::station& target_; // Target station defined by the user.
			const Timetables::Structures::date_time earliest_departure_; // Earliest departure defined by the user.
			const std::size_t max_transfers_; // Maximum number of transfers defined by the user.
			const std::size_t count_; // Count of journeys to search defined by the user.
			
			std::vector<std::unordered_map<const Timetables::Structures::stop*, Timetables::Structures::journey>> journeys_; // The best journey we can get from source stop to a stop using k transfers.
			std::unordered_map<const Timetables::Structures::stop*, Timetables::Structures::date_time> temp_labels_; // The best time we can get to a stop.
			std::unordered_set<const Timetables::Structures::stop*> marked_stops_; // Stops to be processed by the algorithm.
			std::unordered_map<const Timetables::Structures::route*, const Timetables::Structures::stop*> active_routes_; // Routes that will be traversed in current round.

			std::set<Timetables::Structures::journey> fastest_journeys_; // Fastest journeys found by the router, key is the arrival time to the target station. That means, they are sorted.

			void accumulate_routes(); // Accumulates routes that will be traversed in next method.
			void traverse_each_route(); // Traverses each route.
			void look_at_footpaths(); // Tries to improve journeys using footpaths.

			std::unique_ptr<Timetables::Structures::journey> obtain_journey(const Timetables::Structures::date_time& departure); // Returns the best journey obtained in this iteration.

			void traverse_route(const Timetables::Structures::route& current_route, const Timetables::Structures::stop& starting_stop); // Traverses one route.
			std::pair<const Timetables::Structures::trip*, Timetables::Structures::date_time> // Returns pointer to the trip and starting date of the trip.
				find_earliest_trip(const Timetables::Structures::route& route, const Timetables::Structures::date_time& arrival, std::size_t stop_index); // Finds the earliest trip that can be caught in given stop. 
		public:
			router(const Timetables::Structures::data_feed& feed, const std::size_t source_id, const std::size_t target_id, const Timetables::Structures::date_time& earliest_departure, const std::size_t count, const std::size_t transfers) :
				max_transfers_(transfers + 1), count_(count), earliest_departure_(earliest_departure), source_(feed.stations().at(source_id)), target_(feed.stations().at(target_id)) {}

			void obtain_journeys(); // Obtains given count of the best journeys.

			const std::set<Timetables::Structures::journey>& show_journeys() const { return fastest_journeys_; } // Shows the best journeys found by the algorithm.
		};
	}
}

#endif // !ROUTER_HPP