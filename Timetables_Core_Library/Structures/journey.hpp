#ifndef JOURNEY_HPP
#define JOURNEY_HPP

#include <memory> // Polymorphism.
#include <vector> // Used in journeys.
#include "stop_time.hpp" // Used in journeys.
#include "date_time.hpp" // Datetime.
#include <algorithm> // Reversion of the vector.

namespace Timetables {
	namespace Structures {
		class trip; // Forward declaration.
		class stop_time; // Forward declaration.

		// Class used in journeys.
		class journey_segment {
			friend class journey; // Clone method should not be a part of API. Pointer to the previous segment as well.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() = 0;
			date_time arrival_; // Arrival at target stop.
			std::shared_ptr<journey_segment> previous_; // Previous segment in the sequence.
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
			const Timetables::Structures::trip* trip_; // Trip that serves the trip segment.
			std::vector<stop_time>::const_iterator source_stop_; // Source stop.
			std::vector<stop_time>::const_iterator target_stop_; // Target stop.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() { return std::make_unique<trip_segment>(*this); }
		public:
			trip_segment(const Timetables::Structures::trip& trip, const Timetables::Structures::date_time& arrival, const stop& source, const stop& target, std::shared_ptr<journey_segment> previous) : trip_(&trip) {
				arrival_ = arrival; previous_ = previous;
				for (std::vector<stop_time>::const_iterator it = trip.stop_times().cbegin(); it != trip.stop_times().cend(); ++it) { // Searches for stops in the trip.
					if (&it->stop() == &source) source_stop_ = it;
					if (&it->stop() == &target) target_stop_ = it;
				}
			}

			virtual inline const Timetables::Structures::trip* trip() const override { return trip_; } // Trip.
			virtual inline const stop& source_stop() const override { return source_stop_->stop(); } // Source stop.
			virtual inline const stop& target_stop() const override { return target_stop_->stop(); } // Target stop.
			virtual inline const date_time departure_from_source() const override { return date_time(arrival_, SECOND * (-1) * (target_stop_->arrival() - source_stop_->departure())); } // Gets departure from source stop.
			virtual inline const date_time& arrival_at_target() const override { return arrival_; } // Gets arrival at target.
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const override { // Gets intermediate stops between source and target stop.
				std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> stops;
				std::size_t base = source_stop_->departure();
				for (auto it = source_stop_; it <= target_stop_; ++it)
					stops.push_back(std::make_pair(it->arrival() - base, &it->stop()));
				return move(stops);
			}
		};

		// Transfer.
		class footpath_segment final : public journey_segment {
		private:
			const std::size_t duration_; // Duration of the transfer.
			const stop& source_stop_; // Source stop.
			const stop& target_stop_; // Target stop.
		protected:
			virtual inline std::unique_ptr<journey_segment> clone() { return std::make_unique<footpath_segment>(*this); }
		public:
			footpath_segment(const Timetables::Structures::date_time& arrival, const stop& source, const stop& target, std::size_t duration, std::shared_ptr<journey_segment> previous) :
				duration_(duration), source_stop_(source), target_stop_(target) { arrival_ = arrival; previous_ = previous;	}

			virtual inline const Timetables::Structures::trip* trip() const override { return nullptr; } // Trip. Nullptr because this is a transfer.
			virtual inline const stop& source_stop() const override { return source_stop_; } // Source stop.
			virtual inline const stop& target_stop() const override { return target_stop_; } // Target stop.
			virtual inline const date_time departure_from_source() const override { return date_time(arrival_, SECOND * (-1) * duration_); } // Gets departure from source stop.
			virtual inline const date_time& arrival_at_target() const override { return arrival_; } // Gets arrival at target.
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const override { return std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>>(); } // Gets intermediate stops between source and target stop.
		};

		// Class collecting information about one journey.
		class journey {
		private:
			std::vector<std::shared_ptr<journey_segment>> journey_segments_; // Journey consists of multiple segments, we will store them here.

			void clone(const journey& other) {
				for (auto&& segment : other.journey_segments_)
					journey_segments_.push_back(segment->clone());
			}

			bool add_to_journey(std::shared_ptr<journey_segment> js); // Adds trip or footpath segment to the journey.


		public:
			journey(std::shared_ptr<journey_segment> js);
						
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

			bool operator< (const journey& other) const; // Preferences: Arrival time, duration (=dep. time), number of transfers, number of stops, total duration of transfers,.
		
			inline const std::size_t number_of_stops() const { // Total number of stops in the journey.
				std::size_t number = 1;
				for (auto&& seg : journey_segments_) if (seg->trip() != nullptr) number += seg->intermediate_stops().size() + 1;
				return number;
			}

			inline const std::size_t duration_of_transfers() const { // Duration of all the footpaths in the journey.
				std::size_t number = 0;
				for (auto&& seg : journey_segments_) if (seg->trip() == nullptr) number += date_time::difference(seg->arrival_at_target(), seg->departure_from_source());
				return number;
			}

			inline const std::vector<std::shared_ptr<journey_segment>>& journey_segments() const { return journey_segments_; } // Gets journey segments.			
		};
	}
}

#endif //!JOURNEY_HPP