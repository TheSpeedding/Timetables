#include "StopTime.hpp"

using namespace std;
using namespace Timetables::Structures;

bool Timetables::Structures::StopTime::IsOperatingInDate(const DateTime& dateTime) const {
	return trip.Service().IsOperatingInDate(dateTime.AddDays((-1) * (departure.TotalSecondsSinceMidnight() / 86400)));
}

DateTime Timetables::Structures::StopTime::AbsoluteDepartureTime(const DateTime& dateTime) const {
	return DateTime(departure.TotalSecondsSinceMidnight() + dateTime.AddDays((-1) * (departure.TotalSecondsSinceMidnight() / 86400)).TotalSecondsSinceEpochUntilMidnight());
}

DateTime Timetables::Structures::StopTime::AbsoluteArrivalTime(const DateTime& dateTime) const {
	return DateTime(arrival.TotalSecondsSinceMidnight() + dateTime.AddDays((-1) * (departure.TotalSecondsSinceMidnight() / 86400)).TotalSecondsSinceEpochUntilMidnight());
}

DateTime Timetables::Structures::StopTime::StartingDateForTrip(const DateTime& dateTime) const {
	return DateTime(0, dateTime.AddDays((-1) * (departure.TotalSecondsSinceMidnight() / 86400)).TotalSecondsSinceEpochUntilMidnight());
}
