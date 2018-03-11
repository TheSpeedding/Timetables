#define _CRT_SECURE_NO_WARNINGS

#include "Utilities.hpp"
#include <ctime>
#include <sstream>

using namespace std;
using namespace Timetables::Structures;

std::time_t Timetables::Structures::DateTime::ParseDate(const string& input) {
	struct tm d;

	d.tm_mon = stoi(input.substr(4, 2)) - 1;
	d.tm_mday = stoi(input.substr(6, 2));
	d.tm_year = stoi(input.substr(0, 4)) - 1900;
	d.tm_hour = 0; d.tm_min = 0; d.tm_sec = 0;

	return _mkgmtime(&d);
}

std::size_t Timetables::Structures::DateTime::Day(std::time_t dt) {
	return gmtime(&dt)->tm_mday;
}

std::size_t Timetables::Structures::DateTime::Month(std::time_t dt) {
	return 1 + gmtime(&dt)->tm_mon;
}

std::size_t Timetables::Structures::DateTime::Year(std::time_t dt) {
	return 1900 + gmtime(&dt)->tm_year;
}