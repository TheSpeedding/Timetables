#include "Routes.hpp"
#include <algorithm>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Exceptions;

/*
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
}*/

Timetables::Structures::Routes::Routes(std::istream&& routes, RoutesInfo& routesInfo) {
	
	string token;
	std::getline(routes, token); // Number of entries.

	size_t size = stoi(token);

	list.reserve(size);

	for (size_t i = 0; i < size; i++) { // Over all the entries.

		// Entry format: RouteID, RouteInfoID

		for (size_t i = 0; i < 2; i++)
			std::getline(routes, token, ';');
		
		list.push_back(Route(routesInfo[size_t(stoi(token))]));

	}
}

void Timetables::Structures::Routes::SetStopsForRoutes() {

	for (auto&& route : list) {

		// We will reconstruct stops from any of the trip using stop times -> stop time has reference to the stop.

		for (auto&& stopTime : route.Trips()[0]->StopTimes())
			route.AddStop(stopTime.Stop());

	}

}
