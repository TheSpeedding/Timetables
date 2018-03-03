#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "Reports.hpp"
#include <Windows.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::Exceptions;


void Timetables::SampleApp::GetDepartureBoardReport(const Timetables::Structures::DataFeed & feed, const std::wstring & stationName, const Timetables::Structures::DateTime & dateTime, const size_t count) {
	
	vector<Departure> departures;

	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	SetConsoleTextAttribute(hConsole, 7);

	cout << endl << DateTime::Now() << " : Starting departure board finding for "; wcout << stationName; cout << " station in datetime " << dateTime << "." << endl;

	try {		
		DepartureBoard db(feed, stationName, dateTime, count);
		db.ObtainDepartureBoard();
		departures = db.ShowDepartureBoard();
	}
	catch (StopNotFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"Stop " << ex.GetStopName() << L" not found." << endl;
		return;
	}
	catch (NoDeparturesFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"No departures for stop " << ex.GetStopName() << L" found." << endl;
	}

	SetConsoleTextAttribute(hConsole, 7);

	cout << DateTime::Now() << " : Ending departure board finding for "; wcout << stationName << L" station." << endl << endl;

	
	for (auto&& dep : departures) {
		int color = 0;

		switch (dep.Line().Color()) {
		case 0x408000: color = 10; break;
		case 0xFFFF00: color = 14; break;
		case 0xFF0000: color = 12; break;
		case 0x00FFFF: color = 11; break;
		case 0xCC0000: color = 6; break;
		default: color = 15; break;
		}

		SetConsoleTextAttribute(hConsole, color);

		cout << "Departure at " << dep.DepartureTime().ToString() << " with line " << dep.Line().ShortName() << " going ahead to "; wcout << dep.Headsign(); cout << " station goes via following stops:" << endl;
		auto followingStops = dep.FollowingStops();
		for (auto it = followingStops.cbegin() + 1; it != followingStops.cend(); ++it) {
			cout << "  "; wcout << it->Stop().Name(); cout << " with arrival at " << it->ArrivalTime().ToString() << "." << endl;
		}
		cout << endl;
	}
}
