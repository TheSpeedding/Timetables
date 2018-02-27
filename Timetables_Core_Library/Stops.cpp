#include "Stops.hpp"
#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <array>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;

Timetables::Structures::Stops::Stops(std::wistream&& stops, Stations& stations) {
	stops.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring token;
	std::getline(stops, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	array<wstring, 5> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.
		// Entry format: StopID, Name, Latitude, Longitude, ParentStationID
		for (size_t i = 0; i < 5; i++)
			std::getline(stops, tokens[i], wchar_t(';'));

		Station& station = stations[stoi(tokens[4])];

		Stop s(tokens[1], stod(tokens[2]), stod(tokens[3]), station);
		
		list.push_back(move(s));

		station.AddChildStop(*(list.cend() - 1));

	}
}

void Timetables::Structures::Stops::SetFootpaths(std::istream&& footpaths) {
	string token;
	std::getline(footpaths, token); // Number of entries.

	size_t size = stoi(token);
	
	array<string, 3> tokens;

	for (size_t i = 0; i < size; i++) { // Over all the entries.
		// Entry format: Duration, FirstStop, SecondStop
		for (size_t i = 0; i < 3; i++)
			std::getline(footpaths, tokens[i], ';');

		Stop& stopA = list[stoi(tokens[1])];

		Stop& stopB = list[stoi(tokens[2])];

		stopA.AddFootpath(stopB, stoi(tokens[0]));
	}
}

void Timetables::Structures::Stops::SetThroughgoingRoutesForStops(Routes& routes) {

	for (size_t i = 0; i < routes.Count; i++) {

		const vector<const Stop*>& stops = routes[i].Stops();

		for (auto&& s : stops) {

			Stop& stop = Stop(*s); // Variable s is const reference (pointer respectively) to required stop. We need the non-const one.

			auto it = find(stop.ThroughgoingRoutes().cbegin(), stop.ThroughgoingRoutes().cend(), &routes[i]);

			if (it == stop.ThroughgoingRoutes().cend())
				stop.AddThroughgoingRoute(routes[i]);

		}

	}

}
