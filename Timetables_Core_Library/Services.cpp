#include "Services.hpp"
#include <array>
#include <string>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Service::Service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const Date& start, const Date& end) :
	validSince(start), validUntil(end) {
	operatingDays[0] = mon;
	operatingDays[1] = tue;
	operatingDays[2] = wed;
	operatingDays[3] = thu;
	operatingDays[4] = fri;
	operatingDays[5] = sat;
	operatingDays[6] = sun;
}

bool Timetables::Structures::Service::IsAddedInDate(const Date& Date) const {
	auto entry = exceptions.find(Date);
	if (entry == exceptions.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this Date.
	else if (entry->second == true) // Found. Service IS operating in this Date. Returns true.
		return true;
	return false;
}

bool Timetables::Structures::Service::IsRemovedInDate(const Date& Date) const {
	auto entry = exceptions.find(Date);
	if (entry == exceptions.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this Date.
	else if (entry->second == false) // Found. Service IS NOT operating in this Date. Returns true.
		return true;
	return false;
}

bool Timetables::Structures::Service::IsOperatingInDate(const Date& Date) const {
	if (Date < validSince || Date > validUntil) return false;
	if (IsAddedInDate(Date)) return true;
	if (IsRemovedInDate(Date)) return false;
	auto day_in_week = Date.GetDayInWeek();
	return operatingDays[day_in_week - 1];
}

Timetables::Structures::Services::Services(std::istream&& calendar, std::istream&& calendar_dates) {
	string line;
	getline(calendar, line); // The first line is an invalid entry.

	while (calendar.good()) {

		array<string, 10> tokens;
		for (int i = 0; i < 9; i++)
			getline(calendar, tokens[i], ',');
		getline(calendar, tokens[9], '\n');
		if (tokens[0] == "") continue; // Empty line.

		/*
		* tokens[0] = service id
		* tokens[1] = bool value, operating on Monday
		* tokens[2] = bool value, operating on Tuesday
		* tokens[3] = bool value, operating on Wednesday
		* tokens[4] = bool value, operating on Thursday
		* tokens[5] = bool value, operating on Friday
		* tokens[6] = bool value, operating on Saturday
		* tokens[7] = bool value, operating on Sunday
		* tokens[8] = start Date
		* tokens[9] = end Date
		*/

		Service s(tokens[1] == "1" ? true : false, tokens[2] == "1" ? true : false, tokens[3] == "1" ? true : false,
			tokens[4] == "1" ? true : false, tokens[5] == "1" ? true : false, tokens[6] == "1" ? true : false,
			tokens[7] == "1" ? true : false, Date(tokens[8]), Date(tokens[9]));

		list.push_back(move(s));
	}

	// Let's process the file with extraordinary events.

	getline(calendar_dates, line); // The first line is an invalid entry.

	while (calendar_dates.good()) {
		array<string, 3> tokens;
		for (int i = 0; i < 2; i++)
			getline(calendar_dates, tokens[i], ',');
		getline(calendar_dates, tokens[2], '\n');
		if (tokens[0] == "") continue; // Empty line.

		getline(calendar_dates, line); // Move to the next line.

		/*
		* tokens[0] = service id
		* tokens[1] = exception Date
		* tokens[2] = exception type - 1 means added, 2 means removed.
		*/

		size_t id(stoi(tokens[0]));

		if (id >= list.size()) throw ServiceNotFoundException(id);

		Service& s = list[id];

		bool type;
		if (tokens[2] == "1") type = true;
		else if (tokens[2] == "2") type = false;
		else throw InvalidDataFormatException("Unknown type of extrordinary event.");

		s.AddExtraordinaryEvent(Date(tokens[1]), type);
	}
}
