#include "Trips.hpp"
#include "Exceptions.hpp"
#include "StopTime.hpp"
#include <string>
#include <array>
#include <algorithm>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Trips::Trips(std::istream&& trips, RoutesInfo& routesInfo, Routes& routes, Services& services) {
	
	string token;
	std::getline(trips, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<string, 4> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: TripID, ServiceID, RouteID, DepartureTime

		for (size_t j = 0; j < 4; j++)
			std::getline(trips, tokens[j], ';');
				
		Service& service = services[stoi(tokens[1])];

		Route& route = routes[stoi(tokens[2])];

		Trip t(service, route, stoi(tokens[3]));

		list.push_back(move(t));

		route.AddTrip(*(list.cend() - 1));
	}

}

void Timetables::Structures::Trips::SetTimetables(std::istream&& stopTimes, Stops& stops) {

	string token;
	std::getline(stopTimes, token); // Number of entries.

	size_t size = stoi(token);
	
	array<string, 4> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: TripID, StopID, ArrivalTime, DepartureTime

		for (size_t j = 0; j < 4; j++)
			std::getline(stopTimes, tokens[j], ';');

		Trip& trip = list[stoi(tokens[0])];

		Stop& stop = stops[stoi(tokens[1])];

		StopTime st(trip, stop, stoi(tokens[2]), stoi(tokens[3]));

		trip.AddToTrip(move(st));

		stop.ParentStation().AddDeparture(*(trip.StopTimes().cend() - 1));


	}

}