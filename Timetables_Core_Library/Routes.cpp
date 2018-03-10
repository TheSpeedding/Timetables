#include "Routes.hpp"
#include "Trips.hpp"
#include "StopTime.hpp"
#include <algorithm>
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::Routes::Routes(std::wistream&& routes, RoutesInfo& routesInfo) {
	
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
		
		list.push_back(Route(routesInfo[size_t(stoi(tokens[1]))], stoi(tokens[2]), tokens[3]));

	}

}

void Timetables::Structures::Routes::SetStopsForRoutes() {

	for (auto&& route : list) {

		// We will reconstruct stops from any of the trip using stop times -> stop time has reference to the stop.

		for (auto&& stopTime : route.Trips()[0]->StopTimes())
			route.AddStop(stopTime.Stop());

	}

}
