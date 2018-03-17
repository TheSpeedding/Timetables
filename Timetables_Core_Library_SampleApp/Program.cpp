#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Router.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "Reports.hpp"
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
		const wstring& RandomStation(const DataFeed& feed) {
			std::mt19937 rng;
			rng.seed(std::random_device()());
			std::uniform_int_distribution<std::mt19937::result_type> dist6(0, feed.Stations().Count() - 1);
			return feed.Stations().Get(dist6(rng)).Name();
		}

		void CpuTimeMicroBenchmark(const DataFeed& feed) {
			for (int i = 0; i < 1000; i++) {
				try {
					Router r(feed, RandomStation(feed), RandomStation(feed), DateTime::Now(), 1, 10);
					r.ObtainJourneys();
				}
				catch (StationNotFoundException ex) {
					wcout << L"Stop " << ex.GetStationName() << L" not found." << endl;
				}
				catch (JourneyNotFoundException ex) {
					wcout << L"No journeys between stops " << ex.GetStations().first << L" and " << ex.GetStations().second << L" found." << endl;
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

	cout << DateTime::Now() << " : Application has started." << endl;

	cout << DateTime::Now() << " : Starting data init." << endl;

	DataFeed feed;

	cout << DateTime::Now() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");
	SetConsoleCP(GetACP());
	SetConsoleOutputCP(GetACP());
		
	locale::global(locale("Czech"));
			
	// CpuTimeMicroBenchmark(feed); return 0;

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
				GetDepartureBoardReport(feed, station, DateTime::Now(), stoi(count));
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

				GetJourneysReport(feed, stationA, stationB, DateTime::Now(), stoi(count), stoi(transfers));
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