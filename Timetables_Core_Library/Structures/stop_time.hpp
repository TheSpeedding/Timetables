#ifndef STOP_TIME_HPP
#define STOP_TIME_HPP

#include "../Structures/date_time.hpp" // Extension methods for datetime.
#include "../Structures/trips.hpp" // Reference to the trip in stop time class.
#include "../Structures/stops.hpp" // Reference to the stop in stop time class.

namespace Timetables {
	namespace Structures {
		// Class collecting information about one stop time.
		class stop_time {
		private:
			const trip& trip_; // Reference to the trip belonging to the stop time.
			const stop& stop_; // Reference to the stop belonging to the stop time.
			int arrival_; // Arrival to the stop, relative time (in seconds) since trip departure.
			int departure_; // Departure from the stop, relative time (in seconds) since trip departure.
		public:
			stop_time(const trip& trip, const stop& stop, int arrival, int departure) :
				trip_(trip), stop_(stop), arrival_(arrival), departure_(departure) {}

			inline const trip& trip() const { return trip_; } // Trip belonging to the stop time.
			inline const stop& stop() const { return stop_; } // Stop belonging to the stop time.
			inline int arrival() const { return arrival_; } // Arrival to this stop, seconds since trip beginning.
			inline int departure() const { return departure_; } // Departure from this stop, seconds since trip beginning.

			inline int arrival_since_midnight() const { return arrival_ + trip_.departure(); } // Absolute time, arrival in seconds since midnight.
			inline int departure_since_midnight() const { return departure_ + trip_.departure(); } // Absolute time, departure in seconds since midnight.

			inline bool is_operating_in_date_time(const date_time& date_time) const { return trip_.service().is_operating_in_date(date_time.add_seconds((-1) * departure_since_midnight())); } // Checks whether trip is operating in this datetime.

			inline bool operator< (const stop_time& other) const { return departure_since_midnight() < other.departure_since_midnight(); }
			inline bool operator> (const stop_time& other) const { return departure_since_midnight() > other.departure_since_midnight(); }
			inline bool operator==(const stop_time& other) const { return departure_since_midnight() == other.departure_since_midnight(); }
		};
	}
}

#endif // !STOP_TIME_HPP