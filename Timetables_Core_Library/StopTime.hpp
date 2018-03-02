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

			inline bool IsOperatingInDate(const DateTime& dateTime) const { return trip.Service().IsOperatingInDate(dateTime.Date().AddDays((-1) * (departure.Time().TotalSeconds() / 86400))); }
		};
	}
}

#endif // !STOP_TIME_HPP
