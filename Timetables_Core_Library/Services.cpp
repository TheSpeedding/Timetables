#include "Services.hpp"
#include <string>
#include <iostream>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::Service::Service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const DateTime& start, const DateTime& end) :
	validSince(start), validUntil(end) {
	operatingDays[0] = mon;
	operatingDays[1] = tue;
	operatingDays[2] = wed;
	operatingDays[3] = thu;
	operatingDays[4] = fri;
	operatingDays[5] = sat;
	operatingDays[6] = sun;
}

bool Timetables::Structures::Service::IsAddedInDate(const DateTime& dateTime) const {
	auto entry = exceptions.find(dateTime);
	if (entry == exceptions.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this Date.
	else if (entry->second == true) // Found. Service IS operating in this Date. Returns true.
		return true;
	return false;
}

bool Timetables::Structures::Service::IsRemovedInDate(const DateTime& dateTime) const {
	auto entry = exceptions.find(dateTime);
	if (entry == exceptions.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this Date.
	else if (entry->second == false) // Found. Service IS NOT operating in this Date. Returns true.
		return true;
	return false;
}

bool Timetables::Structures::Service::IsOperatingInDate(const DateTime& dateTime) const {
	if (dateTime < validSince || dateTime > validUntil) return false;
	if (IsAddedInDate(dateTime)) return true;
	if (IsRemovedInDate(dateTime)) return false;
	size_t dayInWeek = dateTime.DayInWeek();
	return IsOperatingOnDay(dayInWeek);
}

Timetables::Structures::Services::Services(std::istream&& calendar, std::istream&& calendarDates) {

	string token;
	std::getline(calendar, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<string, 10> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: ServiceID, Monday ... Sunday, ValidSince, ValidUntil

		for (size_t i = 0; i < 10; i++)
			std::getline(calendar, tokens[i], ';');

		Service s(tokens[1] == "1" ? true : false, tokens[2] == "1" ? true : false, tokens[3] == "1" ? true : false,
			tokens[4] == "1" ? true : false, tokens[5] == "1" ? true : false, tokens[6] == "1" ? true : false,
			tokens[7] == "1" ? true : false, DateTime(tokens[8]), DateTime(tokens[9]));

		list.push_back(move(s));

	}

	// Let's process the file with extraordinary events.

	std::getline(calendarDates, token); // Number of entries.

	size = stoi(token);
	
	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: ServiceID, Date, TypeOfExtraordinaryEvent

		for (size_t i = 0; i < 3; i++)
			std::getline(calendarDates, tokens[i], ';');

		size_t id(stoi(tokens[0]));

		Service& s = list[id];

		bool type = tokens[2] == "1" ? true : false;

		s.AddExtraordinaryEvent(DateTime(tokens[1]), type);
	}

}
