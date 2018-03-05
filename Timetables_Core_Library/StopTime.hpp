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
			int arrival;
			int departure;
		public:
			StopTime(const Trip& trip, const Stop& stop, int arrival, int departure) :
				trip(trip), stop(stop), arrival(arrival), departure(departure) {}

			inline const Trip& Trip() const { return trip; }
			inline const Stop& Stop() const { return stop; }
			inline const int Arrival() const { return arrival; }
			inline const int Departure() const { return departure; }

			const int ArrivalSinceTripBeginning() const;
			const int DepartureSinceTripBeginning() const;

			bool IsOperatingInDate(const DateTime& dateTime) const;
			DateTime StartingDateForTrip(const DateTime& dateTime) const;
		};
	}
}

#endif // !STOP_TIME_HPP
