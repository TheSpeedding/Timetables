#ifndef DEPARTURE_BOARD_HPP
#define DEPARTURE_BOARD_HPP

#include "../Structures/stop_time.hpp" // Iterator for stop times.
#include "../Structures/data_feed.hpp" // Reference to data feed.

namespace Timetables {
	namespace Structures {
		// We could use stop_time class, but we will define this one instead to have it more developer friendly.
		class departure {
		private:
			date_time departure_; // Departure from the station.
			bool outdated_; // Indicates whether timetables are outdated.
			std::vector<stop_time>::const_iterator trip_begin_; // Start of the trip from departure stop.
			std::vector<stop_time>::const_iterator trip_end_; // End of the trip at the end stop.
		public:
			departure(const stop_time& stop_time, const date_time& departure, bool outdated);

			inline const date_time& departure_time() const { return departure_; } // Returns departure date time from the stop.
			inline const std::wstring& headsign() const { return trip_begin_->trip().route().headsign(); } // Returns headsign of the trip.
			inline const route_info& line() const { return trip_begin_->trip().route().info(); } // Returns basic information about the line.
			inline const stop& stop() const { return trip_begin_->stop(); } // Returns the stop.
			const std::vector<std::pair<std::size_t, const Timetables::Structures::stop*>> following_stops() const; // Returns following stops in the trip.
			inline bool outdated() const { return outdated_; } // Determines whether departure uses outdated timetables or not.

			inline bool operator< (const departure& other) const { return departure_ < other.departure_; }
			inline bool operator> (const departure& other) const { return departure_ > other.departure_; }
			inline bool operator==(const departure& other) const { return departure_ == other.departure_; }
		};
	}

	namespace Algorithms {	
		// Class collecting information about departure board.
		class departure_board {
		private:
			std::vector<Timetables::Structures::departure> found_departures_; // Departures found by the algorithm.
			std::vector<const Timetables::Structures::stop*> stops_; // Stops of origin.
			const Timetables::Structures::date_time earliest_departure_; // Earliest departure set by the user.
			const std::size_t count_; // Number of departures to show.
			const Timetables::Structures::route_info* route_to_show_; // Nullptr if a user wants to show all the lines.
		public:
			departure_board(const Timetables::Structures::data_feed& feed, const std::size_t station_or_stop_id, const Timetables::Structures::date_time& earliest_departure,
				const size_t count, const std::size_t route_info_id, bool true_if_station) : earliest_departure_(earliest_departure), count_(count), 
				route_to_show_(route_info_id == -1 ? nullptr : &feed.routes_info().at(route_info_id)){
				
				if (true_if_station)
						stops_ = feed.stations().at(station_or_stop_id).child_stops();

				else
					stops_.push_back(&feed.stops().at(station_or_stop_id));

			}

			void obtain_departure_board(); // Gets a departure board.

			inline const std::vector<Timetables::Structures::departure>& show_departure_board() { return found_departures_; } // Shows a departure board.
		};
	}
}

#endif // !DEPARTURE_BOARD_HPP