#ifndef ROUTER_RAPTOR_HPP
#define ROUTER_RAPTOR_HPP

#define BENCHMARK

#include <memory> // Polymorphism.
#include <vector> // Used in journeys.
#include <map> // Structure for algorithm
#include <set> // Structure for algorithm
#include "../Structures/routes_info.hpp" // Mean of transport.
#include "../Structures/journey.hpp" // Journey.
#include "../Structures/data_feed.hpp" // Reference to data feed.
#include <tuple> // Return type of earliest trip function.
#include "../Algorithms//router_base.hpp" // Inheritance.

namespace Timetables {
	namespace Algorithms {
		// Class ensuring functionality of the main algorithm.
		class router_raptor : public router_base {
		private:
			const Timetables::Structures::station& source_; // Source station defined by the user.
			const Timetables::Structures::station& target_; // Target station defined by the user.
			const Timetables::Structures::date_time earliest_departure_; // Earliest departure defined by the user.
			const std::size_t max_transfers_; // Maximum number of transfers defined by the user.
			const std::size_t count_; // Count of journeys to search defined by the user.
			const Timetables::Structures::date_time maximal_arrival_; // Maximal arrival defined by the user.
			
			const double transfers_coefficient_; // Coefficient by which the duration of transfers are multiplied.
			const Timetables::Structures::mean_of_transport mot_; // Means of transport that can be used.

			const bool search_by_arrival_; // Indicates whether the algorithm should search by maximal arrival. Otherwise by count.
			
			std::vector<std::map<const Timetables::Structures::stop*, std::shared_ptr<Timetables::Structures::journey_segment>>> journeys_; // The best journey we can get from source stop to a stop using k transfers.
			std::map<const Timetables::Structures::stop*, Timetables::Structures::date_time> temp_labels_; // The best time we can get to a stop.
			std::set<const Timetables::Structures::stop*> marked_stops_; // Stops to be processed by the algorithm.
			std::map<const Timetables::Structures::route*, const Timetables::Structures::stop*> active_routes_; // Routes that will be traversed in current round.

			std::set<Timetables::Structures::journey> fastest_journeys_; // Fastest journeys found by the router, sorted.

			void accumulate_routes(); // Accumulates routes that will be traversed in next method.
			void traverse_each_route(); // Traverses each route.
			void look_at_footpaths(); // Tries to improve journeys using footpaths.

			const Timetables::Structures::journey* obtain_journey(const Timetables::Structures::date_time& departure); // Returns the best journey obtained in this iteration.

			void traverse_route(const Timetables::Structures::route& current_route, const Timetables::Structures::stop& starting_stop); // Traverses one route.
			std::tuple<const Timetables::Structures::trip*, Timetables::Structures::date_time, Timetables::Structures::service_state> // Returns pointer to the trip and starting date of the trip.
				find_earliest_trip(const Timetables::Structures::route& route, const Timetables::Structures::date_time& arrival, std::size_t stop_index); // Finds the earliest trip that can be caught in given stop. 
		
#ifdef BENCHMARK
			std::size_t total_marked_stops_ = 0;
			std::size_t total_traversed_routes_ = 0;
			std::size_t total_et_calls_ = 0;
			std::size_t total_rounds_ = 0;
#endif
		public:
			router_raptor(const Timetables::Structures::data_feed& feed, const std::size_t source_id, const std::size_t target_id, const Timetables::Structures::date_time& earliest_departure, const std::size_t count, const std::size_t transfers, double coef = 1, Timetables::Structures::mean_of_transport mot = static_cast<Timetables::Structures::mean_of_transport>(255)) :
				max_transfers_(transfers + 1), count_(count), earliest_departure_(earliest_departure), source_(feed.stations().at(source_id)),
				target_(feed.stations().at(target_id)), transfers_coefficient_(coef), mot_(mot), search_by_arrival_(false), maximal_arrival_(0) {}

			router_raptor(const Timetables::Structures::data_feed& feed, const std::size_t source_id, const std::size_t target_id, const Timetables::Structures::date_time& earliest_departure, const Timetables::Structures::date_time& maximal_arrival, const std::size_t transfers, double coef = 1, Timetables::Structures::mean_of_transport mot = static_cast<Timetables::Structures::mean_of_transport>(255)) :
				max_transfers_(transfers + 1), count_(0), earliest_departure_(earliest_departure), source_(feed.stations().at(source_id)),
				target_(feed.stations().at(target_id)), transfers_coefficient_(coef), mot_(mot), search_by_arrival_(true), maximal_arrival_(maximal_arrival) {}

			virtual void obtain_journeys() override; // Obtains given count of the best journeys.

			virtual const std::set<Timetables::Structures::journey>& show_journeys() const override { return fastest_journeys_; } // Shows the best journeys found by the algorithm.

#ifdef BENCHMARK
			std::size_t total_marked_stops() const { return total_marked_stops_; }
			std::size_t total_traversed_routes() const { return total_traversed_routes_; }
			std::size_t total_et_calls() const { return total_et_calls_; }
			std::size_t total_rounds() const { return total_rounds_; }
#endif
		};
	}
}

#endif // !ROUTER_RAPTOR_HPP