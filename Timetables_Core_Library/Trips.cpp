#include "Trips.hpp"
#include "Exceptions.hpp"
#include <string>
#include <array>
#include <algorithm>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Trips::Trips(std::wistream&& trips, Routes& routes, const Services& services, const Shapes& shapes) {

	trips.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring line;
	getline(trips, line); // The first line is an invalid entry.

	while (trips.good()) {
		array<wstring, 6> tokens;
		for (int i = 0; i < 3; i++)
			getline(trips, tokens[i], wchar_t(','));

		trips.get(); // '"' char
		getline(trips, tokens[3], wchar_t('"'));
		trips.get(); // ',' char

		getline(trips, tokens[4], wchar_t(','));
		getline(trips, tokens[5], wchar_t('\n'));

		if (tokens[0] == wstring()) continue; // Empty line.

		/*
		* tokens[0] = route id
		* tokens[1] = service id
		* tokens[2] = trip id
		* tokens[3] = trip headsign
		* tokens[4] = shape id
		* tokens[5] = wheelchair // Currently unused.
		*/

		Route& route = routes.GetRoute(string(tokens[0].begin(), tokens[0].end()));

		const Service& service = services.GetService(stoi(tokens[1]));

		const ShapesSequence& shape = shapes.GetShapesSequence(stoi(tokens[4]));

		list.push_back(Trip(route, service, shape, tokens[3]));
	}

	for (auto&& trip : list)
		trip.GetRoute().AddTrip(trip);
}

void Timetables::Structures::Trips::SetTimetables(std::istream&& stop_times, Stops& stops) {

	string line;
	getline(stop_times, line); // The first line is an invalid entry.

	while (stop_times.good()) {

		array<string, 7> tokens;
		for (int i = 0; i < 6; i++)
			getline(stop_times, tokens[i], ',');
		getline(stop_times, tokens[6], '\n');

		if (tokens[0] == "") continue; // Empty line.

		/*
		* tokens[0] = trip id
		* tokens[1] = arrival
		* tokens[2] = departure
		* tokens[3] = stop id
		* tokens[4] = stop sequence
		* tokens[5] = pickup type // Currently unused.
		* tokens[6] = drop off type // Currently unused.
		*/

		Time arrival(tokens[1]);
		Time departure(tokens[2]);
		Trip& trip = GetTrip(stoi(tokens[0]));
		Stop& stop = stops.GetStop(tokens[3]);

		// Shared pointers might be better but they have big memory overhead.

		trip.AddToTrip(make_unique<StopTime>(stop, trip, arrival, departure));

		stop.AddDeparture(departure, *(--trip.GetStopTimes().cend())->get());
	}
}