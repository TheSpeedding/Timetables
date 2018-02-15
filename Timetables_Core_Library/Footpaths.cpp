#include "Footpaths.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Footpaths::Footpaths(const Stops& stops) {

	// Some GTFS feeds includes these footpaths, PID feed unfortunately does not. 
	// So we have to compute it manually, at least approximately.

	for (auto&& A : stops.GetStops())
		for (auto&& B : stops.GetStops()) {
			if (&A.second == &B.second) continue;
			int time = GpsCoords::GetWalkingTime(A.second.GetLocation(), B.second.GetLocation());
			if (time < 900) // Heuristic: Walking time between two stops should be 15 minutes at max. Saves a lot of memory.
				walkingTime.insert(make_pair(make_pair(&A.second, &B.second), time));
		}
}
