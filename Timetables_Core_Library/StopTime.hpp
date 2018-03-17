#ifndef STOP_TIME_HPP
#define STOP_TIME_HPP

#include "Utilities.hpp" // Extension methods for datetime.
#include "Trips.hpp" // Reference to the trip in stop time class.
#include "Stops.hpp" // Reference to the stop in stop time class.

namespace Timetables {
	namespace Structures {
		// Class collecting information about one stop time.
		class StopTime {
		private:
			const Trip& trip; // Reference to the trip belonging to the stop time.
			const Stop& stop; // Reference to the stop belonging to the stop time.
			int arrival; // Arrival to the stop, relative time (in seconds) since trip departure.
			int departure; // Departure from the stop, relative time (in seconds) since trip departure.
		public:
			StopTime(const Trip& trip, const Stop& stop, int arrival, int departure) :
				trip(trip), stop(stop), arrival(arrival), departure(departure) {}

			inline const Trip& Trip() const { return trip; } // Trip belonging to the stop time.
			inline const Stop& Stop() const { return stop; } // Stop belonging to the stop time.
			inline int Arrival() const { return arrival; } // Arrival to this stop, seconds since trip beginning.
			inline int Departure() const { return departure; } // Departure from this stop, seconds since trip beginning.

			inline int ArrivalSinceMidnight() const { return arrival + trip.Departure(); } // Absolute time, arrival in seconds since midnight.
			inline int DepartureSinceTripMidnight() const { return departure + trip.Departure(); } // Absolute time, departure in seconds since midnight.

			inline bool IsOperatingInDateTime(const DateTime& dateTime) const { return trip.Service().IsOperatingInDate(dateTime.AddSeconds((-1) * DepartureSinceTripMidnight())); } // Checks whether trip is operating in this datetime.
		};
	}
}

#endif // !STOP_TIME_HPP