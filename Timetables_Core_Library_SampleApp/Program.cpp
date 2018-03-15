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

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::SampleApp;
using namespace Timetables::Exceptions;

int main(int argc, char** argv) {

	cout << DateTime::Now() << " : Application has started." << endl;

	cout << DateTime::Now() << " : Starting data init." << endl;

	DataFeed feed;

	cout << DateTime::Now() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");
		
	// CpuTimeMicroBenchmark(feed); return 0;

	cout << endl << "To obtain a departure board, use the command DB;Name of the station;Number of departures shown." << endl << "E.g. DB;Malostranské námìstí;5." << endl << endl;

	cout << "To obtain a journey, use the command R;Source station;Target station;Number of journeys shown;Max transfers number." << endl << "E.g. R;Malostranské námìstí;Nádraží Hostivaø;5;3." << endl << endl;
	
	wstring line;
	while (line != L"END") {
		end:
		getline(wcin, line);
		wistringstream input(line);

		wstring token;
		getline(input, token, wchar_t(';'));

		if (token == L"DB") { // Departure board. Example: DB;Malostranské námìstí;3
			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring station;
			getline(input, station, wchar_t(';'));

			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring count;
			getline(input, count);
			GetDepartureBoardReport(feed, station, DateTime::Now(), stoi(count));
		}

		else if (token == L"R") { // Router. Example: R;Malostranské námìstí;Václavské námìstí;5;5
			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring stationA;
			getline(input, stationA, wchar_t(';'));

			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring stationB;
			getline(input, stationB, wchar_t(';'));

			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring count;
			getline(input, count, wchar_t(';'));

			if (!input.good()) {
				cout << "Too few arguments." << endl << endl;
				goto end;
			}
			wstring transfers;
			getline(input, transfers);

			GetJourneysReport(feed, stationA, stationB, DateTime::Now(), stoi(count), stoi(transfers));
		}

		else
			cout << "Unknown token." << endl << endl;
	}
}