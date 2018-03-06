#include "StopTime.hpp"

using namespace std;
using namespace Timetables::Structures;

const int Timetables::Structures::StopTime::ArrivalSinceTripBeginning() const { 
	return arrival + trip.Departure(); 
}

const int Timetables::Structures::StopTime::DepartureSinceTripBeginning() const {
	return departure + trip.Departure();
}

bool Timetables::Structures::StopTime::IsOperatingInDate(const DateTime& dateTime) const {
	return trip.Service().IsOperatingInDate(dateTime.AddDays((-1) * ((departure + trip.Departure()) / 86400)));
}

DateTime Timetables::Structures::StopTime::StartingDateForTrip(const DateTime& dateTime) const {
	return dateTime.AddDays((-1) * ((departure + trip.Departure()) / 86400)).Date();
}
