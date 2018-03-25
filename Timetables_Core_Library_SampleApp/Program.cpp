#include "../Timetables_Core_Library/data_feed.hpp"
#include "../Timetables_Core_Library/utilities.hpp"
#include "../Timetables_Core_Library/router.hpp"
#include "../Timetables_Core_Library/departure_board.hpp"
#include "../Timetables_Core_Library/exceptions.hpp"
#include "reports.hpp"
#include <exception>
#include <iostream>
#include <sstream>
#include <string>
#include <random>
#include <codecvt>
#include <locale>
#include <Windows.h>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::SampleApp;
using namespace Timetables::Exceptions;

namespace Timetables {
	namespace SampleApp {
		const size_t random_station(const data_feed& feed) {
			std::mt19937 rng;
			rng.seed(std::random_device()());
			std::uniform_int_distribution<std::mt19937::result_type> dist6(0, feed.stations().size() - 1);
			return dist6(rng);
		}

		void cpu_time_micro_benchmark(const data_feed& feed) {
			for (int i = 0; i < 1000; i++) {
				try {
					router r(feed, random_station(feed), random_station(feed), date_time::now(), 1, 10);
					r.obtain_journeys();
				}
				catch (station_not_found ex) {
					wcout << L"Stop " << ex.station_name() << L" not found." << endl;
				}
				catch (journey_not_found ex) {
					wcout << L"No journeys between stops " << ex.stations().first << L" and " << ex.stations().second << L" found." << endl;
				}
				catch (exception ex) {
					cout << "Unknown exception " << ex.what() << endl;
				}
				if (i % 100 == 0)
					cout << i << endl;
			}
		}
	}
}

int main(int argc, char** argv) {

	cout << date_time::now() << " : Application has started." << endl;

	cout << date_time::now() << " : Starting data init." << endl;

	data_feed feed;

	cout << date_time::now() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");
	SetConsoleCP(GetACP());
	SetConsoleOutputCP(GetACP());
		
	locale::global(locale("Czech"));
			
	// cpu_time_micro_benchmark(feed); return 0;

	wcout << endl << "To obtain a departure board, use the command DB;Name of the station;Number of departures shown." << endl << L"E.g. DB;Malostranské námìstí;5." << endl << endl;

	wcout << "To obtain a journey, use the command R;Source station;Target station;Number of journeys shown;Max transfers number." << endl << L"E.g. R;Malostranské námìstí;Nádraží Hostivaø;5;3." << endl << endl;

	wcout << "To exit the application, use the command END." << endl << endl;

	wstring line;
	while (line != L"END") {
		getline(wcin, line);
		wistringstream input(line);

		wstring token;
		getline(input, token, wchar_t(';'));

		try {
			if (token == L"DB") { // Departure board. Example: DB;Malostranské námìstí;3
				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring station;
				getline(input, station, wchar_t(';'));

				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring count;
				getline(input, count);
				get_departure_board_report(feed, station, date_time::now(), stoi(count));
			}

			else if (token == L"R") { // Router. Example: R;Malostranské námìstí;Václavské námìstí;5;5
				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring stationA;
				getline(input, stationA, wchar_t(';'));

				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring stationB;
				getline(input, stationB, wchar_t(';'));

				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring count;
				getline(input, count, wchar_t(';'));

				if (!input.good()) {
					cout << "Too few arguments." << endl << endl;
					continue;
				}
				wstring transfers;
				getline(input, transfers);

				get_journeys_report(feed, stationA, stationB, date_time::now(), stoi(count), stoi(transfers));
			}

			else if (token == L"END")
				continue;

			else
				cout << "Unknown token." << endl << endl;
		}
		catch (...) {
			cout << "Unknown error." << endl << endl;
		}
	}
}