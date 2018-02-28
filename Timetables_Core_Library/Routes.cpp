#include "Routes.hpp"
#include <algorithm>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::Routes::Routes(std::istream&& routes, RoutesInfo& routesInfo) {
	
	string token;
	std::getline(routes, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<string, 3> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: RouteID, RouteInfoID, NumberOfStopTimes

		for (size_t j = 0; j < 3; j++)
			std::getline(routes, tokens[j], ';');
		
		list.push_back(Route(routesInfo[size_t(stoi(tokens[1]))], stoi(tokens[2])));

	}
}

void Timetables::Structures::Routes::SetStopsForRoutes() {

	for (auto&& route : list) {

		// We will reconstruct stops from any of the trip using stop times -> stop time has reference to the stop.

		for (auto&& stopTime : route.Trips()[0]->StopTimes())
			route.AddStop(stopTime.Stop());

	}

}
