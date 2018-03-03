#include "StopTime.hpp"

bool Timetables::Structures::StopTime::IsOperatingInDate(const DateTime & dateTime) const {
	return trip.Service().IsOperatingInDate(dateTime.AddDays((-1) * (departure.TotalSecondsSinceMidnight() / 86400))); 
}
