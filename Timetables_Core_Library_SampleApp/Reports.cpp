/*#include <exception>
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
		DepartureBoard db(feed, stationName, datetime, count);
		db.ObtainDepartureBoard();
		departures = db.GetDepartureBoard();
	}
	catch (StopNotFoundException ex) {
		wcout << L"Stop " << ex.GetStopName() << L" not found." << endl;
		return;
	}
	catch (NoDeparturesFoundException ex) {
		wcout << L"No departures for stop " << ex.GetStopName() << L" found." << endl;
	}

	cout << Time::Now().ToString() << " : Ending departure board finding for "; wcout << stationName << L" station." << endl << endl;

	
	for (auto&& dep : departures) {
		cout << "Departure at " << dep.GetDeparture().ToString() << " with line " << dep.GetLine() << " going ahead to "; wcout << dep.GetHeadsign(); cout << " station goes via following stops:" << endl;
		auto followingStops = dep.GetFollowingStops();
		for (auto it = followingStops.cbegin() + 1; it != followingStops.cend(); ++it) {
			cout << "  "; wcout << it->GetStop().GetName(); cout << " with arrival at " << it->GetArrival().ToString() << "." << endl;
		}
		cout << endl;
	}
}
*/