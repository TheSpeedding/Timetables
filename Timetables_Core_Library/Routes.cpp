#include "Routes.hpp"
#include <algorithm>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

Timetables::Structures::Routes::Routes(const RoutesInfo& info, const Trips& trips) {

	for (auto&& trip : trips.GetTrips()) {
		vector<StopPtrObserver> stopsSequence;

		for (auto&& stopTime : trip.GetStopTimes())
			stopsSequence.push_back(&stopTime->GetStop());

		Route r(trip.GetRouteInfo(), move(stopsSequence));
		auto it = find(list.begin(), list.end(), r);

		if (it == list.cend()) {
			list.push_back(r);
			it = find(list.begin(), list.end(), r); // We have to look for it once again due to possible vector reallocation.
		}

		it->AddTrip(trip.GetStopTimes().cbegin()->get()->GetDeparture(), trip);
	}
}
