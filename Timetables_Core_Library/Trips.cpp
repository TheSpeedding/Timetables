#include "trips.hpp"
#include "exceptions.hpp"
#include "stop_time.hpp"
#include <string>
#include <array>
#include <algorithm>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::trips::trips(std::istream&& trips, routes_info& routes_info, routes& routes, services& services) {
	
	string token;
	std::getline(trips, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<string, 4> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: TripID, ServiceID, RouteID, DepartureTime

		for (size_t j = 0; j < 4; j++)
			std::getline(trips, tokens[j], ';');
				
		service& service = services[stoi(tokens[1])];

		route& route = routes[stoi(tokens[2])];

		trip t(service, route, stoi(tokens[3]));

		list.push_back(move(t));

		route.add_trip(*(list.cend() - 1));
	}

}

void Timetables::Structures::trips::set_timetables(std::istream&& stop_times, stops& stops) {

	string token;
	std::getline(stop_times, token); // Number of entries.

	size_t size = stoi(token);
	
	array<string, 4> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: TripID, StopID, ArrivalTime, DepartureTime

		for (size_t j = 0; j < 4; j++)
			std::getline(stop_times, tokens[j], ';');

		trip& trip = list[stoi(tokens[0])];

		stop& stop = stops[stoi(tokens[1])];

		stop_time st(trip, stop, stoi(tokens[2]), stoi(tokens[3]));

		trip.add_to_trip(move(st));

		stop.add_departure(*(trip.stop_times().cend() - 1));

	}

}