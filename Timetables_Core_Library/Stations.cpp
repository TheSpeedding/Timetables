#include "stations.hpp"
#include "stop_time.hpp"
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::stations::stations(std::wistream&& stations) {
	stations.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(stations, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: StationID, Name

		for (size_t j = 0; j < 2; j++)
			std::getline(stations, token, wchar_t(';'));
		
		list.push_back(station(token));

	}

}