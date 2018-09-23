#include "../Structures/services.hpp"
#include <string>
#include <iostream>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::service::service(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, const date_time& start, const date_time& end) :
	valid_since_(start), valid_until_(end) {
	operating_days_ = (days)0;
	if (mon) operating_days_ = operating_days_ | monday;
	if (tue) operating_days_ = operating_days_ | tuesday;
	if (wed) operating_days_ = operating_days_ | wednesday;
	if (thu) operating_days_ = operating_days_ | thursday;
	if (fri) operating_days_ = operating_days_ | friday;
	if (sat) operating_days_ = operating_days_ | saturday;
	if (sun) operating_days_ = operating_days_ | sunday;
}

bool Timetables::Structures::service::is_added_in_date(const date_time& date) const {
	auto entry = exceptions_.find(date);
	if (entry == exceptions_.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this date.
	else if (entry->second == true) // Found. Service IS operating in this Date. Returns true.
		return true;
	return false;
}

bool Timetables::Structures::service::is_removed_in_date(const date_time& date) const {
	auto entry = exceptions_.find(date);
	if (entry == exceptions_.cend())
		return false; // Not found. It means that the serivce has any extraordinary event in this date.
	else if (entry->second == false) // Found. Service IS NOT operating in this Date. Returns true.
		return true;
	return false;
}

service_state Timetables::Structures::service::is_operating_in_date(const date_time& date) const {

	// Fully optimized for maximal performance.	

	bool operating_on_day_by_default = is_operating_on_day(date.day_in_week());

	if (operating_on_day_by_default) {
		if (is_removed_in_date(date))
			return not_operating;
		else {
			if (date < valid_since_)
				return not_operating;
			else if (date > valid_until_)
				return outdated;
			else 
				return operating;
		}
	}

	else {
		if (is_added_in_date(date))
			return operating;
		else
			return not_operating;
	}
}

Timetables::Structures::services::services(std::istream&& calendar, std::istream&& calendar_dates) {

	string token;
	std::getline(calendar, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<string, 10> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: ServiceID, Monday ... Sunday, ValidSince, ValidUntil

		for (size_t j = 0; j < 10; j++)
			std::getline(calendar, tokens[j], ';');

		service s(tokens[1] == "1" ? true : false, tokens[2] == "1" ? true : false, tokens[3] == "1" ? true : false,
			tokens[4] == "1" ? true : false, tokens[5] == "1" ? true : false, tokens[6] == "1" ? true : false,
			tokens[7] == "1" ? true : false, date_time(tokens[8]), date_time(date_time(tokens[9]), DAY - SECOND));

		list.push_back(move(s));

	}
	
	// Let's process the file with extraordinary events.

	std::getline(calendar_dates, token); // Number of entries.

	size = stoi(token);
	
	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: ServiceID, Date, TypeOfExtraordinaryEvent

		for (size_t j = 0; j < 3; j++)
			std::getline(calendar_dates, tokens[j], ';');

		size_t id(stoi(tokens[0]));

		service& s = list[id];

		bool type = tokens[2] == "1" ? true : false;

		s.add_extraordinary_event(date_time(tokens[1]), type);
	}

}
