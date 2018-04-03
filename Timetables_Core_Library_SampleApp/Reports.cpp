#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/Structures/data_feed.hpp"
#include "../Timetables_Core_Library/Structures/journey.hpp"
#include "../Timetables_Core_Library/Structures/date_time.hpp"
#include "../Timetables_Core_Library/Algorithms/departure_board.hpp"
#include "../Timetables_Core_Library/Algorithms/router.hpp"
#include "Reports.hpp"
#include <Windows.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;


void Timetables::SampleApp::get_departure_board_report(const Timetables::Structures::data_feed& feed, const std::wstring& station_name, const Timetables::Structures::date_time& date_time, const size_t count) {
	
	vector<departure> departures;

	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	SetConsoleTextAttribute(hConsole, 7);

	cout << endl << date_time::now() << " : Starting departure board finding for "; wcout << station_name; cout << " station in datetime " << date_time << "." << endl;

	departure_board db(feed, feed.stations().find_index(station_name), date_time, count, true);

	db.obtain_departure_board();

	if (db.show_departure_board().size() == 0) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"No departures for stop " << station_name << L" found." << endl << endl;
		return;
	}

	departures = db.show_departure_board();

	SetConsoleTextAttribute(hConsole, 7);

	cout << date_time::now() << " : Ending departure board finding for "; wcout << station_name << L" station." << endl << endl;

	
	for (auto&& dep : departures) {
		int color = 0;

		switch (dep.line().color()) {
		case 0x408000: color = 10; break;
		case 0xFFFF00: color = 14; break;
		case 0xFF0000: color = 12; break;
		case 0x00FFFF: color = 11; break;
		case 0xCC0000: color = 6; break;
		default: color = 15; break;
		}

		SetConsoleTextAttribute(hConsole, color);


		if (dep.outdated())
			cout << "Note that this departure uses outdated timetables, consider updating them." << endl << endl;

		cout << "Departure at " << dep.departure_time() << " with line " << dep.line().short_name() << " going ahead to "; wcout << dep.headsign(); cout << " station goes via following stops:" << endl;
		
		auto following_stops = dep.following_stops();

		for (auto it = following_stops.cbegin(); it != following_stops.cend(); ++it) {
			cout << "  "; wcout << it->second->name(); cout << " with arrival at " << Timetables::Structures::date_time(dep.departure_time(), SECOND * it->first) << "." << endl;
		}

		cout << endl;

		SetConsoleTextAttribute(hConsole, 7);
	}
}

void Timetables::SampleApp::get_journeys_report(const Timetables::Structures::data_feed& feed, const std::wstring& A, const std::wstring& B, const Timetables::Structures::date_time& date_time, const size_t count, const size_t max_transfers) {
	
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	SetConsoleTextAttribute(hConsole, 7);

	cout << endl << date_time::now() << " : Starting journey searching between stops "; wcout << A << L" and " << B << L"." << endl;

	router r(feed, feed.stations().find_index(A), feed.stations().find_index(B), date_time, count, max_transfers);

	r.obtain_journeys();

	if (r.show_journeys().size() == 0) {
		SetConsoleTextAttribute(hConsole, 7);
		wcout << L"No journeys between stops " << A << L" and " << B << L" found." << endl << endl;
		return;
	}

	auto journeys = r.show_journeys();

	SetConsoleTextAttribute(hConsole, 7);

	cout << date_time::now() << " : Ending journey searching between stops "; wcout << A << L" and " << B << L"." << endl;


	for (auto&& journey : journeys) {

		SetConsoleTextAttribute(hConsole, 15);

		cout << endl << "Showing journey in total duration of " << journey.duration() / 60 << " minutes and " << journey.duration() % 60 << " seconds";
		cout << " leaving source station at " << journey.departure_time();
		cout << " and approaching target station at " << journey.arrival_time() << "." << endl << endl;

		if (journey.outdated()) 
			cout << "Note that this journey uses outdated timetables, consider updating them." << endl << endl;

		const stop* previous_stop = nullptr;


		for (auto&& it : journey.journey_segments()) {

			if (it->trip() == nullptr) { // Transfer.
				SetConsoleTextAttribute(hConsole, 15);

				int duration = date_time::difference(it->arrival_at_target(), it->departure_from_source());

				cout << "Transfer " << duration / 60 << " minutes and " << duration % 60 << " seconds.";

				cout << endl << endl;
			}

			else { // Trip segment.

				int color = 0;
				switch (it->trip()->route().info().color()) {
				case 0x408000: color = 10; break;
				case 0xFFFF00: color = 14; break;
				case 0xFF0000: color = 12; break;
				case 0x00FFFF: color = 11; break;
				case 0xCC0000: color = 6; break;
				default: color = 15; break;
				}

				SetConsoleTextAttribute(hConsole, color);

				cout << "Board the line " << it->trip()->route().info().short_name() << " at " << it->departure_from_source() << " at ";
				wcout << it->intermediate_stops().cbegin()->second->name() << " station going ahead to ";
				wcout << it->trip()->route().headsign() << L" station via following stops:" << endl;

				auto intermediate_stops = it->intermediate_stops();

				for (auto it1 = intermediate_stops.cbegin() + 1; it1 != intermediate_stops.cend() - 1; ++it1) {
					wcout << L"  " << it1->second->name(); cout << " with arrival at " << Timetables::Structures::date_time(it->departure_from_source(), SECOND * it1->first) << "." << endl;
				}

				cout << "Get off the line " << it->trip()->route().info().short_name() << " at " << it->arrival_at_target() << " in ";
				wcout << (it->intermediate_stops().cend() - 1)->second->name() << " station." << endl << endl;

				previous_stop = (it->intermediate_stops().cend() - 1)->second;

			}
		}
	}

	SetConsoleTextAttribute(hConsole, 7);	
}
