#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "../Timetables_Core_Library/Router.hpp"
#include "Reports.hpp"
#include <Windows.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::Exceptions;


void Timetables::SampleApp::GetDepartureBoardReport(const Timetables::Structures::DataFeed& feed, const std::wstring& stationName, const Timetables::Structures::DateTime& dateTime, const size_t count) {
	
	vector<Departure> departures;

	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	SetConsoleTextAttribute(hConsole, 7);

	cout << endl << DateTime::Now() << " : Starting departure board finding for "; wcout << stationName; cout << " station in datetime " << dateTime << "." << endl;

	try {
		DepartureBoard db(feed, stationName, dateTime, count);
		db.ObtainDepartureBoard();
		departures = db.ShowDepartureBoard();
	}
	catch (StationNotFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"Stop " << ex.GetStationName() << L" not found." << endl << endl;
		return;
	}
	catch (NoDeparturesFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"No departures for stop " << ex.GetStationName() << L" found." << endl << endl;
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

		cout << "Departure at " << dep.DepartureTime() << " with line " << dep.Line().ShortName() << " going ahead to "; wcout << dep.Headsign(); cout << " station goes via following stops:" << endl;
		
		auto followingStops = dep.FollowingStops();

		for (auto it = followingStops.cbegin(); it != followingStops.cend(); ++it) {
			cout << "  "; wcout << it->second->Name(); cout << " with arrival at " << dep.DepartureTime().AddSeconds(it->first) << "." << endl;
		}

		cout << endl;

		SetConsoleTextAttribute(hConsole, 7);
	}
}

void Timetables::SampleApp::GetJourneysReport(const Timetables::Structures::DataFeed& feed, const std::wstring& A, const std::wstring& B, const Timetables::Structures::DateTime& dateTime, const size_t count, const size_t maxTransfers) {
	
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	SetConsoleTextAttribute(hConsole, 7);

	cout << endl << DateTime::Now() << " : Starting journey searching between stops "; wcout << A << L" and " << B << L"." << endl;

	try {
		Router r(feed, A, B, dateTime, count, maxTransfers);
		r.ObtainJourneys();
		auto journeys = r.ShowJourneys();

		SetConsoleTextAttribute(hConsole, 7);

		cout << DateTime::Now() << " : Ending journey searching between stops "; wcout << A << L" and " << B << L"." << endl;


		for (auto&& journey : journeys) {

			SetConsoleTextAttribute(hConsole, 15);

			cout << endl << "Showing journey in total duration of " << journey.second.TotalDuration() / 60 << " minutes and " << journey.second.TotalDuration() % 60 << " seconds";
			cout << " leaving source station at " << journey.second.DepartureTime();
			cout << " and approaching target station at " << journey.second.ArrivalTime() << "." << endl << endl;

			const Stop* previousStop = nullptr;

			auto itTransfers = journey.second.Transfers().cbegin();

			for (auto it = journey.second.JourneySegments().cbegin(); it != journey.second.JourneySegments().cend(); ++it) {

				if (it != journey.second.JourneySegments().cbegin() && it->IntermediateStops().cbegin()->second != previousStop) { // Transfer.
					
					SetConsoleTextAttribute(hConsole, 15);

					cout << "Transfer " << *itTransfers / 60 << " minutes and " << *itTransfers % 60 << " seconds.";

					cout << endl << endl;

					itTransfers++;
				}

				int color = 0;
				switch (it->Trip().Route().Info().Color()) {
				case 0x408000: color = 10; break;
				case 0xFFFF00: color = 14; break;
				case 0xFF0000: color = 12; break;
				case 0x00FFFF: color = 11; break;
				case 0xCC0000: color = 6; break;
				default: color = 15; break;
				}

				SetConsoleTextAttribute(hConsole, color);


				cout << "Board the line " << it->Trip().Route().Info().ShortName() << " at " << it->DepartureFromSource() << " in ";
				wcout << it->IntermediateStops().cbegin()->second->Name() << " station going ahead to ";
				wcout << it->Trip().Route().Headsign() << L" station via following stops:" << endl;

				auto intStops = it->IntermediateStops();

				for (auto it1 = intStops.cbegin() + 1; it1 != intStops.cend() - 1; ++it1) {
					wcout << L"  " << it1->second->Name(); cout << " with arrival at " << it->DepartureFromSource().AddSeconds(it1->first) << "." << endl;
				}

				cout << "Get off the line " << it->Trip().Route().Info().ShortName() << " at " << it->ArrivalAtTarget() << " in ";
				wcout << (it->IntermediateStops().cend() - 1)->second->Name() << " station." << endl << endl;

				previousStop = (it->IntermediateStops().cend() - 1)->second;
			}

			if (itTransfers != journey.second.Transfers().cend()) { // Transfer.

				SetConsoleTextAttribute(hConsole, 15);

				cout << "Transfer " << *itTransfers / 60 << " minutes and " << *itTransfers % 60 << " seconds.";

				cout << endl << endl;

			}

		}
	}

	catch (StationNotFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"Stop " << ex.GetStationName() << L" not found." << endl << endl;
		return;
	}
	catch (JourneyNotFoundException ex) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"No journeys between stops " << ex.GetStations().first << L" and " << ex.GetStations().second << L" found." << endl << endl;
	}


	SetConsoleTextAttribute(hConsole, 7);	
}
