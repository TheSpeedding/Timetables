#include "Stops.hpp"
#include "Exceptions.hpp"
#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include <algorithm>
#include <map>
#include <array>
#include <fstream>
#include <codecvt>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Stops::Stops(std::wistream&& stops) {

	stops.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));

	wstring line;
	getline(stops, line); // The first line is an invalid entry.

	while (stops.good()) {

		array<wstring, 7> tokens;
		getline(stops, tokens[0], wchar_t(','));

		stops.get(); // '"' char
		getline(stops, tokens[1], wchar_t('"'));
		stops.get(); // ',' char

		for (int i = 2; i < 6; i++)
			getline(stops, tokens[i], wchar_t(','));

		getline(stops, tokens[6], wchar_t('\n'));

		if (tokens[0] == wstring()) continue; // Empty line.

		/*
		* tokens[0] = stop id
		* tokens[1] = name
		* tokens[2] = latitude
		* tokens[3] = longitude
		* tokens[4] = location type // 1 = station, 0 = stop, 2 = entrance (currently unused).
		* tokens[5] = parent station id
		* tokens[6] = wheelchair // Currently unused.
		*/

		// The stop can have parent station defined, but does not have to. 

		string locationType(string(tokens[4].begin(), tokens[4].end()));
		string stopId(string(tokens[0].begin(), tokens[0].end()));

		if (locationType == "0") { // The entry is a stop.

			Stop newStop(tokens[1], stod(tokens[2]), stod(tokens[3]), nullptr);

			string parent_station_id(tokens[5].begin(), tokens[5].end());

			stopsList.insert(make_pair(stopId, move(newStop)));

		}

		else if (locationType == "1") { // The entry is a station.
										// Instant skip, not usable at this moment. Our implementation is better in this situation.
		}

		else if (locationType == "2")
			throw invalid_argument("TO-DO: Support for station entrances/exits."); // Not relevant for PID Timetables.
	}

	// Let's create stations for the stops.

	for (auto&& stop : stopsList) {
		auto it = stationsList.find(stop.second.GetName());
		if (it == stationsList.cend()) {
			stationsList.insert(make_pair(stop.second.GetName(), Station(stop.second.GetName(), stop.second.GetLocation())));
			it = stationsList.find(stop.second.GetName());
		}
		it->second.AddStop(stop.second);
		stop.second.SetParentStation(it->second);
	}

	// Let's add some footpaths.
	// Some GTFS feeds include these footpaths, PID feed unfortunately does not. 
	// So we have to compute it manually, at least approximately.

	// Note: This is measured as aerial distance, ignoring obstacles. Easier to compute. That's why so low average speed was chosen (0,5 m/s).
	// Half time of the initialization is spent in this block of code (due to calling arcsin and cos stl functions). Unfortunately, there's no way to make it better.

	for (auto&& A : stopsList)
		for (auto&& B : stopsList) {
			if (&A.second == &B.second) continue;
			int Time = GpsCoords::GetWalkingTime(A.second.GetLocation(), B.second.GetLocation());
			if (Time < 1200) // Heuristic: Walking Time between two stops should be 20 minutes at max, otherwise it loses the point. Saves a lot of memory.
				A.second.AddFootpath(B.second, Time);
		}
}

const Station& Timetables::Structures::Stops::GetStation(const std::wstring& name) const {
	auto stat = stationsList.find(name);
	if (stat == stationsList.cend())
		throw StopNotFoundException(name);
	else
		return stat->second;
}

void Timetables::Structures::Stops::SetThroughgoingRoutesForStops(const Routes & routes) {
	for (auto&& route : routes.GetRoutes()) {
		const vector<StopPtrObserver>& stops = route.GetStops();
		for (auto&& s : stops) {
			Stop& stop = GetStop(*s); // Variable s is const reference (pointer respectively) to required stop. We need the non-const one.
			auto it = find(stop.GetRoutes().cbegin(), stop.GetRoutes().cend(), &route);
			if (it == stop.GetRoutes().cend())
				stop.AddRoute(route);
		}
	}
}
