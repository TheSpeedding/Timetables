#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/GtfsFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "Reports.hpp"
#include <sstream>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::Algorithms;

int main(int argc, char** argv) {
	
	cout << Time::Now().ToString() << " : Application has started." << endl;

	cout << Time::Now().ToString() << " : Starting data init." << endl;

	Timetables::Structures::GtfsFeed feed("GTFS/");

	cout << Time::Now().ToString() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");

	// Stanice nenalezena.
	Timetables::SampleApp::GetDepartureBoardReport(feed, L"Zvona�ka", Datetime::Now(), 100000);

	// ��dn� odjezdy.
	Timetables::SampleApp::GetDepartureBoardReport(feed, L"Hellichova", Datetime::Now(), 100000);

	Timetables::SampleApp::GetDepartureBoardReport(feed, L"Malostransk� n�m�st�", Datetime::Now(), 3);

	Timetables::SampleApp::GetDepartureBoardReport(feed, L"Malostransk�", Datetime::Now(), 5);
}
