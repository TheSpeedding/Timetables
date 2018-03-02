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

Timetables::Structures::DateTime::Date::Date(std::size_t day, std::size_t month, std::size_t year) {
	struct tm date;
	date.tm_mday = day;
	date.tm_mon = month;
	date.tm_year = year;
	seconds = mktime(&date);
}

Timetables::Structures::DateTime::DateTime(const std::string& input) {

	// Accepts date format in YYYYMMDD and time format in HH:MM:SS.
	// For maximal performance, no format validity check.


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
	size_t hours = now.tm_hour;
	size_t minutes = now.tm_min;
	size_t seconds = now.tm_sec;

}

std::size_t Timetables::Structures::DateTime::Day() const {
	return localtime(&date)->tm_mday;
}

std::size_t Timetables::Structures::DateTime::Month() const {
	return localtime(&date)->tm_mon;
}

std::size_t Timetables::Structures::DateTime::Year() const {
	return localtime(&date)->tm_year;
}

std::size_t Timetables::Structures::DateTime::DayInWeek() const {
	return localtime(&date)->tm_wday;
}