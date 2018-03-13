#include "../Timetables_Core_Library/DataFeed.hpp"
#include "../Timetables_Core_Library/Utilities.hpp"
#include "../Timetables_Core_Library/Router.hpp"
#include "../Timetables_Core_Library/DepartureBoard.hpp"
#include "../Timetables_Core_Library/Exceptions.hpp"
#include "Reports.hpp"
#include <exception>
#include <iostream>
#include <string>
#include <random>

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
				catch (StopNotFoundException ex) {
					wcout << L"Stop " << ex.GetStopName() << L" not found." << endl;
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

	cout << DateTime::ToString(DateTime::Now()) << " : Application has started." << endl;

	cout << DateTime::ToString(DateTime::Now()) << " : Starting data init." << endl;

	DataFeed feed;

	cout << DateTime::ToString(DateTime::Now()) << " : Ending data init." << endl;

	setlocale(LC_ALL, "");

	// GetDepartureBoardReport(feed, L"Klobouènická", DateTime::Now(), 2);

	// GetDepartureBoardReport(feed, L"Malostranská", DateTime::Now(), 3);

	// GetDepartureBoardReport(feed, L"Roztyly", DateTime(23, 50, 00, 28, 02, 2018), 3);
	
	// CpuTimeMicroBenchmark(feed);

	GetJourneysReport(feed, L"Roztyly", L"Dejvická", DateTime::Now() + 3600, 3, 10);

}