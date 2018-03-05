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

		dateTime = stoi(input.substr(0, 2)) * 3600 + stoi(input.substr(3, 2)) * 60 + stoi(input.substr(6, 2));

	}


	else {

		// Date parsing.

		struct tm d;

		d.tm_mon = stoi(input.substr(4, 2)) - 1;
		d.tm_mday = stoi(input.substr(6, 2));
		d.tm_year = stoi(input.substr(0, 4)) - 1900;
		d.tm_hour = 0; d.tm_min = 0; d.tm_sec = 0;

		dateTime = _mkgmtime(&d);

	}

}

Timetables::Structures::DateTime::DateTime(std::size_t hours, std::size_t mins, std::size_t secs, std::size_t day, std::size_t month, std::size_t year) {
	struct tm d;
	d.tm_mon = month - 1;
	d.tm_mday = day;
	d.tm_year = year - 1900;
	d.tm_hour = hours;
	d.tm_min = mins; 
	d.tm_sec = secs;

	dateTime = _mkgmtime(&d);
}

DateTime Timetables::Structures::DateTime::Now() {
	time_t dateTime = std::time(0);
	struct tm now; gmtime_s(&now, &dateTime);
	return DateTime(dateTime);
}

std::size_t Timetables::Structures::DateTime::Day() const {
	return gmtime(&dateTime)->tm_mday;
}

std::size_t Timetables::Structures::DateTime::Month() const {
	return 1 + gmtime(&dateTime)->tm_mon;
}

std::size_t Timetables::Structures::DateTime::Year() const {
	return 1900 + gmtime(&dateTime)->tm_year;
}
