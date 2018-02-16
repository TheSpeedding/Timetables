#include <exception>
#include <iostream>
#include <string>
#include "../Timetables_Core_Library/GtfsFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"

using namespace std;
using namespace Timetables::Structures;

int main(int argc, char** argv) {
	
	cout << Time::Now().ToString() << " : Application has started." << endl;

	cout << Time::Now().ToString() << " : Starting Structures init." << endl;

	GtfsFeed feed("GTFS/");

	cout << Time::Now().ToString() << " : Ending Structures init." << endl << endl;
}
