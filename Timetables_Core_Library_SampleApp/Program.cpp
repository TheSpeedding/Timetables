#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Router.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "Reports.hpp"
#include <sstream>

using namespace std;
using namespace Timetables::Structures;
using namespace Timetables::Algorithms;
using namespace Timetables::SampleApp;

int main(int argc, char** argv) {
	
	cout << DateTime::Now() << " : Application has started." << endl;

	cout << DateTime::Now() << " : Starting data init." << endl;
	
	DataFeed feed;

	cout << DateTime::Now() << " : Ending data init." << endl;

	setlocale(LC_ALL, "");
	
	GetDepartureBoardReport(feed, L"Klobouènická", DateTime::Now(), 2);

	GetDepartureBoardReport(feed, L"Malostranská", DateTime::Now(), 2);

	GetDepartureBoardReport(feed, L"Roztyly", DateTime(23, 50, 00, 28, 02, 2018), 3);

	GetJourneysReport(feed, L"Roztyly", L"Dejvická", DateTime(10, 15, 00, 6, 3, 2018), 1, 5);
	
}