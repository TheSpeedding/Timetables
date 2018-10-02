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
			friend class journey; // Pointer to the previous segment should not be a part of API.
		protected:
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
			virtual inline bool outdated() const = 0;
			virtual std::shared_ptr<journey_segment> find_later_departure(const Timetables::Structures::date_time& latest_arrival) const = 0;
			inline std::size_t duration() const { return date_time::difference(arrival_at_target(), departure_from_source()); }

			inline bool operator== (const journey_segment& other) const { 
				return trip() == other.trip() && &source_stop() == &other.source_stop() && &target_stop() == &other.target_stop() &&
					departure_from_source().timestamp() == other.departure_from_source().timestamp() && arrival_at_target().timestamp() == other.arrival_at_target().timestamp();
			}
			inline bool operator!= (const journey_segment& other) const {
				return !(*this == other);
			}
		};

		// Trip segment.
		class trip_segment final : public journey_segment {
		private:
			const Timetables::Structures::trip* trip_; // Trip that serves the trip segment.
			std::vector<stop_time>::const_iterator source_stop_; // Source stop.
			std::vector<stop_time>::const_iterator target_stop_; // Target stop.
			bool outdated_; // Flag that says whether the journey uses outdated timetables.
		public:
			trip_segment(const Timetables::Structures::trip& trip, const Timetables::Structures::date_time& arrival, const stop& source, 
				const stop& target, std::shared_ptr<journey_segment> previous, bool outdated) : trip_(&trip), outdated_(outdated) {
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
				for (auto it = source_stop_ + 1; it < target_stop_; ++it)
					stops.push_back(std::make_pair(it->arrival() - base, &it->stop()));
				return std::move(stops);
			}
			virtual inline bool outdated() const override { return outdated_; }
			virtual std::shared_ptr<journey_segment> find_later_departure(const Timetables::Structures::date_time& latest_arrival) const override;
		};

		// Transfer.
		class footpath_segment final : public journey_segment {
		private:
			const int duration_; // Duration of the transfer.
			const stop& source_stop_; // Source stop.
			const stop& target_stop_; // Target stop.
		public:
			footpath_segment(const Timetables::Structures::date_time& arrival, const stop& source, const stop& target, int duration, std::shared_ptr<journey_segment> previous) :
				duration_(duration), source_stop_(source), target_stop_(target) { arrival_ = arrival; previous_ = previous;	}

			virtual inline const Timetables::Structures::trip* trip() const override { return nullptr; } // Trip. Nullptr because this is a transfer.
			virtual inline const stop& source_stop() const override { return source_stop_; } // Source stop.
			virtual inline const stop& target_stop() const override { return target_stop_; } // Target stop.
			virtual inline const date_time departure_from_source() const override { return date_time(arrival_, (-1) * duration_); } // Gets departure from source stop.
			virtual inline const date_time& arrival_at_target() const override { return arrival_; } // Gets arrival at target.
			virtual const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> intermediate_stops() const override { return std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>>(); } // Gets intermediate stops between source and target stop.
			virtual inline bool outdated() const override { return false; }
			virtual std::shared_ptr<journey_segment> find_later_departure(const Timetables::Structures::date_time& latest_arrival) const override {
				return std::make_shared<footpath_segment>(latest_arrival, source_stop_, target_stop_, duration_, previous_);
			}
		};

		// Class collecting information about one journey.
		class journey {
		private:
			std::vector<std::shared_ptr<journey_segment>> journey_segments_; // Journey consists of multiple segments, we will store them here.	
		public:
			journey(std::shared_ptr<journey_segment> js);						

			inline const date_time departure_time() const { return journey_segments_.cbegin()->get()->departure_from_source(); } // Departure time from source stop.
			inline const date_time& arrival_time() const { return (journey_segments_.cend() - 1)->get()->arrival_at_target(); } // Arrival time at target stop.
			inline const std::time_t duration() const { return date_time::difference(arrival_time(), departure_time()); } // Total duration of the journey.
			inline const std::time_t duration_without_waiting_times() const { std::time_t t = 0; for (auto&& js : journey_segments_) t += js->duration(); return t; } // Total duration of the journey without transfers.
			bool contains_redundant_footpath() const;

			bool operator< (const journey& other) const; // Preferences: Arrival time, duration, number of transfers, number of stops, total duration of transfers,.
		
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

			inline bool outdated() const { for (auto&& js : journey_segments_) if (js->outdated()) return true; return false; }
		};
	}
}

#endif //!JOURNEY_HPP