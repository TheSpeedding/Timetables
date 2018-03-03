#define _CRT_SECURE_NO_WARNINGS

#include "Utilities.hpp"
#include "Exceptions.hpp"
#include <vector>
#include <string>
#include <ctime>
#include <sstream>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::DateTime::DateTime(const std::string& input) {

	// Accepts date format in YYYYMMDD and time format in HH:MM:SS.
	// For maximal performance, no format validity check. 
	// This library operates with data that should be correct, because they were produced by preprocessor (containing format checking).


	if (input[2] == ':' && input[5] == ':') {

		// Time parsing.

		time = stoi(input.substr(0, 2)) * 3600 + stoi(input.substr(3, 2)) * 60 + stoi(input.substr(6, 2));

		date = 0;

	}


	else {

		// Date parsing.

		struct tm d;

		d.tm_mon = stoi(input.substr(4, 2)) - 1;
		d.tm_mday = stoi(input.substr(6, 2));
		d.tm_year = stoi(input.substr(0, 4)) - 1900;
		d.tm_hour = 0; d.tm_min = 0; d.tm_sec = 0;

		date = mktime(&d);

		time = 0;
	}

}

DateTime Timetables::Structures::DateTime::Now() {
	time_t date = std::time(0);
	struct tm now; localtime_s(&now, &date);
	size_t time = now.tm_hour * 3600 + now.tm_min * 60 + now.tm_sec;
	date -= time;
	return DateTime(time, date);
}

long int Timetables::Structures::DateTime::Difference(const DateTime& first, const DateTime& second) {
	DateTime newDate(first);
	newDate.time -= second.time;
	newDate.date -= second.date;
	return newDate.time + newDate.date;
}

std::size_t Timetables::Structures::DateTime::Day() const {
	return localtime(&date)->tm_mday;
}

std::size_t Timetables::Structures::DateTime::Month() const {
	return 1 + localtime(&date)->tm_mon;
}

std::size_t Timetables::Structures::DateTime::Year() const {
	return 1900 + localtime(&date)->tm_year;
}

std::size_t Timetables::Structures::DateTime::DayInWeek() const {
	auto day = (localtime(&date)->tm_wday - 1);
	return day == -1 ? 6 : day;
}

DateTime Timetables::Structures::DateTime::SetDate(const DateTime& dateTime) const {
	DateTime newDate(*this);
	newDate.date = dateTime.date;
	if (time >= 86400) {				
		newDate.date += 86400 * (time / 86400);
		newDate.time %= 86400;
	}
	return newDate;
}
