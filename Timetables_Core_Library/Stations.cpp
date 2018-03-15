#include "Stations.hpp"
#include "StopTime.hpp"
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::Stations::Stations(std::wistream&& stations) {
	stations.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(stations, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: StationID, Name

		for (size_t j = 0; j < 2; j++)
			std::getline(stations, token, wchar_t(';'));
		
		list.push_back(Station(token));

	}

}

void Timetables::Structures::Station::AddDeparture(const StopTime & stopTime) {
	// We have to set new time because of the time relativity. That means departure of the trip + departure from given stoptime. 
	// Plus we will normalize it. This serves only for departure boards.
	departures.insert(std::make_pair(DateTime((stopTime.Trip().Departure() + stopTime.Departure()) % 86400), &stopTime));
} 
