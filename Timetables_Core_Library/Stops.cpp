#include "stops.hpp"
#include "trips.hpp"
#include "stop_time.hpp"
#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::stops::stops(std::istream&& stops, std::istream&& footpaths, stations& stations) {

	string token;
	std::getline(stops, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);


	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: StopID, ParentStationID

		for (size_t j = 0; j < 2; j++)
			std::getline(stops, token, ';');

		station& station = stations[stoi(token)];

		stop s(station);
		
		list.push_back(move(s));

		station.add_child_stop(*(list.cend() - 1));

	}

	// Sets footpaths.

	std::getline(footpaths, token); // Number of entries.

	size = stoi(token);

	array<string, 3> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: Duration, FirstStop, SecondStop

		for (size_t j = 0; j < 3; j++)
			std::getline(footpaths, tokens[j], ';');

		stop& stopA = list[stoi(tokens[1])];

		stop& stopB = list[stoi(tokens[2])];

		stopA.add_footpath(stopB, stoi(tokens[0]));
	}
}

void Timetables::Structures::stops::set_throughgoing_routes_for_stops(routes& routes) {

	for (size_t i = 0; i < routes.size(); i++) {

		const vector<const stop*>& stops = routes[i].stops();

		for (auto&& s : stops) {

			size_t index = s - list.data(); // Variable s is a const observer pointer to the stop. We have to modify it. We need non-const reference.
											// So we will use contiguousnity of vector elements and compute index in the vector (of that stop) using difference of two addresses.

			stop& stop = list[index];

			auto it = find(stop.throughgoing_routes().cbegin(), stop.throughgoing_routes().cend(), &routes[i]);

			if (it == stop.throughgoing_routes().cend())
				stop.add_throughgoing_route(routes[i]);

		}

	}
}
