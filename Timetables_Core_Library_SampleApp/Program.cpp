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
	
	//GetDepartureBoardReport(feed, L"Malostranské námìstí", DateTime::Now(), 3);

	//GetDepartureBoardReport(feed, L"Klobouènická", DateTime::Now(), 2);

	GetDepartureBoardReport(feed, L"Roztyly", DateTime(23, 55, 00, 28, 02, 2018), 6);

	/*
	Router r(feed, L"Klobouènická", L"Roztyly", Datetime::Now(), 5, 5);

	r.ObtainJourney();

	r.GetJourneys();
	*/

}