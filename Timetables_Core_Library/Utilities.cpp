#include "Utilities.hpp"
#include "Exceptions.hpp"
#include <vector>
#include <string>
#include <ctime>
#include <sstream>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;


/*
Timetables::Structures::Date Timetables::Structures::Date::Now() {
	time_t t = std::time(0);
	struct tm now; localtime_s(&now, &t);
	return Date(now.tm_mday, now.tm_mon + 1, now.tm_year + 1900);
}*/

/*
int Timetables::Structures::Date::GetDayInWeek() const {
	// Using methods described here : https://en.wikipedia.org/wiki/Determination_of_the_day_of_the_week
	auto y = year, d = day, m = month;
	return (d += m < 3 ? y-- : y - 2, 23 * m / 9 + d + 4 + y / 4 - y / 100 + y / 400) % 7;
}
*/

/*
int Timetables::Structures::Date::GetDaysInMonth(int month, int year) {
	switch (month) {
	case 1: case 3: case 5: case 7: case 8: case 10: case 12:
		return 31;
	case 4: case 6: case 9: case 11:
		return 30;
	case 2:
		if (year % 4 == 0) {
			if (year % 100 == 0) {
				if (year % 400 == 0)
					return 29;
				else
					return 28;
			}
			else
				return 29;
		}
		else
			return 28;
	}
	return -1;
}*/

Timetables::Structures::DateTime::DateTime(const std::string& input) {

	// Accepts date format in YYYYMMDD and time format in HH:MM:SS.
	// For maximal performance, no format validity checks. Just ensure no memory leaks can occur.

	if (input.size() != 8) throw invalid_argument("Not a valid datetime.");

	if (input[2] == ':' && input[5] == ':') {

		// Time parsing.

		dateTime.tm_hour = stoi(input.substr(0, 2));
		dateTime.tm_min = stoi(input.substr(3, 2));
		dateTime.tm_sec = stoi(input.substr(6, 2));

	}

	else {

		// Date parsing.

		dateTime.tm_year = stoi(input.substr(0, 4));
		dateTime.tm_mon = stoi(input.substr(4, 2)) - 1;
		dateTime.tm_mday = stoi(input.substr(6, 2)) - 1;

	}
}