#ifndef ROUTER_HPP
#define ROUTER_HPP

#include <memory> // Polymorphism.
#include <vector> // Used in journeys.
#include <unordered_map> // Structure for algorithm
#include <unordered_set> // Structure for algorithm
#include <set> // Structure for algorithm
#include "../Structures/stop_time.hpp" // Used in journeys.
#include "../Structures/data_feed.hpp" // Reference to data feed.

namespace Timetables {
	namespace Structures {
		class trip; // Forward declaration.
		class stop_time; // Forward declaration.

		// Class used in journeys.
		class journey_segment {
			friend class journey; // Clone method should not be a part of API.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() = 0;
		public:
			virtual ~journey_segment() noexcept {}
			virtual inline const trip* trip() const = 0;
			virtual inline const stop& source_stop() const = 0;
			virtual inline const stop& target_stop() const = 0;
			virtual inline const date_time departure_from_source() const = 0;
			virtual inline const date_time& arrival_at_target() const = 0;
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const = 0;
		};

		// Trip segment.
		class trip_segment final : public journey_segment {
		private:
			const date_time arrival_; // Arrival at target stop.
			const Timetables::Structures::trip* trip_; // Trip that serves the trip segment.
			std::vector<stop_time>::const_iterator source_stop_; // Source stop.
			std::vector<stop_time>::const_iterator target_stop_; // Target stop.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() { return std::make_unique<trip_segment>(*this); }
		public:
			trip_segment(const Timetables::Structures::trip& trip, const Timetables::Structures::date_time& arrival, const stop& source, const stop& target) :
				trip_(&trip), arrival_(arrival) {
				for (std::vector<stop_time>::const_iterator it = trip.stop_times().cbegin(); it != trip.stop_times().cend(); ++it) { // Searches for stops in the trip.
					if (&it->stop() == &source) source_stop_ = it;
					if (&it->stop() == &target) target_stop_ = it;
				}
			}

			virtual inline const Timetables::Structures::trip* trip() const override { return trip_; } // Trip.
			virtual inline const stop& source_stop() const override { return source_stop_->stop(); } // Source stop.
			virtual inline const stop& target_stop() const override { return target_stop_->stop(); } // Target stop.
			virtual inline const date_time departure_from_source() const override { return arrival_.add_seconds((-1) * (target_stop_->arrival() - source_stop_->departure())); } // Gets departure from source stop.
			virtual inline const date_time& arrival_at_target() const override { return arrival_; } // Gets arrival at target.
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const override; // Gets intermediate stops between source and target stop.
			
		};

		// Transfer.
		class footpath_segment final : public journey_segment {
		private:
			const std::size_t duration_; // Duration of the transfer.
			const date_time arrival_; // Arrival at target stop.
			const stop& source_stop_; // Source stop.
			const stop& target_stop_; // Target stop.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() { return std::make_unique<footpath_segment>(*this); }
		public:
			footpath_segment(const Timetables::Structures::date_time& arrival, const stop& source, const stop& target, std::size_t duration) :
				duration_(duration), arrival_(arrival), source_stop_(source), target_stop_(target) {}

			virtual inline const Timetables::Structures::trip* trip() const override { return nullptr; } // Trip. Nullptr because this is a transfer.
			virtual inline const stop& source_stop() const override { return source_stop_; } // Source stop.
			virtual inline const stop& target_stop() const override { return target_stop_; } // Target stop.
			virtual inline const date_time departure_from_source() const override { return arrival_.add_seconds((-1) * duration_); } // Gets departure from source stop.
			virtual inline const date_time& arrival_at_target() const override { return arrival_; } // Gets arrival at target.
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const override { return std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>>(); } // Gets intermediate stops between source and target stop.
		};

		// Class collecting information about one journey.
		class journey {
		private:
			std::vector<std::unique_ptr<journey_segment>> journey_segments_; // Journey consists of multiple segments, we will store them here.

			void clone(const journey& other) {
				for (auto&& segment : other.journey_segments_)
					journey_segments_.push_back(segment->clone());
			}
		public:
			journey(const trip_segment& js) { add_to_journey(js); }
			journey(const footpath_segment& js) { add_to_journey(js); }
			
			journey(const journey& other) { clone(other); }
			journey& operator= (const journey& other) {
				if (&other == this) return *this;
				journey_segments_.clear();
				clone(other);
				return *this;
			}

			inline const date_time departure_time() const { return journey_segments_.cbegin()->get()->departure_from_source(); } // Departure time from source stop.
			inline const date_time& arrival_time() const { return (journey_segments_.cend() - 1)->get()->arrival_at_target(); } // Arrival time at target stop.
			inline const int duration() const { return date_time::difference(arrival_time(), departure_time()); } // Total duration of the journey.
			
			inline bool operator< (const journey& other) const { // Preferences: Arrival time, duration, number of transfers, total duration of transfers, number of stops.
				if (arrival_time() != other.arrival_time())
					return arrival_time() < other.arrival_time();
				else if (departure_time() != other.departure_time())
					return departure_time() < other.departure_time();
				else if (duration_of_transfers() != other.duration_of_transfers())
					return duration_of_transfers() < other.duration_of_transfers();
				else if (number_of_stops() != other.number_of_stops())
					return number_of_stops() < other.number_of_stops();
				else
					return journey_segments_.size() < other.journey_segments_.size();
			}

			inline const std::size_t number_of_stops() const { // Total number of stops in the journey.
				std::size_t number = 1;
				for (auto&& seg : journey_segments_)
					if (seg->trip() != nullptr)
						number += seg->intermediate_stops().size() + 1;
				return number;
			}
			
			inline const std::size_t duration_of_transfers() const { // Duration of all the footpaths in the journey.
				std::size_t number = 0;
				for (auto&& seg : journey_segments_)
					if (seg->trip() == nullptr)
						number += date_time::difference(seg->arrival_at_target(), seg->departure_from_source());
				return number;
			}

			inline const std::vector<std::unique_ptr<journey_segment>>& journey_segments() const { return journey_segments_; } // Gets journey segments.

			template <typename T>
			void add_to_journey(const T& js) { // Adds trip or footpath segment to the journey.
				// static_assert(std::is_base_of<journey_segment, T>::value); // C++17 feature

				if (journey_segments_.size() == 1) { 
					
					if ((journey_segments_.cend() - 1)->get()->departure_from_source() == (journey_segments_.cend() - 1)->get()->arrival_at_target())
						journey_segments_.pop_back(); // The last segment lasts 0 seconds. No point of having it in the vector.

					else if (journey_segments_[0]->trip() == nullptr) { // Change initial departure time if footpath comes first.
						
						// "The problem" is the journey segment is immutable.

						const stop& source = journey_segments_[0]->source_stop();
						const stop& target = journey_segments_[0]->target_stop();
						const size_t duration = date_time::difference(journey_segments_[0]->arrival_at_target(), journey_segments_[0]->departure_from_source());

						journey_segments_.pop_back();

						add_to_journey(footpath_segment(static_cast<const journey_segment&>(js).departure_from_source(), source, target, duration));
					}
				}

				journey_segments_.push_back(std::move(std::make_unique<T>(js))); 
			} 
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