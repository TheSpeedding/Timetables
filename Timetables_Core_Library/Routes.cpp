#include "routes.hpp"
#include "trips.hpp"
#include "stop_time.hpp"
#include <algorithm>
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::routes::routes(std::wistream&& routes, routes_info& routes_info) {
	
	routes.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(routes, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<wstring, 4> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: RouteID, RouteInfoID, NumberOfStopTimes, Headsign

		for (size_t j = 0; j < 4; j++)
			std::getline(routes, tokens[j], wchar_t(';'));
		
		list.push_back(route(routes_info[size_t(stoi(tokens[1]))], stoi(tokens[2]), tokens[3]));

	}

}

void Timetables::Structures::routes::set_stops_for_routes() {

	for (auto&& route : list) {

		// We will reconstruct stops from any of the trip using stop times -> stop time has reference to the stop.

		for (auto&& stop_time : route.trips()[0]->stop_times())
			route.add_stop(stop_time.stop());

	}

}
