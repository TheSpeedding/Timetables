#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/GtfsFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "Reports.hpp"

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::Exceptions;


void Timetables::SampleApp::GetDepartureBoardReport(const Timetables::Structures::GtfsFeed & feed, const std::wstring & stationName, const Timetables::Structures::Datetime & datetime, const size_t count) {
	
	vector<Departure> departures;

	cout << endl << Time::Now().ToString() << " : Starting departure board finding for "; wcout << stationName; cout << " station." << endl;

	try {		
		departures = GetDepartureBoard(feed, stationName, datetime, count); 	}
	catch (StopNotFoundException ex) {
		cout << "Stop "; wcout << ex.GetStopName(); cout << " not found." << endl;
		return;
	}
	catch (NoDeparturesFoundException ex) {
		cout << "No departures for stop "; wcout << ex.GetStopName(); cout << " found." << endl;
	}

	cout << Time::Now().ToString() << " : Ending departure board finding for "; wcout << stationName; cout << " station." << endl << endl;

	
	for (auto&& dep : departures) {
		cout << "Departure at " << dep.GetDeparture().ToString() << " with line " << dep.GetLine() << " going ahead to "; wcout << dep.GetHeadsign(); cout << " station goes via following stops:" << endl;
		auto followingStops = dep.GetFollowingStops();
		for (auto it = followingStops.cbegin() + 1; it != followingStops.cend(); ++it) {
			cout << "  "; wcout << it->GetStop().GetName(); cout << " with arrival at " << it->GetArrival().ToString() << "." << endl;
		}
		cout << endl;
	}
}
