#ifndef STOP_TIME_HPP
#define STOP_TIME_HPP

#include "Trips.hpp"
#include "Utilities.hpp"
#include "Stops.hpp"

namespace Timetables {
	namespace Structures {
		class Trip;
		class Stop;
		class StopTime {
		private:
			const Trip& trip;
			const Stop& stop;
			DateTime arrival;
			DateTime departure;
		public:
			StopTime(const Trip& trip, const Stop& stop, const DateTime& arrival, const DateTime& departure) :
				trip(trip), stop(stop), arrival(arrival), departure(departure) {}

			inline const Trip& Trip() const { return trip; }
			inline const Stop& Stop() const { return stop; }
			inline const DateTime& Arrival() const { return arrival; }
			inline const DateTime& Departure() const { return departure; }

			bool IsOperatingInDate(const DateTime& dateTime) const;
			DateTime AbsoluteDepartureTime(const DateTime& dateTime) const;
			DateTime AbsoluteArrivalTime(const DateTime& dateTime) const;
			DateTime StartingDateForTrip(const DateTime& dateTime) const;
		};
	}
}

#endif // !STOP_TIME_HPP
