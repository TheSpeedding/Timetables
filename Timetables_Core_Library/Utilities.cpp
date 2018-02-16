#include "Utilities.hpp"
#include "Exceptions.hpp"
#include <vector>
#include <string>
#include <ctime>
#include <sstream>
#include <cmath>
#include <math.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Time::Time(const std::string& Time) {

	vector<uint8_t> tokens;
	int hours, minutes;

	if (Time.size() != 8 && Time.size() != 7) throw InvalidDataFormatException("Invalid Time format.");

	// Wrote this style in order to get maximal performance.

	if (Time[1] == ':') hours = Time[0] - '0';
	else if (Time[2] == ':') hours = (Time[0] - '0') * 10 + Time[1] - '0';
	else throw InvalidDataFormatException("Invalid Time format.");

	if (Time[4] == ':') { // x:xx:xx
		minutes = (Time[2] - '0') * 10 + Time[3] - '0';
		seconds = (Time[5] - '0') * 10 + Time[6] - '0';
	}
	else if (Time[5] == ':') { // xx:xx:xx
		minutes = (Time[3] - '0') * 10 + Time[4] - '0';
		seconds = (Time[6] - '0') * 10 + Time[7] - '0';
	}
	else throw InvalidDataFormatException("Invalid Time format.");

	seconds += minutes * 60;
	seconds += hours * 3600;

	// Note: Trips that span multiple dates will have stop times greater than 24:00:00. 
	// For example, if a trip begins at 10:30:00 p.m. and ends at 2:15:00 a.m. on the following day, 
	// the stop times would be 22:30:00 and 26:15:00. 
	// Entering those stop times as 22:30:00 and 02:15:00 would not produce the desired results.

}

Timetables::Structures::Time Timetables::Structures::Time::Now() {
	time_t t = std::time(0);
	struct tm now; localtime_s(&now, &t);
	return Time(now.tm_hour, now.tm_min, now.tm_sec);
}

Timetables::Structures::Date::Date(const std::string& Date) {
	if (Date.size() != 8) throw InvalidDataFormatException("Invalid Date format.");
	year = (Date[0] - '0') * 1000 + (Date[1] - '0') * 100 + (Date[2] - '0') * 10 + (Date[3] - '0');
	month = (Date[4] - '0') * 10 + (Date[5] - '0');
	day = (Date[6] - '0') * 10 + (Date[7] - '0');
	if (day > 31 || month > 12) throw InvalidDataFormatException("Invalid Date format.");
}

Timetables::Structures::Date Timetables::Structures::Date::Now() {
	time_t t = std::time(0);
	struct tm now; localtime_s(&now, &t);
	return Date(now.tm_mday, now.tm_mon + 1, now.tm_year + 1900);
}

int Timetables::Structures::Date::GetDayInWeek() const {
	// Using methods described here : https://en.wikipedia.org/wiki/Determination_of_the_day_of_the_week
	auto y = year, d = day, m = month;
	return (d += m < 3 ? y-- : y - 2, 23 * m / 9 + d + 4 + y / 4 - y / 100 + y / 400) % 7 - 1;
}

bool Timetables::Structures::Date::operator<(const Date& other) const {
	if (year < other.year)
		return true;
	else if (year == other.year) {
		if (month < other.month)
			return true;
		else if (month == other.month)
			if (day < other.day)
				return true;
	}
	return false;
}

bool Timetables::Structures::Date::operator>(const Date& other) const {
	if (year > other.year)
		return true;
	else if (year == other.year) {
		if (month > other.month)
			return true;
		else if (month == other.month)
			if (day > other.day)
				return true;
	}
	return false;
}

Timetables::Structures::Date& Timetables::Structures::Date::operator++() {
	day++;
	auto days_in_month = GetDaysInMonth(month, year);
	if (day > days_in_month) {
		day -= days_in_month;
		month++;
	}
	if (month > 12) {
		month -= 12;
		year++;
	}
	return *this;
}

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
}

int Timetables::Structures::GpsCoords::GetWalkingTime(const GpsCoords& A, const GpsCoords& B) {
	// Using Haversine formula. 
	double AlatR = Deg2Rad(A.latitude);
	double AlonR = Deg2Rad(A.longitude);
	double BlatR = Deg2Rad(B.latitude);
	double BlonR = Deg2Rad(B.longitude);
	double u = sin((BlatR - AlatR) / 2);
	double v = sin((BlonR - AlonR) / 2);
	return int(2.0 * 6371.0 * asin(sqrt(u * u + cos(AlatR) * cos(BlatR) * v * v)) * 1000 * 2);
}
