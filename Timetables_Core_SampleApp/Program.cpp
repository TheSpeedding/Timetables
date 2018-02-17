#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/GtfsFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "../Timetables_Core_Library/Router.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "Reports.hpp"
#include <sstream>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::SampleApp;

int main(int argc, char** argv) {
	
	cout << Time::Now().ToString() << " : Application has started." << endl;

	cout << Time::Now().ToString() << " : Starting data init." << endl;

	Timetables::Structures::GtfsFeed feed("GTFS/");

	cout << Time::Now().ToString() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");

	// Stanice nenalezena.
	GetDepartureBoardReport(feed, L"Zvonaøka", Datetime::Now(), 100000);

	// Žádné odjezdy.
	GetDepartureBoardReport(feed, L"Hellichova", Datetime::Now(), 100000);

	GetDepartureBoardReport(feed, L"Malostranské námìstí", Datetime::Now(), 3);

	GetDepartureBoardReport(feed, L"Malostranská", Datetime::Now(), 5);

	FindRoutes(feed, L"Klobouènická", L"Roztyly", Datetime::Now(), 1);
}
