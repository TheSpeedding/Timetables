#include "Trips.hpp"
#include "Exceptions.hpp"
#include <string>
#include <array>
#include <algorithm>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Trips::Trips(std::wistream&& trips, RoutesInfo& routesInfo, Routes& routes, Services& services) {

	trips.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(trips, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<wstring, 6> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: TripID, RouteInfoID, ServiceID, RouteID, Headsign, NumberOfStopTimes

		for (size_t j = 0; j < 6; j++)
			std::getline(trips, tokens[j], wchar_t(';'));

		RouteInfo& routeInfo = routesInfo[stoi(tokens[1])];

		Service& service = services[stoi(tokens[2])];

		Route& route = routes[stoi(tokens[3])];

		Trip t(routeInfo, service, tokens[4], stoi(tokens[5]));

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

		StopTime st(trip, stop, DateTime(tokens[2]), DateTime(tokens[3]));

		trip.AddToTrip(move(st));

		stop.AddDeparture(*(trip.StopTimes().cend() - 1));


	}

}