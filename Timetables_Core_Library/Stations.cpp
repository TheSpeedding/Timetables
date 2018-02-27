#include "Stations.hpp"
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Stations::Stations(std::wistream&& stations) {
	stations.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(stations, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: StationID, Name

		for (size_t i = 0; i < 2; i++)
			std::getline(stations, token, wchar_t(';'));
		
		list.push_back(Station(token));

	}

}
